using System;
using System.Linq;
using DataAcessLayer;
using DataAcessLayer.ModelsCalculator;
using Spectre.Console;

namespace SharedLibrary
{
    public class CalculatorMenu
    {
        private readonly AllDbContext _context;
        public CalculatorMenu(AllDbContext context) => _context = context;

        public void Run()
        {
            bool back = false;
            while (!back)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new FigletText("CALCULATOR").Centered().Color(Color.Green));
                AnsiConsole.Write(new Rule());

                int choice = PromptChoice();
                Console.Clear();

                switch (choice)
                {
                    case 1: //Create(); break;
                    case 2: //ListAll(); break;
                    case 3: //Update(); break;
                    case 4: //Delete(); break;
                    case 5: back = true; break;
                }
            }
        }

        private int PromptChoice()
        {
            var options = new[]
            {
                "1. Ny beräkning",
                "2. Lista alla",
                "3. Uppdatera",
                "4. Radera",
                "5. Tillbaka"
            };

            int maxLen = options.Max(o => o.Length);
            int consoleWidth = Console.WindowWidth;
            int indent = Math.Max((consoleWidth - maxLen) / 2, 0);
            var padding = new string(' ', indent);
            var padded = options.Select(o => padding + o).ToArray();

            AnsiConsole.Write(
                new Markup("[yellow]Välj ett alternativ:[/]")
                    .Centered());
            AnsiConsole.Write(new Rule());

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(padded.Length)
                    .AddChoices(padded)
            );

            return int.Parse(selection.TrimStart().Split('.')[0]);
        }
    }
}
