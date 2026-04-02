using Lab1.Enums;

namespace Lab1.Menus;

public class MenuHelper
{
    public void PressAnyKey()
    {
        Console.WriteLine("Натисніть будь яку клавішу щоб продовжити...");
        Console.ReadLine();
    }

    public Guid SelectGroup(List<Group> groups)
    {
        while (true)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                Console.WriteLine($"{i+1}. {groups[i]}");
            }

            int groupNumber = ReadInt("Виберіть групу: ");

            if (groupNumber < 1 || groupNumber > groups.Count)
            {
                Console.WriteLine("Число вийшло за межі!");
                continue;
            }

            return groups[groupNumber - 1].Id;
        }
    }

    public Guid SelectStudent(List<Student> students)
    {
        while (true)
        {
            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine($"{i+1}. {students[i]}");
            }

            int studentNumber = ReadInt("Виберіть студента: ");

            if (studentNumber < 1 || studentNumber > students.Count)
            {
                Console.WriteLine("Число вийшло за межі!");
                continue;
            }

            return students[studentNumber - 1].Id;
        }
    }

    public DisciplineType SelectDisciplineType()
    {
        var values = Enum.GetValues<DisciplineType>();
        while (true)
        {
            Console.WriteLine("Оберіть тип дисципліни:");
            for (int i = 0; i < values.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {values[i]}");
            }

            int number = ReadInt("Вибір: ");

            if (number < 1 || number > values.Length)
            {
                Console.WriteLine("Число вийшло за межі!");
                continue;
            }

            return values[number - 1];
        }
    }

    public ActivityType SelectActivityType()
    {
        var values = Enum.GetValues<ActivityType>();
        while (true)
        {
            Console.WriteLine("Оберіть тип активності:");
            for (int i = 0; i < values.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {values[i]}");
            }

            int number = ReadInt("Вибір: ");

            if (number < 1 || number > values.Length)
            {
                Console.WriteLine("Число вийшло за межі!");
                continue;
            }

            return values[number - 1];
        }
    }

    public int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (!int.TryParse(Console.ReadLine(), out int value))
            {
                Console.WriteLine("Потрібно ввести число!");
                continue;
            }

            return value;
        }
    }

    public ActivityType SelectActivityTypeFromList(List<ActivityType> types)
    {
        while (true)
        {
            Console.WriteLine("Оберіть тип активності:");
            for (int i = 0; i < types.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {types[i]}");
            }

            int number = ReadInt("Вибір: ");

            if (number < 1 || number > types.Count)
            {
                Console.WriteLine("Число вийшло за межі!");
                continue;
            }

            return types[number - 1];
        }
    }

    public Guid SelectTeacher(List<Teacher> teachers)
    {
        while (true)
        {
            for (int i = 0; i < teachers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {teachers[i].Name} {teachers[i].Surname}");
            }

            int number = ReadInt("Виберіть викладача: ");

            if (number < 1 || number > teachers.Count)
            {
                Console.WriteLine("Число вийшло за межі!");
                continue;
            }

            return teachers[number - 1].Id;
        }
    }

    public Guid SelectDiscipline(List<Discipline> disciplines)
    {
        while (true)
        {
            for (int i = 0; i < disciplines.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {disciplines[i].Name} — {disciplines[i].TotalHours} год.");
            }

            int number = ReadInt("Виберіть дисципліну: ");

            if (number < 1 || number > disciplines.Count)
            {
                Console.WriteLine("Число вийшло за межі!");
                continue;
            }

            return disciplines[number - 1].Id;
        }
    }
}