namespace Lab1;

public interface IUniversityData
{
    List<Student> Students { get; }
    List<Group> Groups { get; }
    List<SubGroup> SubGroups { get; }
    List<Discipline> Disciplines { get; }
    List<Teacher> Teachers { get; }

    void AddStudent(Student student);
    void AddGroup(Group group);
    void AddSubgroup(SubGroup subgroup);
    void AddDiscipline(Discipline discipline);
    void AddTeacher(Teacher teacher);

    Student? GetStudentById(Guid id);
    Group? GetGroupById(Guid id);
    SubGroup? GetSubgroupById(Guid id);
    Discipline? GetDisciplineById(Guid id);
    Teacher? GetTeacherById(Guid id);

    void Serialize();
}

