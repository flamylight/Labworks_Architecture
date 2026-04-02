namespace Lab1.Menus;

public class MainMenu
{
    private MenuHelper _helper = new MenuHelper();
    private StudentMenu _studentMenu;
    private GroupMenu _groupMenu;
    private DisciplineMenu _disciplineMenu;
    private TeacherMenu _teacherMenu;

    public MainMenu(StudentMenu studentMenu, GroupMenu groupMenu, DisciplineMenu disciplineMenu, TeacherMenu teacherMenu)
    {
        _studentMenu = studentMenu;
        _groupMenu = groupMenu;
        _disciplineMenu = disciplineMenu;
        _teacherMenu = teacherMenu;
    }

    public void RunMenu()
    { 
        bool isActive = true;

        while (isActive)
        {
            Console.Clear();
            Console.WriteLine("1. Студенти\n" +
                              "2. Групи\n" +
                              "3. Дисципліни\n" +
                              "4. Викладачі\n" +
                              "0. Вийти");

            int parseChoice = _helper.ReadInt("Твій вибір: ");

            switch (parseChoice)
            {
                case 1:
                    _studentMenu.RunMenu();
                    break;
                case 2:
                    _groupMenu.RunMenu();
                    break;
                case 3:
                    _disciplineMenu.RunMenu();
                    break;
                case 4:
                    _teacherMenu.RunMenu();
                    break;
                case 0:
                    isActive = false;
                    break;
                default:
                    Console.WriteLine("Невідома команда");
                    _helper.PressAnyKey();
                    break;
            }
        }
    }
}