namespace Lab1.Exceptions;

public class NotEnoughStudentsException : UniversityException
{
    public NotEnoughStudentsException() : base("Недостатньо студентів щоб поділити на підгрупи!") { }
}

