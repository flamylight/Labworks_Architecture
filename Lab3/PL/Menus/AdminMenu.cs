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
                              "0. Вийти");

            var choice = MenuHelper.ReadChoiceNumber("Ваш вибір: ", 0, 1);

            switch (choice)
            {
                case 1:
                    CreateNewService();
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
}