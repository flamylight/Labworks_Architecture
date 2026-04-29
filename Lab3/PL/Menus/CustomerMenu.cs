using BLL.DTOs;
using BLL.Interfaces;

namespace PL.Menus;

public class CustomerMenu(IServiceManager serviceManager,
    IOrderManager orderManager,
    IPackageManager packageManager)
{
    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("-----Меню клієнта-----");
            Console.WriteLine("1. Переглянути послуги\n" +
                              "2. Зробити замовлення послуги\n" +
                              "3. Переглянути пакети послуг (під ключ)\n" +
                              "4. Зробити замовлення пакету послуг\n" +
                              "5. Переглянути замовлення\n" +
                              "6. Портфоліо\n" +
                              "0. Вийти");

            var choice = MenuHelper.ReadChoiceNumber("Ваш вибір: ", 0, 6);

            switch (choice)
            {
                case 1:
                    ViewServices();
                    MenuHelper.PressAnyKey();
                    break;
                case 2:
                    MakeServiceOrder();
                    break;
                case 3:
                    ViewPackages();
                    MenuHelper.PressAnyKey();
                    break;
                case 4:
                    MakePackageOrder();
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

            try
            {
                orderManager.CreateServiceOrder(orderDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            MenuHelper.PressAnyKey();
        }
    }

    private void MakePackageOrder()
    {
        var packages = packageManager.GetAllPackages().ToList();

        if (ViewPackages().Count == 0)
        {
            MenuHelper.PressAnyKey();
        }
        else
        {
            var choice = MenuHelper.ReadChoiceNumber("Ваш вибір: ", 1, packages.Count);

            var clientTitle = MenuHelper.ReadRequiredString("Назва проекту: ");
            var clientDescription = MenuHelper.ReadRequiredString("Опис проекту: ");

            var orderDto = new CreateOrderDto
            {
                Title = clientTitle,
                ClientDescription = clientDescription,
                IsTurnkey = true,
                PackageId = packages[choice - 1].Id
            };

            try
            {
                orderManager.CreateTurnkeyOrder(orderDto);
                Console.WriteLine("Створено!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            MenuHelper.PressAnyKey();
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
            foreach (var order in orders)
            {
                MenuHelper.PrintOrderDetails(order);
            }
        }
        MenuHelper.PressAnyKey();
    }
    
    private List<GetPackageDto> ViewPackages()
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
        return packages;
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
        
        MenuHelper.PressAnyKey();
    }
}