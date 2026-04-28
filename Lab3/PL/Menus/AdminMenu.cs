using BLL.DTOs;
using BLL.Interfaces;

namespace PL.Menus;

public class AdminMenu(IServiceManager serviceManager)
{
    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("-----Меню адміністратора-----");
            Console.WriteLine("1. Додати нову послугу\n" +
                              "2. Переглянути послуги\n" +
                              "0. Вийти");

            var choice = MenuHelper.ReadChoiceNumber("Ваш вибір: ", 0, 2);

            switch (choice)
            {
                case 1:
                    CreateNewService();
                    break;
                case 2:
                    ViewServices();
                    break;
                case 0:
                    return;
            }
        }
    }

    private void CreateNewService()
    {
        var title = MenuHelper.ReadRequiredString("Назва: ");
        var description = MenuHelper.ReadRequiredString("Опис: ");
        var price = MenuHelper.ReadDecimal("Ціна: ");

        var dto = new CreateServiceDto
        {
            Title = title,
            Description = description,
            Price = price
        };

        try
        {
            serviceManager.CreateService(dto);
            Console.WriteLine("Успішно створено!");
            MenuHelper.PressAnyKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void ViewServices()
    {
        var services = serviceManager.GetAllServices().ToList();

        if (!services.Any())
        {
            Console.WriteLine("Список порожній!");
        }

        foreach (var s in services)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"# {s.Title} ({s.Price:F2} грн)");
            Console.ResetColor();
        
            if (!string.IsNullOrEmpty(s.Description))
            {
                Console.WriteLine($"  Опис: {s.Description}");
            }
            Console.WriteLine(new string('.', 40));
        }
        MenuHelper.PressAnyKey();
    }
}