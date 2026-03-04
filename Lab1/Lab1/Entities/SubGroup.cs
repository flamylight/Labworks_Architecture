namespace Lab1;

public class SubGroup
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public List<Guid> Students { get; set; } = new();

    public void AddStudent(Guid studentId)
    {
        Students.Add(studentId);
    }
}