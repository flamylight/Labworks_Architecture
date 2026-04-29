using Lab1.Enums;
using Lab1.Events;

namespace Lab1.Interfaces;

public interface IDisciplineService
{
    Discipline CreateDiscipline(DisciplineType type, Guid groupId, List<Activity> activities);
    List<Discipline> GetDisciplinesByGroup(Guid groupId);
    void ConductActivity(Guid disciplineId, ActivityType activityType, int hours, Guid? subGroupId = null);
    void AssignTeacherToActivity(Guid disciplineId, ActivityType activityType, Guid teacherId);
    public event EventHandler<ProgressUpdatedEventArgs>? ProgressUpdated;
    public bool IsActivityFullyCompleted(Discipline discipline, Group group, ActivityType activityType);
}

