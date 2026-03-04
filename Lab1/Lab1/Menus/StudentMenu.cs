using Lab1.Exceptions;

namespace Lab1.Menus;

public class StudentMenu
{
    private MenuHelper _helper = new MenuHelper();
    private UniversitySimulation _simulation;

    public StudentMenu(UniversitySimulation simulation)
    {
        _simulation = simulation;
    }
    
    public void RunMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Додати студента\n" +
                              "2. Видати комп'ютер студенту\n" +
                              "0. Вийти");

            Console.Write("Твій вибір: ");
            var choice = Console.ReadLine();

            if (int.TryParse(choice, out int parseChoice))
            {
                switch (parseChoice)
                {
                    case 1:
                        CreateStudent();
                        break;
                    case 2:
                        ProvideComputerToStudent();
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

    private void CreateStudent()
    {
        Console.Write("Ім'я студента: ");
        var name = Console.ReadLine();

        Console.Write("Прізвище студента: ");
        var surname = Console.ReadLine();

        bool hasComputer;

        while (true)
        {
            Console.WriteLine("Студент має комп'ютер? ");
            Console.WriteLine("1. Так\n" +
                              "2. Ні");

            var hasComputerString = Console.ReadLine();

            if (hasComputerString == "1")
            {
                hasComputer = true;
                break;
            }

            if (hasComputerString == "2")
            {
                hasComputer = false;
                break;
            }

            Console.WriteLine("Невідома опція!");
        }


        try
        {
            _simulation.CreateStudent(name, surname, hasComputer);
            Console.WriteLine("Успішно додано!");
        }
        catch (UniversityException ex)
        {
            Console.WriteLine(ex.Message);
        }

        _helper.PressAnyKey();
    }

    private void ProvideComputerToStudent()
    {
        Console.Clear();
        var studentsWithoutPc = _simulation.GetStudentsWithoutComputer();

        if (studentsWithoutPc.Count == 0)
        {
            Console.WriteLine("Усі студенти вже мають комп'ютер.");
            _helper.PressAnyKey();
            return;
        }

        Console.WriteLine("Оберіть студента, якому видати комп'ютер:");
        var studentId = _helper.SelectStudent(studentsWithoutPc);

        try
        {
            _simulation.SetStudentComputer(studentId);
            Console.WriteLine("Комп'ютер видано. Тепер студент може вивчати дисципліни.");
        }
        catch (UniversityException ex)
        {
            Console.WriteLine(ex.Message);
        }
            
        _helper.PressAnyKey();
    }
}