using Lab1.Interfaces;
using Lab1.Menus;
using Lab1.Observers;
using Lab1.Services;

namespace Lab1;

class Program
{
    static void Main()
    {
        IUniversityData universityData = UniversityData.Deserialize("data.json");

        IStudentService studentService = new StudentService(universityData);
        IGroupService groupService = new GroupService(universityData);
        IDisciplineService disciplineService = new DisciplineService(universityData);
        ITeacherService teacherService = new TeacherService(universityData);
        
        var creditObserver = new CreditAwardObserver();
        creditObserver.Subscribe(disciplineService);

        UniversitySimulation simulation = new UniversitySimulation(studentService, groupService, disciplineService, teacherService);

        StudentMenu studentMenu = new StudentMenu(simulation);
        GroupMenu groupMenu = new GroupMenu(simulation);
        DisciplineMenu disciplineMenu = new DisciplineMenu(simulation, universityData);
        TeacherMenu teacherMenu = new TeacherMenu(simulation);
        MainMenu mainMenu = new MainMenu(studentMenu, groupMenu, disciplineMenu, teacherMenu);
        
        mainMenu.RunMenu();
        
        Console.WriteLine("\nСеріалізувати дані перед виходом?");
        Console.WriteLine("1. Так\n2. Ні");
        var choice = Console.ReadLine();

        if (choice == "1")
        {
            universityData.Serialize();
            Console.WriteLine("Дані збережені.");
        }
    }
}

