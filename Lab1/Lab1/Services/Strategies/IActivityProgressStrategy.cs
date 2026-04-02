using Lab1.Enums;

namespace Lab1.Services.Strategies;

public interface IActivityProgressStrategy
{
    bool CanHandle(ActivityType activityType);
    
    void AddHours(Discipline discipline, Group group, ActivityType activityType, int hours, Guid? subGroupId = null);
    bool IsFullyCompleted(Discipline discipline, Group group, ActivityType activityType);
}

