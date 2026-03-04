namespace Lab1;

public class Student
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } 
    public string Surname { get; set; }
    public Guid? GroupId { get; set; }
    public bool HasComputer { get; set; }
    
    public Student(string name, string surname, bool hasComputer)
    {
        Name = name;
        Surname = surname;
        HasComputer = hasComputer;
    }

    public void AssignToGroup(Guid groupId)
    {
        GroupId = groupId;
    }

    public override string ToString()
    {
        return $"{Name} {Surname}";
    }
}