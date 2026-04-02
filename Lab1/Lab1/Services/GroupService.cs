using Lab1.Exceptions;

namespace Lab1.Services;

public class GroupService : IGroupService
{
    private IUniversityData _data;

    public GroupService(IUniversityData universityData)
    {
        _data = universityData;
    }
    
    public void CreateGroup(Group group)
    {
        ValidateGroup(group);
        _data.AddGroup(group);
    }

    public List<Group> GetAllGroups()
    {
        return _data.Groups;
    }

    public void AddStudentToGroup(Guid studentId, Guid groupId)
    {
        var student = _data.GetStudentById(studentId)!;
        var group = _data.GetGroupById(groupId)!;

        if (student.GroupId != null)
        {
            throw new StudentValidationException("В студента вже є група!");
        }
        
        student.AssignToGroup(groupId);
        group.AddStudent(studentId);

        if (group.SubGroups.Any())
        {
            var random = new Random();
            var subgroupId = group.SubGroups[random.Next(group.SubGroups.Count)];

            var subgroup = _data.GetSubgroupById(subgroupId)!;
            subgroup.AddStudent(studentId);
        }
    }
    
    private void ValidateGroup(Group group)
    {
        if (string.IsNullOrWhiteSpace(group.Name))
        {
            throw new GroupValidationException("Назва групи не може бути порожньою");
        }

        if (group.Course < 1 || group.Course > 6)
        {
            throw new GroupValidationException("Курс має бути в межах від 1 до 6");
        }
    }

    public void DivideIntoSubgroups(Guid groupId)
    {
        var group = _data.GetGroupById(groupId)!;

        if (group.SubGroups.Count != 0)
        {
            throw new GroupAlreadyDividedException();
        }

        if (group.Students.Count < 20 )
        {
            throw new NotEnoughStudentsException(group.Students.Count);
        }

        var subgroup1 = new SubGroup
        {
            Name = group.Name + "(1)"
        };

        var subgroup2 = new SubGroup
        {
            Name = group.Name + "(2)"
        };

        for (int i = 0; i < group.Students.Count; i++)
        {
            if (i < group.Students.Count/2)
            {
                subgroup1.AddStudent(group.Students[i]);
            }
            else
            {
                subgroup2.AddStudent(group.Students[i]);
            }
        }

        _data.SubGroups.Add(subgroup1);
        _data.SubGroups.Add(subgroup2);
        
        group.DivideIntoSubgroups(new List<Guid>
        {
            subgroup1.Id,
            subgroup2.Id
        });
    }
}