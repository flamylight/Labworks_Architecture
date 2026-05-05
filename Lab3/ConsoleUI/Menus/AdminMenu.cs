using System.Net.Http.Json;
using Contracts.DTOs;

namespace ConsoleUI.Menus;

public class AdminMenu(HttpClient client)
{
    public async Task Run()
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
                    await CreateNewService();
                    break;
                case 2:
                    await CreateNewPackage();
                    break;
                case 3:
                    await ViewServices();
                    break;
                case 4:
                    await ViewPackages();
                    break;
                case 5:
                    await ViewOrders();
                    break;
                case 6:
                    await ViewPortfolio();
                    break;
                case 0:
                    return;
            }
        }
    }

    private async Task CreateNewService()
    {
        var title = MenuHelper.ReadRequiredString("Назва: ");
        var description = MenuHelper.ReadRequiredString("Опис: ");
        var price = MenuHelper.ReadDecimal("Ціна: ");

        var request = new CreateServiceDto
        {
            Title = title,
            Description = description,
            Price = price
        };

        try
        {
            await client.PostAsJsonAsync("/api/Order/service", request);
            Console.WriteLine("Успішно створено!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        MenuHelper.PressAnyKey();
    }

    private async Task CreateNewPackage()
    {
        var services = await client.GetFromJsonAsync<List<GetServiceDto>>("/api/service");

        if (services == null || !services.Any())
        {
            Console.WriteLine("Послуг поки немає");
        }
        else
        {
            var title = MenuHelper.ReadRequiredString("Назва: ");
            var description = MenuHelper.ReadRequiredString("Опис: ");

            await ViewServices();

            var selectedServices = SelectServices(services);

            try
            {
                var request = new CreatePackageDto
                {
                    Title = title,
                    Description = description,
                    Services = selectedServices.Select(s => s.Id).ToList()
                };
                
                await client.PostAsJsonAsync("/api/package", request);
                Console.WriteLine("Успішно створено!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            MenuHelper.PressAnyKey();
        }
    }

    private async Task ViewPackages()
    {
        var packages = await client.GetFromJsonAsync<List<GetPackageDto>>("/api/package");

        if (packages == null ||!packages.Any())
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

    private async Task ViewPortfolio()
    {
        var portfolioItems = await client.GetFromJsonAsync<List<GetOrderDto>>("/api/order/portfolio");

        if (portfolioItems == null || !portfolioItems.Any())
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
        
        await AddToPortfolio();
    }

    private async Task AddToPortfolio()
    {
        Console.WriteLine("Добавити роботу у портфоліо?");
        Console.WriteLine("1. Так\n" +
                          "2. Ні");

        var choice = MenuHelper.ReadChoiceNumber("Ваш вибір: ", 1, 2);

        switch (choice)
        {
            case 1:
                var doneOrders = await client.GetFromJsonAsync<List<GetOrderDto>>("/api/order/done");

                if (doneOrders == null || !doneOrders.Any())
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

                    try
                    {
                        await client.PutAsync(
                            $"/api/order/{doneOrders[choiceOrder - 1].Id}/portfolio", null);
                        Console.WriteLine("Успішно додано!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
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

    private async Task ViewServices()
    {
        var services = await client.GetFromJsonAsync<List<GetServiceDto>>("api/service");

        if (services == null || !services.Any())
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
        MenuHelper.PressAnyKey();
    }

    private async Task ViewOrders()
    {
        var orders = await client.GetFromJsonAsync<List<GetOrderDto>>("/api/order");

        if (orders == null || !orders.Any())
        {
            Console.WriteLine("Список порожній!");
            MenuHelper.PressAnyKey();
            return;
        }
        for (int i = 0; i < orders.Count; i++)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"[{i+1}] ");
            Console.ResetColor();
            MenuHelper.PrintOrderDetails(orders[i]);
        }
        CompleteOrder(orders);
    }

    private async void CompleteOrder(List<GetOrderDto> orders)
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
                        await client.PutAsync($"api/order/{orders[orderNumber - 1].Id}/done", null);
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