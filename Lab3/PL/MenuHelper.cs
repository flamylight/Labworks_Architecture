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
    
    public static void PressAnyKey()
    {
        Console.WriteLine("Натисніть будь яку клавішу щоб продовжити...");
        Console.ReadLine();
    }
}