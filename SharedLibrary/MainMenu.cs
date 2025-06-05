using Spectre.Console;
using System;
using System.Linq;

namespace SharedLibrary
{
    public class MainMenu
    {
        private readonly ShapesMenu _shapesMenu;
        private readonly CalculatorMenu _calcMenu;
        private readonly RpsMenu _rpsMenu;     


        private readonly string[] _menuOptions = new[]
        {
            "Shapes",
            "Calculator",
            "Rock-Paper-Scissors",  
            "Exit"
        };

        public int OptionCount => _menuOptions.Length;

        public MainMenu(ShapesMenu shapesMenu, CalculatorMenu calcMenu, RpsMenu rpsMenu)
        {
            _shapesMenu = shapesMenu;
            _calcMenu = calcMenu;
            _rpsMenu = rpsMenu;
        }

        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                ShowHeader();
                int choice = PromptChoice();
                Console.Clear();

                switch (choice)
                {
                    case 1:
                        // Kör Shapes-modulen
                        _shapesMenu.Run();
                        break;

                    case 2:
                        // Kör Calculator-modulen
                        _calcMenu.Run();
                        break;

                    case 3:
                        // Kör Rock-Paper-Scissors-modulen
                        _rpsMenu.Run();
                        break;

                    case 4:
                        AnsiConsole.MarkupLine("[grey]Avslutar…[/]");
                        exit = true;
                        break;

                    default:
                        AnsiConsole.MarkupLine("[red]Ogiltigt val, försök igen.[/]");
                        break;
                }

                // När inte exit, rensa skärmen inför nästa loop
                if (!exit)
                {
                    Console.Clear();
                }
            }
        }

        private void ShowHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText("Main Menu")
                    .Centered()
                    .Color(Color.Green));
            AnsiConsole.Write(new Rule());
        }

        private int PromptChoice()
        {
            var options = _menuOptions
                .Select((opt, i) => $"{i + 1}. {opt}")
                .ToArray();

            int maxLen = options.Max(o => o.Length);
            int consoleWidth = Console.WindowWidth;
            int indent = Math.Max((consoleWidth - maxLen) / 2, 0);
            string padding = new string(' ', indent);

            var padded = options
                .Select(o => padding + o)
                .ToArray();

            AnsiConsole.Write(
                new Markup("[yellow]Välj ett alternativ:[/]")
                    .Centered());
            AnsiConsole.Write(new Rule());

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(padded.Length)
                    .AddChoices(padded)
            );

            var trimmed = selection.TrimStart();
            return int.Parse(trimmed.Split('.')[0]);
        }
    }
}
