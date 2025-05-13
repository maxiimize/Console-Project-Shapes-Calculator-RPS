using SharedLibrary;

namespace ShapesApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var menu = new MainMenu();
            menu.Show();
            Console.ReadKey();
        }
    }
}
