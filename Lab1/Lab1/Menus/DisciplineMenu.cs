using Lab1.Enums;
using Lab1.Exceptions;

namespace Lab1.Menus;

public class DisciplineMenu
{
    private readonly MenuHelper _helper = new MenuHelper();
    private readonly UniversitySimulation _simulation;

    public DisciplineMenu(UniversitySimulation simulation)
    {
        _simulation = simulation;
    }

    public void RunMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Створити дисципліну\n" +
                              "2. Переглянути дисципліни групи\n" +
                              "3. Провести активність дисципліни\n" +
                              "0. Вийти");

            Console.Write("Твій вибір: ");
            var choice = Console.ReadLine();

            if (!int.TryParse(choice, out int parseChoice))
            {
                Console.WriteLine("Невідома команда!");
                _helper.PressAnyKey();
                continue;
            }

            switch (parseChoice)
            {
                case 1:
                    CreateDiscipline();
                    break;
                case 2:
                    ViewDisciplinesByGroup();
                    break;
                case 3:
                    ConductActivityFlow();
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

    private void CreateDiscipline()
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
        var type = _helper.SelectDisciplineType();

        var activities = new List<Activity>();
        int totalHours = 0;

        Console.WriteLine("Додайте активності (мінімум 64 год загалом).");

        while (true)
        {
            var activityType = _helper.SelectActivityType();

            int hours = _helper.ReadPositiveInt($"Кількість годин для {activityType}: ");
            activities.Add(new Activity(activityType, hours));
            totalHours += hours;

            Console.WriteLine($"Поточна сума годин: {totalHours}");

            Console.WriteLine("Додати ще активність?");
            Console.WriteLine("1. Так\n2. Ні");
            var more = Console.ReadLine();

            if (more == "2")
            {
                break;
            }
        }

        try
        {
            var discipline = _simulation.CreateDiscipline(type, groupId, activities);
            Console.WriteLine($"Дисципліну '{discipline.Name}' створено. Загалом годин: {discipline.TotalHours}");
        }
        catch (UniversityException ex)
        {
            Console.WriteLine(ex.Message);
        }

        _helper.PressAnyKey();
    }

    private void ViewDisciplinesByGroup()
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
            Console.WriteLine("Для цієї групи дисциплін ще немає.");
            _helper.PressAnyKey();
            return;
        }

        for (int i = 0; i < disciplines.Count; i++)
        {
            var d = disciplines[i];
            Console.WriteLine($"{i + 1}. {d.Name} — заплановано {d.TotalHours} год."
                              + (d.Type == DisciplineType.BasicsProgramming && d.IsCreditAwarded ? " (Залік: автоматично виставлено)" : ""));

            var activityTypes = new Dictionary<ActivityType, int>();
            foreach (var activity in d.Activities)
            {
                if (activityTypes.ContainsKey(activity.Type))
                {
                    activityTypes[activity.Type] += activity.Hours;
                }
                else
                {
                    activityTypes[activity.Type] = activity.Hours;
                }
            }
            
            foreach (var kv in activityTypes)
            {
                var type = kv.Key;
                var planned = kv.Value;
                d.CompletedHours.TryGetValue(type, out int done);
                var status = done >= planned ? "[виконано]" : "[у процесі]";
                Console.WriteLine($"   - {type}: {done}/{planned} год. {status}");
            }
        }

        _helper.PressAnyKey();
    }

    private void ConductActivityFlow()
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
            Console.WriteLine("Для цієї групи дисциплін ще немає.");
            _helper.PressAnyKey();
            return;
        }

        var disciplineId = _helper.SelectDiscipline(disciplines);
        var discipline = disciplines.First(d => d.Id == disciplineId);

        var activityTypes = discipline.Activities.Select(a => a.Type).Distinct().ToList();
        var selectedType = _helper.SelectActivityTypeFromList(activityTypes);

        int hours = _helper.ReadPositiveInt("Скільки годин провести: ");

        try
        {
            _simulation.ConductActivity(disciplineId, selectedType, hours);
            Console.WriteLine("Активність проведено успішно.");
        }
        catch (UniversityException ex)
        {
            Console.WriteLine(ex.Message);
        }

        _helper.PressAnyKey();
    }
}
