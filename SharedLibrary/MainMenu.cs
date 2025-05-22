using Spectre.Console;
using System;
using System.Linq;

namespace SharedLibrary
{
    public class MainMenu
    {
        private readonly ShapesMenu _shapesMenu;
        private readonly string[] _menuOptions = new[]
        {
            "Shapes",
            "Calculator (Coming soon!)",
            "Rock-Paper-Scissors (Coming soon!)",
            "Exit"
        };

        public int OptionCount => _menuOptions.Length;

        public MainMenu(ShapesMenu shapesMenu)
        {
            _shapesMenu = shapesMenu;
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
                        // Kör fullständig Shapes-modul
                        _shapesMenu.Run();
                        break;

                    case 2:
                        // Placeholder för Calculator
                        AnsiConsole.MarkupLine("[yellow]Calculator är på gång – kommer snart![/]");
                        break;

                    case 3:
                        // Placeholder för RPS
                        AnsiConsole.MarkupLine("[yellow]Rock-Paper-Scissors är på gång – kommer snart![/]");
                        break;

                    case 4:
                        AnsiConsole.MarkupLine("[grey]Avslutar…[/]");
                        exit = true;
                        break;

                    default:
                        AnsiConsole.MarkupLine("[red]Ogiltigt val, försök igen.[/]");
                        break;
                }

                if (!exit)
                {
                    AnsiConsole.MarkupLine("\nTryck på valfri tangent för att återvända till huvudmenyn…");
                    Console.ReadKey(true);
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
            int width = Console.WindowWidth;
            var paddedOptions = _menuOptions
                .Select((opt, i) =>
                {
                    var text = $"{i + 1}. {opt}";
                    int pad = Math.Max((width - text.Length) / 2, 0);
                    return text.PadLeft(text.Length + pad);
                })
                .ToArray();

            AnsiConsole.Write(
                new Markup("[yellow]Välj ett alternativ:[/]")
                    .Centered());
            AnsiConsole.Write(new Rule());

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(OptionCount)
                    .AddChoices(paddedOptions)
            );

            var trimmed = selection.Trim();
            var indexStr = trimmed.Split('.')[0];
            return int.Parse(indexStr);
        }
    }
}
