using Lab1.Enums;

namespace Lab1;

public class Discipline
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DisciplineType Type { get; set; }
    public Guid GroupId { get; set; }
    public List<Activity> Activities { get; set; } = new();

    public Dictionary<ActivityType, List<Guid>> ActivityTeachers { get; set; } = new();

    public Dictionary<ActivityType, int> CompletedHours { get; set; } = new();

    public bool IsCreditAwarded { get; set; }
    
    public string Name => Type switch
    {
        DisciplineType.BasicsProgramming => "Основи програмування",
        DisciplineType.OOP => "ООП",
        DisciplineType.AlgorithmsAndDataStructures => "Алгоритми та структури даних",
        _ => "Невідома дисципліна"
    };
    
    public int TotalHours => Activities.Sum(a => a.Hours);

    public Discipline(DisciplineType type, Guid groupId)
    {
        Type = type;
        GroupId = groupId;
    }
}
