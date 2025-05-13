using SharedLibrary;

namespace RPSApp
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
