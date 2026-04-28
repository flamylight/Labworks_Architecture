using BLL.Interfaces;
using BLL.Managers;
using DAL.Data;
using PL;
using PL.Menus;

namespace Lab3;

class Program
{
    static void Main()
    {
        var appDbContext = new AppDbContext();
        var unitOfWork = new UnitOfWork(appDbContext);
        IServiceManager serviceManager = new ServiceManager(unitOfWork);
        IOrderManager orderManager = new OrderManager(unitOfWork);
        IPackageManager packageManager = new PackageManager(unitOfWork);
        AdminMenu adminMenu = new AdminMenu(serviceManager, orderManager, packageManager);
        CustomerMenu customerMenu = new CustomerMenu(serviceManager, orderManager, packageManager);

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
                    adminMenu.Run();
                    break;
                case 2:
                    customerMenu.Run();
                    break;
                case 0:
                    return;
            }
        }
    }
}