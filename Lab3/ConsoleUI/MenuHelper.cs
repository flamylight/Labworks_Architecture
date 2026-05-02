namespace ConsoleUI;
using Contracts.DTOs;

public class MenuHelper
{
    public static int ReadChoiceNumber(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            if (!int.TryParse(Console.ReadLine(), out int value))
            {
                Console.WriteLine("Потрібно ввести число!");
                continue;
            }

            if (value < min || value > max)
            {
                Console.WriteLine($"Число має бути в межах від {min} до {max}!");
                continue;
            }

            return value;
        }
    }
    
    public static string ReadRequiredString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Поле не може бути порожнім!");
                continue;
            }

            return input;
        }
    }
    
    public static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);

            if (!decimal.TryParse(Console.ReadLine(), out decimal value))
            {
                Console.WriteLine("Введіть коректне число (наприклад, 100.50)!");
                continue;
            }

            return value;
        }
    }
    
    public static void PrintOrderDetails(GetOrderDto order)
    {
        var originalColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(new string('=', 60));

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($" ЗАМОВЛЕННЯ: {order.Title.ToUpper()}");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(new string('-', 60));

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(" Створено:   ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{order.CreatedAt:dd.MM.yyyy HH:mm}");

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(" Статус:     ");
        Console.ForegroundColor = order.IsDone ? ConsoleColor.Green : ConsoleColor.Yellow;
        Console.WriteLine(order.IsDone ? "Виконано" : "В процесі");

        if (order.FinishedAt.HasValue)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" Завершено:  ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{order.FinishedAt.Value:dd.MM.yyyy HH:mm}");
        }

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(" Тип:        ");
        Console.ForegroundColor = order.IsTurnkey ? ConsoleColor.Magenta : ConsoleColor.Blue;
        Console.WriteLine(order.IsTurnkey ? "Під ключ" : "Окрема послуга");

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(" Опис:       ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(order.ClientDescription);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(new string('-', 60));

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(" ПОСЛУГИ:");

        if (order.OrderServices.Any())
        {
            foreach (var item in order.OrderServices)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("  * ");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{item.Title,-32}");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(" | ");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{item.Price,10:C2}");
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  (список послуг порожній)");
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(new string('-', 60));

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" ЗАГАЛЬНА ВАРТІСТЬ: ");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{order.TotalPrice,16:C2}");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(new string('=', 60));
        Console.WriteLine();

        Console.ForegroundColor = originalColor;
    }
    
    public static void PressAnyKey()
    {
        Console.WriteLine("Натисніть будь яку клавішу щоб продовжити...");
        Console.ReadLine();
    }
}