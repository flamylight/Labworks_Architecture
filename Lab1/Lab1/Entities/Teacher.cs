namespace Lab1;

public class Teacher
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } 
    public string Surname { get; set; } 

    public Teacher(string name, string surname)
    {
        Name = name;
        Surname = surname;
    }

    public override string ToString() => $"{Name} {Surname}";
}
