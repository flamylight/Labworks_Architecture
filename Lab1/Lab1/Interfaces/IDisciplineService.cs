using Lab1.Enums;

namespace Lab1.Services;

public interface IDisciplineService
{
    Discipline CreateDiscipline(DisciplineType type, Guid groupId, List<Activity> activities);
    List<Discipline> GetDisciplinesByGroup(Guid groupId);
    void ConductActivity(Guid disciplineId, ActivityType activityType, int hours);
    void AssignTeacherToActivity(Guid disciplineId, ActivityType activityType, Guid teacherId);
}

