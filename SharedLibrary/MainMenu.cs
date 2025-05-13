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

        public int Prompt()
        {
            while (true)
            {
                Console.Write("\nVälj ett alternativ (1–4): ");
                var input = Console.ReadLine()?.Trim();

                if (int.TryParse(input, out int choice) &&
                    choice >= 1 && choice <= _menuOptions.Count)
                {
                    return choice;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt val! Försök igen.");
                Console.ResetColor();
            }
        }

        public void Run()
        {
            bool exitRequested = false;
            while (!exitRequested)
            {
                Show();
                int choice = Prompt();

                switch (choice)
                {
                    case 1:
                        NavigateToShapes();
                        break;
                    case 2:
                        NavigateToCalculator();
                        break;
                    case 3:
                        NavigateToRps();
                        break;
                    case 4:
                        exitRequested = true;
                        break;
                }
            }
        }

        private void NavigateToShapes()
        {
            Console.Clear();
            Console.WriteLine(">>> Shapes-modulen kommer snart! (Coming soon!)");
            Console.WriteLine("Tryck på valfri tangent för att återvända till huvudmenyn.");
            Console.ReadKey();
        }

        private void NavigateToCalculator()
        {
            Console.Clear();
            Console.WriteLine(">>> Calculator-modulen kommer snart! (Coming soon!)");
            Console.WriteLine("Tryck på valfri tangent för att återvända till huvudmenyn.");
            Console.ReadKey();
        }

        private void NavigateToRps()
        {
            Console.Clear();
            Console.WriteLine(">>> Sten–Sax–Påse-modulen kommer snart! (Coming soon!)");
            Console.WriteLine("Tryck på valfri tangent för att återvända till huvudmenyn.");
            Console.ReadKey();
        }
    }
}
