namespace Lab1.Exceptions;

public class StudentValidationException : UniversityException
{
    public StudentValidationException(string message) : base(message) { }
}

