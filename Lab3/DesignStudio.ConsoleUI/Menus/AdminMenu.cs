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
                              "7. Оновити послугу\n" +
                              "8. Видалити замовлення\n" +
                              "9. Видалити послугу\n" +
                              "10. Додати послугу в пакет\n" +
                              "11. Видалити пакет\n" +
                              "0. Вийти");

            var choice = MenuHelper.ReadChoiceNumber("Ваш вибір: ", 0, 11);

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
                    MenuHelper.PressAnyKey();
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
                case 7:
                    await UpdateService();
                    break;
                case 8:
                    await DeleteOrder();
                    break;
                case 9:
                    await DeleteService();
                    break;
                case 10:
                    await AddServiceToPackage();
                    break;
                case 11:
                    await DeletePackage();
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
            var response = await client.PostAsJsonAsync("/api/Service", request);

            await MenuHelper.CheckResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        MenuHelper.PressAnyKey();
    }


    private async Task CreateNewPackage()
    {
        var services = await client.GetFromJsonAsync<List<GetServiceDto>>("/api/Service");

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
                
                var response = await client.PostAsJsonAsync("/api/Package", request);

                await MenuHelper.CheckResponse(response);
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
        var packages = await client.GetFromJsonAsync<List<GetPackageDto>>("/api/Package");

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
        var portfolioItems = await client.GetFromJsonAsync<List<GetOrderDto>>("/api/Order/portfolio");

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
                var doneOrders = await client.GetFromJsonAsync<List<GetOrderDto>>("/api/Order/done");

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
                        var response = await client.PutAsync(
                            $"/api/order/{doneOrders[choiceOrder - 1].Id}/portfolio", null);
                        await MenuHelper.CheckResponse(response);
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
                        var response = await client.PutAsync($"api/order/{orders[orderNumber - 1].Id}/done", null);
                        await MenuHelper.CheckResponse(response);
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
    
    private async Task UpdateService()
    {
        var services = await client.GetFromJsonAsync<List<GetServiceDto>>("api/service");

        if (services == null || !services.Any())
        {
            Console.WriteLine("Послуг поки немає");
            MenuHelper.PressAnyKey();
            return;
        }

        await ViewServices();

        var index = MenuHelper.ReadChoiceNumber("Виберіть сервіс для оновлення: ", 1, services.Count);
        var selected = services[index - 1];

        Console.Write($"Нова назва (натисніть Enter щоб залишити \"{selected.Title}\"): ");
        var newTitle = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newTitle)) newTitle = selected.Title;

        Console.Write($"Новий опис (натисніть Enter щоб залишити): ");
        var newDesc = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newDesc)) newDesc = selected.Description;

        Console.Write($"Нова ціна (натисніть Enter щоб залишити {selected.Price:F2}): ");
        var priceInput = Console.ReadLine();
        decimal newPrice;
        if (string.IsNullOrWhiteSpace(priceInput))
        {
            newPrice = selected.Price;
        }
        else if (!decimal.TryParse(priceInput, out newPrice))
        {
            Console.WriteLine("Невірний формат числа. Операцію скасовано.");
            MenuHelper.PressAnyKey();
            return;
        }

        var updateRequest = new UpdateServiceDto
        {
            Title = newTitle,
            Description = newDesc,
            Price = newPrice
        };

        try
        {
            var response = await client.PutAsJsonAsync($"/api/service/{selected.Id}", updateRequest);
            await MenuHelper.CheckResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        MenuHelper.PressAnyKey();
    }
    
    private async Task DeleteOrder()
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

        var index = MenuHelper.ReadChoiceNumber("Виберіть замовлення для видалення: ", 1, orders.Count);
        var selected = orders[index - 1];
        
        try
        {
            var response = await client.DeleteAsync($"/api/order/{selected.Id}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Замовлення успішно видалено!");
            }
            else
            {
                var errorContent = await response.Content.ReadFromJsonAsync<ErrorDto>();
                Console.WriteLine(errorContent!.Error);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        MenuHelper.PressAnyKey();
    }

    private async Task DeleteService()
    {
        var services = await client.GetFromJsonAsync<List<GetServiceDto>>("/api/service");

        if (services == null || !services.Any())
        {
            Console.WriteLine("Послуг поки немає");
            MenuHelper.PressAnyKey();
            return;
        }

        await ViewServices();

        var index = MenuHelper.ReadChoiceNumber("Виберіть послугу для видалення: ", 1, services.Count);
        var selected = services[index - 1];
        
        try
        {
            var response = await client.DeleteAsync($"/api/service/{selected.Id}");
            await MenuHelper.CheckResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
        
        MenuHelper.PressAnyKey();
    }

    private async Task AddServiceToPackage()
    {
        var packages = await client.GetFromJsonAsync<List<GetPackageDto>>("/api/package");
        var services = await client.GetFromJsonAsync<List<GetServiceDto>>("/api/service");

        if (packages == null || !packages.Any())
        {
            Console.WriteLine("Пакетів поки немає");
            MenuHelper.PressAnyKey();
            return;
        }

        if (services == null || !services.Any())
        {
            Console.WriteLine("Послуг поки немає");
            MenuHelper.PressAnyKey();
            return;
        }

        Console.WriteLine("Виберіть пакет:");
        for (int i = 0; i < packages.Count; i++)
        {
            Console.WriteLine($"{i+1}. {packages[i].Title} ({packages[i].TotalPrice:F2})");
        }
        var pIndex = MenuHelper.ReadChoiceNumber("Ваш вибір: ", 1, packages.Count);
        var selectedPackage = packages[pIndex - 1];

        Console.WriteLine("Виберіть послугу для додавання в пакет:");
        for (int i = 0; i < services.Count; i++)
        {
            Console.WriteLine($"{i+1}. {services[i].Title} ({services[i].Price:F2})");
        }
        var sIndex = MenuHelper.ReadChoiceNumber("Ваш вибір: ", 1, services.Count);
        var selectedService = services[sIndex - 1];

        try
        {
            var response = await client.PutAsync($"/api/package/{selectedPackage.Id}/services/{selectedService.Id}", null);
            await MenuHelper.CheckResponse(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        MenuHelper.PressAnyKey();
    }

    private async Task DeletePackage()
    {
        var packages = await client.GetFromJsonAsync<List<GetPackageDto>>("/api/package");

        if (packages == null || !packages.Any())
        {
            Console.WriteLine("Пакетів поки немає");
            MenuHelper.PressAnyKey();
            return;
        }

        Console.WriteLine("Виберіть пакет для видалення:");
        for (int i = 0; i < packages.Count; i++)
        {
            Console.WriteLine($"{i+1}. {packages[i].Title} ({packages[i].TotalPrice:F2})");
        }
        var index = MenuHelper.ReadChoiceNumber("Ваш вибір: ", 1, packages.Count);
        var selected = packages[index - 1];
        
        try
        {
            var response = await client.DeleteAsync($"/api/package/{selected.Id}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Пакет успішно видалено!");
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Помилка при видаленні: {response.StatusCode} - {content}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        MenuHelper.PressAnyKey();
    }
}