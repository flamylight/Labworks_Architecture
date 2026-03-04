using Lab1.Enums;

namespace Lab1;

public class Activity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public ActivityType Type { get; set; }
    public int Hours { get; set; }

    public Activity(ActivityType type, int hours)
    {
        Type = type;
        Hours = hours;
    }
}
