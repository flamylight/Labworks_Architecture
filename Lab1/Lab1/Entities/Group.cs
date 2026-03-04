namespace Lab1;

public class Group
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public int Course { get; set; }
    public List<Guid> Students { get; set; } = new();
    public List<Guid> SubGroups { get; set; } = new();

    public Group(string name, int course)
    {
        Name = name;
        Course = course;
    }

    public void AddStudent(Guid studentId)
    {
        Students.Add(studentId);
    }

    public void DivideIntoSubgroups(List<Guid> subgroups)
    {
        SubGroups = subgroups;
    }

    public override string ToString()
    {
        return $"{Name} (курс {Course})";
    }
}