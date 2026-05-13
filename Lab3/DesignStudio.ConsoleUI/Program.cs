using ConsoleUI;
using ConsoleUI.Menus;

class Program
{
    static async Task Main()
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7281/");

        AdminMenu adminMenu = new AdminMenu(client);
        CustomerMenu customerMenu = new CustomerMenu(client);
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine("-----РЕЖИМ ВХОДУ-----");
            Console.WriteLine("1. Адміністартор\n" +
                              "2. Клієнт\n" +
                              "0. Вийти");
        
            var choice = MenuHelper.ReadChoiceNumber("Ваш вибір: ",0, 2);

            switch (choice)
            {
                case 1:
                    await adminMenu.Run();
                    break;
                case 2:
                    await customerMenu.Run();
                    break;
                case 0:
                    return;
            }
        }
    }
}