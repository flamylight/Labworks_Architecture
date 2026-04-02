namespace Lab1.Exceptions;

public class NotEnoughStudentsException : UniversityException
{
    public NotEnoughStudentsException(int count) 
        : base($"Недостатньо студентів щоб поділити на підгрупи! ({count}/20)") { }
}

