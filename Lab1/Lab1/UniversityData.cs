using System.Text.Json;

namespace Lab1;

public class UniversityData : IUniversityData
{
    public List<Student> Students { get; set; } = new();
    public List<Group> Groups { get; set; } = new();
    public List<SubGroup> SubGroups { get; set; } = new();
    public List<Discipline> Disciplines { get; set; } = new();
    public List<Teacher> Teachers { get; set; } = new();

    public void AddStudent(Student student)
    {
        Students.Add(student);
    }

    public void AddGroup(Group group)
    {
        Groups.Add(group);
    }

    public void AddSubgroup(SubGroup subgroup)
    {
        SubGroups.Add(subgroup);
    }

    public void AddDiscipline(Discipline discipline)
    {
        Disciplines.Add(discipline);
    }

    public void AddTeacher(Teacher teacher)
    {
        Teachers.Add(teacher);
    }

    public Student? GetStudentById(Guid id)
    {
        return Students.FirstOrDefault(s => s.Id == id);
    }

    public Group? GetGroupById(Guid id)
    {
        return Groups.FirstOrDefault(g => g.Id == id);
    }

    public SubGroup? GetSubgroupById(Guid id)
    {
        return SubGroups.FirstOrDefault(s => s.Id == id);
    }

    public Discipline? GetDisciplineById(Guid id)
    {
        return Disciplines.FirstOrDefault(d => d.Id == id);
    }

    public Teacher? GetTeacherById(Guid id)
    {
        return Teachers.FirstOrDefault(t => t.Id == id);
    }

    public static UniversityData Deserialize(string path)
    {
        if (!File.Exists(path))
        {
            return new UniversityData();
        }

        var json = File.ReadAllText(path);

        var data = JsonSerializer.Deserialize<UniversityData>(json);

        return data ?? new UniversityData();
    }
    
    public void Serialize()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(this, options);
        File.WriteAllText("data.json", json);
    }
}