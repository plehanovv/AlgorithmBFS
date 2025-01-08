using coursework;

namespace coursework
{
    class Program
    {
        static void Main(string[] args)
        {
            var menu = new Menu();
            menu.ShowMenu();
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}