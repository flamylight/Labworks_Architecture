namespace Lab1.Services;

public interface IStudentService
{
    void CreateStudent(Student student);
    List<Student> GetAllStudents();
    void SetStudentComputer(Guid studentId);
    List<Student> GetStudentsWithoutComputer();
}

