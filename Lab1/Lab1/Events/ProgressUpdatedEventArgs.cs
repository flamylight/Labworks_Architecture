using Lab1.Enums;

namespace Lab1.Events;

public class ProgressUpdatedEventArgs : EventArgs
{
    public Discipline Discipline { get; }
    public Group Group { get; }
    public ActivityType ActivityType { get; }
    public int AddedHours { get; }
    public Guid? SubGroupId { get; }

    public ProgressUpdatedEventArgs(Discipline discipline, Group group, ActivityType activityType, int addedHours, Guid? subGroupId)
    {
        Discipline = discipline;
        Group = group;
        ActivityType = activityType;
        AddedHours = addedHours;
        SubGroupId = subGroupId;
    }
}

