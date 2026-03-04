namespace Lab1.Services;

public interface IGroupService
{
    void CreateGroup(Group group);
    List<Group> GetAllGroups();
    void AddStudentToGroup(Guid studentId, Guid groupId);
    void DivideIntoSubgroups(Guid groupId);
}

