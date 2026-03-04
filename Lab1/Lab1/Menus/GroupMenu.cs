using Lab1.Exceptions;

namespace Lab1.Menus;

public class GroupMenu
{
    private MenuHelper _helper = new MenuHelper();
    private UniversitySimulation _simulation;
    
    public GroupMenu(UniversitySimulation simulation)
    {
        _simulation = simulation;
    }

    public void RunMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Створити групу\n" +
                              "2. Додати студента до групи\n" +
                              "3. Поділити групу на підгрупи\n" +
                              "0. Вийти");
            
            Console.Write("Твій вибір: ");
            var choice = Console.ReadLine();
            
            if (int.TryParse(choice, out int parseChoice))
            {
                switch (parseChoice)
                {
                    case 1:
                        CreateGroup();
                        break;
                    case 2:
                        AddStudentToGroup();
                        break;
                    case 3:
                        DivideIntoSubgroups();
                        break;
                    case 0:
                        return;
                }
            }
            else
            {
                Console.WriteLine("Невідома команда!");
                _helper.PressAnyKey();
            }
        }
    }

    private void CreateGroup()
    {
        Console.Write("Назва групи: ");
        var name = Console.ReadLine();

        Console.Write("Курс: ");
        int course = _helper.ReadPositiveInt("Курс: ");

        try
        {
            _simulation.CreateGroup(name!, course);
            Console.WriteLine("Успішно додано");
        }
        catch (GroupValidationException ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        _helper.PressAnyKey();
    }

    private void AddStudentToGroup()
    {
        Console.Clear();

        var groups = _simulation.GetAllGroups();

        if (groups.Count == 0)
        {
            Console.WriteLine("Груп поки немає...");
            _helper.PressAnyKey();
            return;
        }

        var groupId = _helper.SelectGroup(groups);

        var students = _simulation.GetAllStudents();

        if (students.Count == 0)
        {
            Console.WriteLine("Студентів поки немає...");
            _helper.PressAnyKey();
            return;
        }

        var studentId = _helper.SelectStudent(students);

        try
        {
            _simulation.AddStudentToGroup(studentId, groupId);
            Console.WriteLine("Успішно додано!");
        }
        catch (StudentValidationException ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        _helper.PressAnyKey();
    }

    private void DivideIntoSubgroups()
    {
        Console.Clear();
        
        var groups = _simulation.GetAllGroups();

        if (groups.Count == 0)
        {
            Console.WriteLine("Груп поки немає...");
            _helper.PressAnyKey();
            return;
        }

        var groupId = _helper.SelectGroup(groups);

        try
        {
            _simulation.DivideIntoSubgroups(groupId);
            Console.WriteLine("Успішно поділено!");
        }
        catch (NotEnoughStudentsException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (GroupAlreadyDividedException ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        _helper.PressAnyKey();
    }
}