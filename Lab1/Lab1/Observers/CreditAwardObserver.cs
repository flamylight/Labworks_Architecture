using Lab1.Enums;
using Lab1.Events;
using Lab1.Interfaces;

namespace Lab1.Observers;

public class CreditAwardObserver
{
    public void Subscribe(IDisciplineService disciplineService)
    {
        disciplineService.ProgressUpdated += OnProgressUpdated;
    }

    public void Unsubscribe(IDisciplineService disciplineService)
    {
        disciplineService.ProgressUpdated -= OnProgressUpdated;
    }

    private void OnProgressUpdated(object? sender, ProgressUpdatedEventArgs e)
    {
        var discipline = e.Discipline;
        var group = e.Group;

        if (discipline.IsCreditAwarded)
            return;
        
        bool hasCreditActivity = false;
        foreach (var activity in discipline.Activities)
        {
            if (activity.Type == ActivityType.Credit)
            {
                hasCreditActivity = true;
                break;
            }
        }

        if (!hasCreditActivity)
            return;

        if (sender is not IDisciplineService disciplineService)
            return;
        
        bool allCompleted = AreAllNonCreditActivitiesCompleted(disciplineService, discipline, group);
        if (!allCompleted)
            return;
        
        int creditHours = 0;
        foreach (var activity in discipline.Activities)
        {
            if (activity.Type == ActivityType.Credit)
            {
                creditHours += activity.Hours;
            }
        }

        discipline.SetCredit(creditHours);
    }

    private static bool AreAllNonCreditActivitiesCompleted(IDisciplineService disciplineService, Discipline discipline, Group group)
    {
        var plannedTypes = new List<ActivityType>();
        foreach (var activity in discipline.Activities)
        {
            if (!plannedTypes.Contains(activity.Type))
            {
                plannedTypes.Add(activity.Type);
            }
        }

        foreach (var type in plannedTypes)
        {
            if (type == ActivityType.Credit)
                continue;

            if (!disciplineService.IsActivityFullyCompleted(discipline, group, type))
                return false;
        }

        return true;
    }
}
