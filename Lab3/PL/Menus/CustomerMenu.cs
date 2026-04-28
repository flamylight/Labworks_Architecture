using BLL.DTOs;
using BLL.Interfaces;

namespace PL.Menus;

public class CustomerMenu(IServiceManager serviceManager,
    IOrderManager orderManager)
{
    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("-----Меню клієнта-----");
            Console.WriteLine("1. Переглянути послуги\n" +
                              "2. Зробити замовлення послуги\n" +
                              "0. Вийти");

            var choice = MenuHelper.ReadChoiceNumber("Ваш вибір: ", 0, 2);

            switch (choice)
            {
                case 1:
                    ViewServices();
                    MenuHelper.PressAnyKey();
                    break;
                case 2:
                    MakeServiceOrder();
                    break;
                case 0:
                    return;
            }
        }
    }

    private List<GetServiceDto> ViewServices()
    {
        var services = serviceManager.GetAllServices().ToList();

        if (!services.Any())
        {
            Console.WriteLine("Список порожній!");
        }
        else
        {
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
        
        return services;
    }

    private void MakeServiceOrder()
    {
        var services = serviceManager.GetAllServices().ToList();

        if (ViewServices().Count == 0)
        {
            MenuHelper.PressAnyKey();
        }
        else
        {
            var choice = MenuHelper.ReadChoiceNumber("Ваш вибір: ", 1, services.Count);

            var clientTitle = MenuHelper.ReadRequiredString("Назва проекту: ");
            var clientDescription = MenuHelper.ReadRequiredString("Опис проекту: ");

            var orderDto = new CreateOrderDto
            {
                Title = clientTitle,
                ClientDescription = clientDescription,
                ServiceIds = new List<Guid>
                {
                    services[choice - 1].Id
                }
            };
            
            orderManager.CreateOrder(orderDto);
            MenuHelper.PressAnyKey();
        }
    }
}