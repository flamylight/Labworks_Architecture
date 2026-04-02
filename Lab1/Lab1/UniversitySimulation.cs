using Lab1.Enums;
using Lab1.Interfaces;
using Lab1.Services;

namespace Lab1;

public class UniversitySimulation
{
    private readonly IStudentService _studentService;
    private readonly IGroupService _groupService;
    private readonly IDisciplineService _disciplineService;
    private readonly ITeacherService _teacherService;

    public UniversitySimulation(IStudentService studentService, IGroupService groupService, IDisciplineService disciplineService, ITeacherService teacherService)
    {
        _studentService = studentService;
        _groupService = groupService;
        _disciplineService = disciplineService;
        _teacherService = teacherService;
    }
    
    public void CreateStudent(string name, string surname, bool hasComputer)
    {
        _studentService.CreateStudent(new Student(name, surname, hasComputer));
    }
    
    public void CreateGroup(string name, int course)
    {
        _groupService.CreateGroup(new Group(name, course));
    }
    
    public List<Group> GetAllGroups()
    {
        return _groupService.GetAllGroups();
    }
    
    public List<Student> GetAllStudents()
    {
        return _studentService.GetAllStudents();
    }
    
    public List<Student> GetStudentsWithoutComputer()
    {
        return _studentService.GetStudentsWithoutComputer();
    }

    public void SetStudentComputer(Guid studentId)
    {
        _studentService.SetStudentComputer(studentId);
    }

    public void AddStudentToGroup(Guid studentId, Guid groupId)
    {
        _groupService.AddStudentToGroup(studentId, groupId);
    }
    
    public void DivideIntoSubgroups(Guid groupId)
    {
        _groupService.DivideIntoSubgroups(groupId);
    }
    
    public Discipline CreateDiscipline(DisciplineType type, Guid groupId, List<Activity> activities)
    {
        return _disciplineService.CreateDiscipline(type, groupId, activities);
    }
    
    public List<Discipline> GetDisciplinesByGroup(Guid groupId)
    {
        return _disciplineService.GetDisciplinesByGroup(groupId);
    }
    
    public void ConductActivity(Guid disciplineId, ActivityType activityType, int hours, Guid? subGroupId = null)
    {
        _disciplineService.ConductActivity(disciplineId, activityType, hours, subGroupId);
    }
    
    public Teacher CreateTeacher(string name, string surname)
    {
        return _teacherService.CreateTeacher(new Teacher(name, surname));
    }
    
    public List<Teacher> GetAllTeachers()
    {
        return _teacherService.GetAllTeachers();
    }
    
    public void AssignTeacherToActivity(Guid disciplineId, ActivityType activityType, Guid teacherId)
    {
        _disciplineService.AssignTeacherToActivity(disciplineId, activityType, teacherId);
    }
}
