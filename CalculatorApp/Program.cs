using SharedLibrary;

namespace CalculatorApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var menu = new MainMenu();
            menu.Run();
        }
    }
}
