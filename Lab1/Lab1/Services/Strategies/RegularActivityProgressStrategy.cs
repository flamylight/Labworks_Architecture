using Lab1.Enums;

namespace Lab1.Services.Strategies;

public class RegularActivityProgressStrategy : IActivityProgressStrategy
{
    public bool CanHandle(ActivityType activityType)
    {
        return activityType != ActivityType.Lab;
    }

    public void AddHours(Discipline discipline, Group group, ActivityType activityType, int hours, Guid? subGroupId = null)
    {
        int planned = GetPlannedHours(discipline, activityType);
        int current = GetCompletedHours(discipline, activityType);
        discipline.CompletedHours[activityType] = Math.Min(current + hours, planned);
    }

    private int GetCompletedHours(Discipline discipline, ActivityType activityType, Guid? subGroupId = null)
    {
        return discipline.CompletedHours.TryGetValue(activityType, out var hours) ? hours : 0;
    }

    private int GetPlannedHours(Discipline discipline, ActivityType activityType)
    {
        return discipline.Activities.Where(a => a.Type == activityType).Sum(a => a.Hours);
    }

    public bool IsFullyCompleted(Discipline discipline, Group group, ActivityType activityType)
    {
        int planned = GetPlannedHours(discipline, activityType);
        int completed = GetCompletedHours(discipline, activityType);
        return completed >= planned;
    }
}

