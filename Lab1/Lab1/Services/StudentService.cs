using Lab1.Exceptions;
using Lab1.Services;

namespace Lab1;

public class StudentService : IStudentService
{
    private IUniversityData _data;
    
    public StudentService(IUniversityData universityData)
    {
        _data = universityData;
    }
    
    public void CreateStudent(Student student)
    {
        ValidateStudent(student);
        _data.AddStudent(student);
    }

    public List<Student> GetAllStudents()
    {
        return _data.Students;
    }
    
    public void SetStudentComputer(Guid studentId)
    {
        var student = _data.GetStudentById(studentId);
        if (student == null)
        {
            throw new StudentValidationException("Студента не знайдено");
        }

        student.HasComputer = true;
    }

    public List<Student> GetStudentsWithoutComputer()
    {
        var result = new List<Student>();
        foreach (var s in _data.Students)
        {
            if (!s.HasComputer)
            {
                result.Add(s);
            }
        }
        return result;
    }

    private void ValidateStudent(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.Name))
        {
            throw new StudentValidationException("І'мя студента не може бути порожнім");
        }

        if (string.IsNullOrWhiteSpace(student.Surname))
        {
            throw new StudentValidationException("Прізвище студента не може бути порожнім");
        }
    }

}