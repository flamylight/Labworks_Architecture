using BLL.DTOs;
using BLL.Interfaces;

namespace PL.Menus;

public class AdminMenu(IServiceManager serviceManager, IOrderManager orderManager)
{
    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("-----Меню адміністратора-----");
            Console.WriteLine("1. Додати нову послугу\n" +
                              "2. Переглянути послуги\n" +
                              "3. Переглянути замовлення\n" +
                              "0. Вийти");

            var choice = MenuHelper.ReadChoiceNumber("Ваш вибір: ", 0, 3);

            switch (choice)
            {
                case 1:
                    CreateNewService();
                    break;
                case 2:
                    ViewServices();
                    MenuHelper.PressAnyKey();
                    break;
                case 3:
                    ViewOrders();
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

        for (int i = 0; i < services.Count; i++)
        {
            var s = services[i];
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{i+1} # {s.Title} ({s.Price:F2} грн)");
            Console.ResetColor();
        
            if (!string.IsNullOrEmpty(s.Description))
            {
                Console.WriteLine($"  Опис: {s.Description}");
            }
            Console.WriteLine(new string('.', 40));
        }
    }

    private void ViewOrders()
    {
        var orders = orderManager.GetAllOrders().ToList();

        if (!orders.Any())
        {
            Console.WriteLine("Список порожній!");
        }
        else
        {
            for (int i = 0; i < orders.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"[{i+1}] ");
                Console.ResetColor();
                MenuHelper.PrintOrderDetails(orders[i]);
            }
        }
        MenuHelper.PressAnyKey();
    }
}