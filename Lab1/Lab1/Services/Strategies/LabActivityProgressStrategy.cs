using Lab1.Enums;
using Lab1.Exceptions;

namespace Lab1.Services.Strategies;

public class LabActivityProgressStrategy : IActivityProgressStrategy
{
    public bool CanHandle(ActivityType activityType)
    {
        return activityType == ActivityType.Lab;
    }

    public void AddHours(Discipline discipline, Group group, ActivityType activityType, int hours, Guid? subGroupId = null)
    {
        if (subGroupId == null)
            throw new DisciplineValidationException("Для лабораторної потрібно передати ID підгрупи.");

        if (!group.SubGroups.Contains(subGroupId.Value))
            throw new DisciplineValidationException("Підгрупа не належить до цієї групи.");

        if (!discipline.SubGroupCompletedHours.ContainsKey(activityType))
            discipline.SubGroupCompletedHours[activityType] = new Dictionary<Guid, int>();

        if (!discipline.SubGroupCompletedHours[activityType].ContainsKey(subGroupId.Value))
            discipline.SubGroupCompletedHours[activityType][subGroupId.Value] = 0;

        int planned = GetPlannedHours(discipline, activityType);
        int current = GetCompletedHours(discipline, activityType, subGroupId);
        discipline.SubGroupCompletedHours[activityType][subGroupId.Value] = Math.Min(current + hours, planned);
    }

    private int GetCompletedHours(Discipline discipline, ActivityType activityType, Guid? subGroupId = null)
    {
        if (subGroupId == null)
            return 0;
        
        if (!discipline.SubGroupCompletedHours.TryGetValue(activityType, out var bySubGroup))
            return 0;

        return bySubGroup.TryGetValue(subGroupId.Value, out var hours) ? hours : 0;
    }

    private int GetPlannedHours(Discipline discipline, ActivityType activityType)
    {
        return discipline.Activities.Where(a => a.Type == activityType).Sum(a => a.Hours);
    }
    
    public bool IsFullyCompleted(Discipline discipline, Group group, ActivityType activityType)
    {
        int planned = GetPlannedHours(discipline, activityType);
        if (planned == 0)
            return true;

        if (!discipline.SubGroupCompletedHours.TryGetValue(activityType, out var bySubGroup))
            return false;

        foreach (var subGroupId in group.SubGroups)
        {
            int completed = bySubGroup.TryGetValue(subGroupId, out var hours) ? hours : 0;
            if (completed < planned)
                return false;
        }

        return true;
    }
}

