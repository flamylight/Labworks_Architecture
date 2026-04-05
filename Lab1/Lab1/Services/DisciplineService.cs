using Lab1.Enums;
using Lab1.Events;
using Lab1.Exceptions;
using Lab1.Interfaces;
using Lab1.Services.Strategies;

namespace Lab1.Services;

public class DisciplineService : IDisciplineService
{
    private const int MinimumHours = 64;
    private readonly IUniversityData _data;
    private readonly List<IActivityProgressStrategy> _progressStrategies  = new()
    {
        new LabActivityProgressStrategy(),
        new RegularActivityProgressStrategy()
    };

    public event EventHandler<ProgressUpdatedEventArgs>? ProgressUpdated;

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

        if (activities.Any(a => a.Hours <= 0))
        {
            throw new DisciplineValidationException("Кількість годин для активності має бути більшою за 0.");
        }

        if (group.SubGroups == null || group.SubGroups.Count == 0)
        {
            throw new DisciplineValidationException("Спочатку поділіть групу на підгрупи.");
        }

        if (_data.Disciplines.Any(d => d.GroupId == group.Id && d.Type == type))
        {
            throw new DisciplineValidationException("Така дисципліна вже створена для цієї групи.");
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

    public void ConductActivity(Guid disciplineId, ActivityType activityType, int hours, Guid? subGroupId = null)
    {
        var discipline = _data.GetDisciplineById(disciplineId)
            ?? throw new DisciplineValidationException("Дисципліну не знайдено.");

        var group = _data.GetGroupById(discipline.GroupId)
            ?? throw new GroupValidationException("Групу не знайдено.");

        ValidateHours(hours);
        ValidateActivityConduction(discipline, group, activityType);

        var strategy = GetProgressStrategy(activityType);
        strategy.AddHours(discipline, group, activityType, hours, subGroupId);
        OnProgressUpdated(discipline, group, activityType, hours, subGroupId);
    }

    private void ValidateActivityConduction(Discipline discipline, Group group, ActivityType activityType)
    {
        if (!discipline.Activities.Any(a => a.Type == activityType))
            throw new DisciplineValidationException("Ця активність не запланована у дисципліні.");
    
        if (!HasTeachersAssigned(discipline, activityType))
            throw new DisciplineValidationException("На цю активність не призначено жодного викладача.");

        var strategy = GetProgressStrategy(activityType);
        if (strategy.IsFullyCompleted(discipline, group, activityType))
            throw new DisciplineValidationException("Активність вже повністю проведена.");

        ValidatePrerequisites(discipline, group, activityType);
    }

    private void ValidateHours(int hours)
    {
        if (hours <= 0)
            throw new DisciplineValidationException("Кількість годин має бути більшою за 0.");
    }

    private void ValidatePrerequisites(Discipline discipline, Group group, ActivityType activityType)
    {
        if (activityType == ActivityType.ModuleTest || activityType == ActivityType.Exam)
        {
            if (GetPlannedHours(discipline, ActivityType.Lab) > 0 &&
                !GetProgressStrategy(ActivityType.Lab).IsFullyCompleted(discipline, group, ActivityType.Lab))
                throw new DisciplineValidationException(
                    "Без проведених лабораторних робіт група не допускається до МКР/екзамену.");

            if (GetPlannedHours(discipline, ActivityType.CourseWork) > 0 &&
                !GetProgressStrategy(ActivityType.CourseWork).IsFullyCompleted(discipline, group, ActivityType.CourseWork))
                throw new DisciplineValidationException(
                    "Без зданої курсової роботи група не допускається до МКР/екзамену.");
        }
    }

    private void OnProgressUpdated(Discipline discipline, Group group, ActivityType activityType, int addedHours, Guid? subGroupId)
    {
        ProgressUpdated?.Invoke(this, new ProgressUpdatedEventArgs(discipline, group, activityType, addedHours, subGroupId));
    }

    private bool HasTeachersAssigned(Discipline discipline, ActivityType activityType)
    {
        return discipline.ActivityTeachers.TryGetValue(activityType, out var teachers) && teachers.Count > 0;
    
    }

    private int GetPlannedHours(Discipline discipline, ActivityType type)
    {
        return discipline.Activities.Where(a => a.Type == type).Sum(a => a.Hours);
    
    }

    private IActivityProgressStrategy GetProgressStrategy(ActivityType activityType)
    {
        return _progressStrategies.First(s => s.CanHandle(activityType));
    }
    
    public bool IsActivityFullyCompleted(Discipline discipline, Group group, ActivityType activityType)
    {
        var strategy = GetProgressStrategy(activityType);
        return strategy.IsFullyCompleted(discipline, group, activityType);
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
