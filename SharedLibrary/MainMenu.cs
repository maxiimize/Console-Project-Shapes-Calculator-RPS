using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class MainMenu
    {
        private readonly List<string> _menuOptions = new()
        {
            "Shapes",
            "Calculator",
            "Rock-Paper-Scissors",
            "Exit"
        };

        public void Show()
        {
            Console.Clear();
            Console.WriteLine("=== Main Menu ===");
            for (int i = 0; i < _menuOptions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_menuOptions[i]}");
            }
        }
    }
}
