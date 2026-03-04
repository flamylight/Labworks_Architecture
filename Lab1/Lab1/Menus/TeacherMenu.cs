using Lab1.Enums;
using Lab1.Exceptions;

namespace Lab1.Menus;

public class TeacherMenu
{
    private readonly MenuHelper _helper = new MenuHelper();
    private readonly UniversitySimulation _simulation;

    public TeacherMenu(UniversitySimulation simulation)
    {
        _simulation = simulation;
    }

    public void RunMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Створити викладача\n" +
                              "2. Призначити викладача на активність дисципліни\n" +
                              "0. Вийти");

            Console.Write("Твій вибір: ");
            var input = _helper.ReadPositiveInt("Твій вибір:");

            switch (input)
            {
                case 1:
                    CreateTeacher();
                    break;
                case 2:
                    AssignTeacherToDisciplineActivity();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Невідома команда!");
                    _helper.PressAnyKey();
                    break;
            }
        }
    }

    private void CreateTeacher()
    {
        Console.Write("Ім'я викладача: ");
        var name = Console.ReadLine();

        Console.Write("Прізвище викладача: ");
        var surname = Console.ReadLine();

        try
        {
            var teacher = _simulation.CreateTeacher(name!, surname!);
            Console.WriteLine($"Створено викладача: {teacher.Name} {teacher.Surname}");
        }
        catch (UniversityException ex)
        {
            Console.WriteLine(ex.Message);
        }

        _helper.PressAnyKey();
    }

    private void AssignTeacherToDisciplineActivity()
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
        var disciplines = _simulation.GetDisciplinesByGroup(groupId);
        if (disciplines.Count == 0)
        {
            Console.WriteLine("Для цієї групи ще немає дисциплін.");
            _helper.PressAnyKey();
            return;
        }

        var disciplineId = _helper.SelectDiscipline(disciplines);
        var discipline = disciplines.First(d => d.Id == disciplineId);

        var teachers = _simulation.GetAllTeachers();
        if (teachers.Count == 0)
        {
            Console.WriteLine("Спочатку створіть хоча б одного викладача.");
            _helper.PressAnyKey();
            return;
        }

        var activityTypes = discipline.Activities.Select(a => a.Type).Distinct().ToList();
        ActivityType selectedType = SelectActivityTypeFromList(activityTypes);

        var teacherId = _helper.SelectTeacher(teachers);

        try
        {
            _simulation.AssignTeacherToActivity(disciplineId, selectedType, teacherId);
            Console.WriteLine("Викладача призначено успішно.");
        }
        catch (UniversityException ex)
        {
            Console.WriteLine(ex.Message);
        }

        _helper.PressAnyKey();
    }

    private ActivityType SelectActivityTypeFromList(List<ActivityType> types)
    {
        while (true)
        {
            Console.WriteLine("Оберіть тип активності:");
            for (int i = 0; i < types.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {types[i]}");
            }

            Console.Write("Вибір: ");
            if (!int.TryParse(Console.ReadLine(), out int number))
            {
                Console.WriteLine("Введіть номер зі списку!");
                continue;
            }

            if (number < 1 || number > types.Count)
            {
                Console.WriteLine("Число вийшло за межі!");
                continue;
            }

            return types[number - 1];
        }
    }
}
