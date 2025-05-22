using Spectre.Console;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class MainMenu
    {
        private readonly string[] _menuOptions = new[]
        {
            "Shapes (Coming soon!)",
            "Calculator (Coming soon!)",
            "Rock-Paper-Scissors (Coming soon!)",
            "Exit"
        };

        public int OptionCount => _menuOptions.Length;

        public async Task RunAsync()
        {
            bool exit = false;
            while (!exit)
            {
                ShowHeader();
                int choice = PromptChoice();
                Console.Clear();

                if (choice == OptionCount)
                {
                    AnsiConsole.MarkupLine("[grey]Avslutar…[/]");
                    exit = true;
                }
                else
                {
                    AnsiConsole.MarkupLine($"\n[bold yellow]>>> {_menuOptions[choice - 1]}[/]");
                    AnsiConsole.MarkupLine("Tryck på valfri tangent för att återvända…");
                    await WaitForKeyPressAsync();
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
            var paddedOptions = _menuOptions.Select((opt, i) =>
            {
                var text = $"{i + 1}. {opt}";
                int pad = Math.Max((width - text.Length) / 2, 0);
                return text.PadLeft(text.Length + pad);
            }).ToArray();

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

        private Task WaitForKeyPressAsync()
        {
            return Task.Run(() => Console.ReadKey(true));
        }
    }
}
