using Lab1.Enums;
using Lab1.Exceptions;

namespace Lab1.Services;

public class DisciplineService : IDisciplineService
{
    private const int MinimumHours = 64;
    private readonly IUniversityData _data;

    public DisciplineService(IUniversityData universityData)
    {
        _data = universityData;
    }

    public Discipline CreateDiscipline(DisciplineType type, Guid groupId, List<Activity> activities)
    {
        var group = _data.GetGroupById(groupId)
            ?? throw new GroupValidationException("Групу не знайдено.");
        
        ValidateDisciplineCreation(type, group, activities);

        var discipline = new Discipline(type, groupId) { Activities = activities };
        _data.AddDiscipline(discipline);
        return discipline;
    }
    
    private void ValidateDisciplineCreation(DisciplineType type, Group group, List<Activity> activities)
    {
        var allowedCourses = AllowedCoursesFor(type);
        if (!allowedCourses.Contains(group.Course))
        {
            throw new DisciplineValidationException("Ця дисципліна не викладається на вказаному курсі.");
        }
        
        if (group.SubGroups == null || group.SubGroups.Count == 0)
        {
            throw new DisciplineValidationException("Спочатку поділіть групу на підгрупи.");
        }
        
        if (_data.Disciplines.Any(d => d.GroupId == group.Id && d.Type == type))
        {
            throw new DisciplineValidationException("Така дисципліна вже створена для цієї групи.");
        }
        
        if (type != DisciplineType.BasicsProgramming && activities.Any(a => a.Type == ActivityType.Credit))
        {
            throw new DisciplineValidationException("Залік можливий лише для дисципліни 'Основи програмування'.");
        }
        
        var totalHours = activities.Sum(a => a.Hours);
        if (totalHours < MinimumHours)
        {
            throw new DisciplineValidationException($"На дисципліну має бути не менше {MinimumHours} аудиторних годин.");
        }
        
        foreach (var studentId in group.Students)
        {
            var student = _data.GetStudentById(studentId);
            if (student != null && !student.HasComputer)
            {
                throw new DisciplineValidationException(
                    "У групі є студенти без комп'ютера/ноутбука — вивчення дисципліни неможливе.");
            }
        }
    }

    public List<Discipline> GetDisciplinesByGroup(Guid groupId)
    {
        return  _data.Disciplines.Where(d => d.GroupId == groupId).ToList();
    }

    public void ConductActivity(Guid disciplineId, ActivityType activityType, int hours)
    {
        var discipline = _data.GetDisciplineById(disciplineId)
            ?? throw new DisciplineValidationException("Дисципліну не знайдено.");

        ValidateActivityConduction(discipline, activityType, hours);

        UpdateCompletedHours(discipline, activityType, hours);
        CheckAndAwardCredit(discipline);
    }

    private void ValidateActivityConduction(Discipline discipline, ActivityType activityType, int hours)
    {
        if (hours <= 0)
            throw new DisciplineValidationException("Кількість годин має бути більшою за 0.");

        if (!discipline.Activities.Any(a => a.Type == activityType))
            throw new DisciplineValidationException("Ця активність не запланована у дисципліні.");

        if (!HasTeachersAssigned(discipline, activityType))
            throw new DisciplineValidationException("На цю активність не призначено жодного викладача.");

        int plannedHours = GetPlannedHours(discipline, activityType);
        int completedHours = GetCompletedHours(discipline, activityType);

        if (completedHours >= plannedHours)
            throw new DisciplineValidationException("Активність вже повністю проведена.");

        ValidatePrerequisites(discipline, activityType);
    }

    private void ValidatePrerequisites(Discipline discipline, ActivityType activityType)
    {
        if (activityType == ActivityType.ModuleTest || activityType == ActivityType.Exam)
        {
            if (GetPlannedHours(discipline, ActivityType.Lab) > 0 &&
                GetCompletedHours(discipline, ActivityType.Lab) < GetPlannedHours(discipline, ActivityType.Lab))
                throw new DisciplineValidationException(
                    "Без проведених лабораторних робіт група не допускається до МКР/екзамену.");

            if (GetPlannedHours(discipline, ActivityType.CourseWork) > 0 &&
                GetCompletedHours(discipline, ActivityType.CourseWork) < GetPlannedHours(discipline, ActivityType.CourseWork))
                throw new DisciplineValidationException(
                    "Без зданої курсової роботи група не допускається до МКР/екзамену.");
        }
    }

