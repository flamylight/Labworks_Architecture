namespace Lab1.Services;

public interface ITeacherService
{
    Teacher CreateTeacher(Teacher teacher);
    List<Teacher> GetAllTeachers();
}

