using BLL.DTOs;
using BLL.Interfaces;

namespace PL.Menus;

public class AdminMenu(IServiceManager serviceManager, 
    IOrderManager orderManager,
    IPackageManager packageManager)
{
    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("-----Меню адміністратора-----");
            Console.WriteLine("1. Додати нову послугу\n" +
                              "2. Створити пакет послуг (під ключ)\n" +
                              "3. Переглянути послуги\n" +
                              "4. Переглянути пакети послуг\n" +
                              "5. Переглянути замовлення\n" +
                              "6. Портфоліо\n" +
                              "0. Вийти");

            var choice = MenuHelper.ReadChoiceNumber("Ваш вибір: ", 0, 6);

            switch (choice)
            {
                case 1:
                    CreateNewService();
                    break;
                case 2:
                    CreateNewPackage();
                    break;
                case 3:
                    ViewServices();
                    MenuHelper.PressAnyKey();
                    break;
                case 4:
                    ViewPackages();
                    break;
                case 5:
                    ViewOrders();
                    break;
                case 6:
                    ViewPortfolio();
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

    private void CreateNewPackage()
    {
        var services = serviceManager.GetAllServices().ToList();

        if (!services.Any())
        {
            Console.WriteLine("Послуг поки немає");
        }
        else
        {
            var title = MenuHelper.ReadRequiredString("Назва: ");
            var description = MenuHelper.ReadRequiredString("Опис: ");

            ViewServices();

            var selectedServices = SelectServices(services);

            try
            {
                packageManager.CreatePackage(new CreatePackageDto
                {
                    Title = title,
                    Description = description,
                    Services = selectedServices.Select(s => s.Id).ToList()
                });
                Console.WriteLine("Успішно створено!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            MenuHelper.PressAnyKey();
        }
    }

    private void ViewPackages()
    {
        var packages = packageManager.GetAllPackages().ToList();

        if (!packages.Any())
        {
            Console.WriteLine("Доступних пакетів «Під ключ» поки немає.");
        }
        else
        {
            Console.WriteLine("\n=== ДОСТУПНІ ПАКЕТИ «ПІД КЛЮЧ» ===");

            for (int i = 0; i < packages.Count; i++)
            {
                var p = packages[i];
        
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{i + 1} # ПАКЕТ: {p.Title.ToUpper()}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  ЦІНА: {p.TotalPrice:F2} грн");
                Console.ResetColor();

                if (!string.IsNullOrEmpty(p.Description))
                {
                    Console.WriteLine($"  Про пакет: {p.Description}");
                }
                
                Console.WriteLine("  Склад пакету:");
                foreach (var ps in p.PackageServicesItemDto)
                {
                    Console.WriteLine($"    - {ps.Title} ({ps.Price:F2} грн)"); 
                }

                Console.WriteLine(new string('-', 45));
            }
        }
        MenuHelper.PressAnyKey();
    }

    private void ViewPortfolio()
    {
        var portfolioItems = orderManager.GetPortfolioOrders().ToList();

        if (!portfolioItems.Any())
        {
            Console.WriteLine("Портфоліо порожнє");
        }
        else
        {
            for (int i = 0; i < portfolioItems.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"[{i+1}] ");
                Console.ResetColor();
                
                MenuHelper.PrintOrderDetails(portfolioItems[i]);
            }
        }
        
        AddToPortfolio();
    }

    private void AddToPortfolio()
    {
        Console.WriteLine("Добавити роботу у портфоліо?");
        Console.WriteLine("1. Так\n" +
                          "2. Ні");

        var choice = MenuHelper.ReadChoiceNumber("Ваш вибір: ", 1, 2);

        switch (choice)
        {
            case 1:
                var doneOrders = orderManager.GetDoneOrders().ToList();

                if (!doneOrders.Any())
                {
                    Console.WriteLine("Поки немає виконаних робіт");
                }
                else
                {
                    foreach (var order in doneOrders)
                    {
                        MenuHelper.PrintOrderDetails(order);
                    }
                
                    var choiceOrder = MenuHelper.ReadChoiceNumber(
                        "Виберіть замовлення: ", 1, doneOrders.Count);
                    
                    orderManager.MarkAsPortfolio(doneOrders[choiceOrder - 1].Id);
                    Console.WriteLine("Успішно додано!");
                }
                MenuHelper.PressAnyKey();
                break;
            case 2:
                return;
        }
    }

    private List<GetServiceDto> SelectServices(List<GetServiceDto> services)
    {
        List<GetServiceDto> selectedServices = new();
        bool isDone = false;
        
        while (!isDone)
        {
            var serviceNumber = MenuHelper.ReadChoiceNumber(
                "Виберіть сервіс: ", 1, services.Count);
            selectedServices.Add(services[serviceNumber - 1]);
                
            Console.WriteLine("Вибрати ще?\n" +
                              "1. Так\n" +
                              "2. Ні");

            var choice = MenuHelper.ReadChoiceNumber("Вибір: ", 1, 2);

            switch (choice)
            {
                case 1:
                    continue;
                case 2:
                    isDone = true;
                    break;
            }
        }
        return selectedServices;
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
        CompleteOrder(orders);
    }

    private void CompleteOrder(List<GetOrderDto> orders)
    {
        Console.WriteLine("1. Виконати замовлення\n" +
                          "0. Вийти");

        var choice = MenuHelper.ReadChoiceNumber("Ваш вибір: ", 0, 1);

        switch (choice)
        {
            case 0:
                return;
            case 1:
                var orderNumber = MenuHelper.ReadChoiceNumber(
                    "Номер замовлення: ", 1, orders.Count);
                if (orders[orderNumber - 1].IsDone)
                {
                    Console.WriteLine("Замовлення вже виконано");
                }
                else
                {
                    try
                    {
                        orderManager.MarkAsDone(orders[orderNumber - 1].Id);
                        Console.WriteLine("Виконано!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                MenuHelper.PressAnyKey();
                break;
        }
    }
}