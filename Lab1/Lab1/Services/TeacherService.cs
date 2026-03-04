using Lab1.Exceptions;

using Lab1.Services;

namespace Lab1;

public class TeacherService : ITeacherService
{
    private readonly IUniversityData _data;

    public TeacherService(IUniversityData data)
    {
        _data = data;
    }

    public Teacher CreateTeacher(Teacher teacher)
    {
        if (string.IsNullOrWhiteSpace(teacher.Name))
            throw new TeacherValidationException("Ім'я викладача не може бути порожнім.");

        if (string.IsNullOrWhiteSpace(teacher.Surname))
            throw new TeacherValidationException("Прізвище викладача не може бути порожнім.");
        
        _data.AddTeacher(teacher);
        return teacher;
    }

    public List<Teacher> GetAllTeachers() => _data.Teachers;
}
