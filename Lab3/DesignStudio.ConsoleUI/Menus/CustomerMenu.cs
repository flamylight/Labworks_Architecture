using System.Net.Http.Json;
using Contracts.DTOs;

namespace ConsoleUI.Menus;

public class CustomerMenu(HttpClient client)
{
    public async Task  Run()
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
                    await ViewServices();
                    MenuHelper.PressAnyKey();
                    break;
                case 2:
                    await MakeServiceOrder();
                    break;
                case 3:
                    await ViewPackages();
                    MenuHelper.PressAnyKey();
                    break;
                case 4:
                    await MakePackageOrder();
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

    private async Task<List<GetServiceDto>?> ViewServices()
    {
        var services = await client.GetFromJsonAsync<List<GetServiceDto>>("api/Service");

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

        return services;
    }

    private async Task MakeServiceOrder()
    {
        var services = await ViewServices();

        if (services == null || !services.Any())
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
                ServiceId = services[choice - 1].Id
            };

            try
            {
                var response = await client.PostAsJsonAsync("api/Order/service", orderDto);
                await MenuHelper.CheckResponse(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            MenuHelper.PressAnyKey();
        }
    }

    private async Task MakePackageOrder()
    {
        var packages = await ViewPackages();

        if (packages == null || !packages.Any())
        {
            MenuHelper.PressAnyKey();
        }
        else
        {
            var choice = MenuHelper.ReadChoiceNumber("Ваш вибір: ", 1, packages.Count);

            var clientTitle = MenuHelper.ReadRequiredString("Назва проекту: ");
            var clientDescription = MenuHelper.ReadRequiredString("Опис проекту: ");

            var orderDto = new CreateTurnkeyOrderDto()
            {
                Title = clientTitle,
                ClientDescription = clientDescription,
                PackageId = packages[choice - 1].Id
            };

            try
            {
                var response = await client.PostAsJsonAsync("api/Order/turnkey", orderDto);
                await MenuHelper.CheckResponse(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            MenuHelper.PressAnyKey();
        }
    }

    private async Task ViewOrders()
    {
        var orders = await client.GetFromJsonAsync<List<GetOrderDto>>("api/Order");

        if (orders == null || !orders.Any())
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

    private async Task<List<GetPackageDto>?> ViewPackages()
    {
        var packages = await client.GetFromJsonAsync<List<GetPackageDto>>("api/Package");

        if (packages == null || !packages.Any())
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

    private async Task ViewPortfolio()
    {
        var portfolioItems = await client.GetFromJsonAsync<List<GetOrderDto>>("api/Order/portfolio");

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

        MenuHelper.PressAnyKey();
    }
}