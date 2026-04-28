using BLL.DTOs;

namespace PL;

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
        Console.WriteLine(new string('=', 50));
        Console.WriteLine($"ЗАМОВЛЕННЯ: {order.Title.ToUpper()}");
        Console.WriteLine(new string('-', 50));
    
        Console.WriteLine($"Створено:    {order.CreatedAt:dd.MM.yyyy HH:mm}");
        Console.WriteLine($"Статус:      {(order.IsDone ? "Виконано" : "В процесі")}");
    
        if (order.FinishedAt.HasValue)
            Console.WriteLine($"Завершено:   {order.FinishedAt.Value:dd.MM.yyyy HH:mm}");
        
        Console.WriteLine($"Тип:         {(order.IsTurnkey ? "Під ключ" : "Окрема послуга")}");
        Console.WriteLine($"Опис:        {order.ClientDescription}");
    
        Console.WriteLine(new string('-', 50));
        Console.WriteLine("ПОСЛУГИ:");
    
        if (order.OrderServices.Any())
        {
            foreach (var item in order.OrderServices)
            {
                Console.WriteLine($"  • {item.Title,-30} | {item.Price,8:C2}");
            }
        }
        else
        {
            Console.WriteLine("  (список послуг порожній)");
        }

        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"ЗАГАЛЬНА ВАРТІСТЬ: {order.TotalPrice,28:C2}");
        Console.WriteLine(new string('=', 50));
        Console.WriteLine();
    }
    
    public static void PressAnyKey()
    {
        Console.WriteLine("Натисніть будь яку клавішу щоб продовжити...");
        Console.ReadLine();
    }
}