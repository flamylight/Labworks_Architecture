namespace Lab1.Exceptions;

public class GroupAlreadyDividedException : UniversityException
{
    public GroupAlreadyDividedException() : base("Група вже поділена!") { }
}