    private void UpdateCompletedHours(Discipline discipline, ActivityType activityType, int hours)
    {
        int planned = GetPlannedHours(discipline, activityType);
        int current = GetCompletedHours(discipline, activityType);
        discipline.CompletedHours[activityType] = Math.Min(current + hours, planned);
    }

    private void CheckAndAwardCredit(Discipline discipline)
    {
        if (discipline.Type != DisciplineType.BasicsProgramming || discipline.IsCreditAwarded)
            return;

        var plannedTypes = discipline.Activities.Select(a => a.Type).ToHashSet();
        if (!plannedTypes.Contains(ActivityType.Credit))
            return;

        bool allCompleted = plannedTypes
            .Where(t => t != ActivityType.Credit)
            .All(t => GetCompletedHours(discipline, t) >= GetPlannedHours(discipline, t));

        if (allCompleted)
        {
            int creditHours = GetPlannedHours(discipline, ActivityType.Credit);
            if (creditHours > 0)
                discipline.CompletedHours[ActivityType.Credit] = creditHours;
            
            discipline.IsCreditAwarded = true;
        }
    }

    private bool HasTeachersAssigned(Discipline discipline, ActivityType activityType)
    {
        return discipline.ActivityTeachers.TryGetValue(activityType, out var teachers) && teachers.Count > 0;

    }

    private int GetPlannedHours(Discipline discipline, ActivityType type)
    {
        return discipline.Activities.Where(a => a.Type == type).Sum(a => a.Hours);

    }

    private int GetCompletedHours(Discipline discipline, ActivityType type)
    {
        return discipline.CompletedHours.TryGetValue(type, out var hours) ? hours : 0;

    }

    public void AssignTeacherToActivity(Guid disciplineId, ActivityType activityType, Guid teacherId)
    {
        var discipline = _data.GetDisciplineById(disciplineId)
            ?? throw new DisciplineValidationException("Дисципліну не знайдено.");
        var teacher = _data.GetTeacherById(teacherId)
            ?? throw new TeacherValidationException("Викладача не знайдено.");
        var group = _data.GetGroupById(discipline.GroupId)
            ?? throw new GroupValidationException("Групу не знайдено.");

        ValidateTeacherAssignment(discipline, activityType, teacher, group);

        AddTeacherToActivity(discipline, activityType, teacherId);
    }

    private void ValidateTeacherAssignment(Discipline discipline, ActivityType activityType, Teacher teacher, Group group)
    {
        if (!discipline.Activities.Any(a => a.Type == activityType))
            throw new DisciplineValidationException("У цій дисципліні немає такої активності.");

        if (IsTeacherAssignedToOtherDiscipline(discipline.Id, teacher.Id))
            throw new DisciplineValidationException("Викладач вже призначений на іншу дисципліну.");

        var currentTeachersCount = GetAllTeacherIdsForDiscipline(discipline).Count;
        int maxTeachers = group.SubGroups.Count + 1;
        bool alreadyAssignedHere = GetAllTeacherIdsForDiscipline(discipline).Contains(teacher.Id);

        if (!alreadyAssignedHere && currentTeachersCount >= maxTeachers)
            throw new DisciplineValidationException("Перевищено ліміт викладачів для дисципліни (підгрупи + лектор).");
    }

    private bool IsTeacherAssignedToOtherDiscipline(Guid currentDisciplineId, Guid teacherId)
    {
        return _data.Disciplines
            .Where(d => d.Id != currentDisciplineId)
            .Any(d => GetAllTeacherIdsForDiscipline(d).Contains(teacherId));
    }
    

    private void AddTeacherToActivity(Discipline discipline, ActivityType activityType, Guid teacherId)
    {
        if (!discipline.ActivityTeachers.TryGetValue(activityType, out var list))
        {
            list = new List<Guid>();
            discipline.ActivityTeachers[activityType] = list;
        }

        if (!list.Contains(teacherId))
            list.Add(teacherId);
    }


    private HashSet<Guid> GetAllTeacherIdsForDiscipline(Discipline discipline) =>
        discipline.ActivityTeachers.Values
            .SelectMany(list => list)
            .ToHashSet();

    private static List<int> AllowedCoursesFor(DisciplineType type) => type switch
    {
        DisciplineType.BasicsProgramming => new List<int> { 1 },
        DisciplineType.OOP => new List<int> { 1, 2 },
        DisciplineType.AlgorithmsAndDataStructures => new List<int> { 2 },
        _ => new List<int>()
    };
}
