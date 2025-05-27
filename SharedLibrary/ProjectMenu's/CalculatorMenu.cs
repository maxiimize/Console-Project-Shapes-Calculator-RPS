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
                AnsiConsole.Write(new FigletText("CALCULATOR").Centered().Color(Color.Red));
                AnsiConsole.Write(new Rule());

                int choice = PromptChoice();
                Console.Clear();

                switch (choice)
                {
                    case 1: Create(); break;
                    case 2: ListAll(); break;
                    case 3: Update(); break;
                    case 4: Delete(); break;
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

        void Create()
        {
            double t1 = PromptDouble("Tal 1");
            string op = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Välj operator:")
                    .AddChoices("+", "-", "*", "/", "√", "%"));

            double? t2 = op != "√" ? PromptDouble("Tal 2") : (double?)null;
            double result = op switch
            {
                "+" => Math.Round(t1 + t2!.Value, 2),
                "-" => Math.Round(t1 - t2!.Value, 2),
                "*" => Math.Round(t1 * t2!.Value, 2),
                "/" => t2 == 0 ? throw new DivideByZeroException() : Math.Round(t1 / t2!.Value, 2),
                "√" => Math.Round(Math.Sqrt(t1), 2),
                "%" => t2 == 0 ? throw new DivideByZeroException() : Math.Round(t1 % t2!.Value, 2),
                _ => throw new InvalidOperationException()
            };

            var rec = new Calculator
            {
                Operand1 = t1,
                Operand2 = t2,
                Operator = op,
                Result = result,
                DateCreated = DateTime.Now
            };
            _context.Calculations.Add(rec);
            _context.SaveChanges();

            AnsiConsole.MarkupLine($"[green]Resultat: {result:0.##}[/]");
            AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
            Console.ReadLine();
        }

        void ListAll()
        {
            var all = _context.Calculations.OrderBy(c => c.Id).ToList();
            var table = new Table().Border(TableBorder.Rounded)
                .AddColumn("Id").AddColumn("Tal1").AddColumn("Tal2")
                .AddColumn("Op").AddColumn("Result").AddColumn("Datum");

            foreach (var c in all)
            {
                table.AddRow(
                    c.Id.ToString(),
                    c.Operand1.ToString("0.##"),
                    c.Operand2?.ToString("0.##") ?? "-",
                    c.Operator,
                    c.Result.ToString("0.##"),
                    c.DateCreated.ToString("yyyy-MM-dd HH:mm"));
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
            Console.ReadLine();
        }

        void Update()
        {
            ListAll();
            int id = AnsiConsole.Prompt(
                new TextPrompt<int>("Ange Id att uppdatera (0 för avbryt):")
                    .Validate(i => i >= 0 ? ValidationResult.Success() : ValidationResult.Error("[red]Ogiltigt[/]")));
            if (id == 0) return;

            var rec = _context.Calculations.Find(id);
            if (rec == null) return;

            rec.Operand1 = PromptDouble("Nytt Tal 1");
            rec.Operator = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("+", "-", "*", "/", "√", "%"));
            rec.Operand2 = rec.Operator != "√" ? PromptDouble("Nytt Tal 2") : null;
            rec.Result = opResult(rec);
            rec.DateCreated = DateTime.Now;
            _context.SaveChanges();

            AnsiConsole.MarkupLine("[green]Uppdaterad![/]");
            AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
            Console.ReadLine();
        }

        void Delete()
        {
            ListAll();
            int id = AnsiConsole.Prompt(
                new TextPrompt<int>("Ange Id att radera (0 för avbryt):")
                    .Validate(i => i >= 0 ? ValidationResult.Success() : ValidationResult.Error("[red]Ogiltigt[/]")));
            if (id == 0) return;

            var rec = _context.Calculations.Find(id);
            if (rec != null)
            {
                _context.Calculations.Remove(rec);
                _context.SaveChanges();
                AnsiConsole.MarkupLine("[green]Raderad![/]");
            }

            AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
            Console.ReadLine();
        }

        double PromptDouble(string label) =>
            AnsiConsole.Prompt(
                new TextPrompt<double>($"{label}:")
                    .Validate(n => n >= 0 ? ValidationResult.Success() : ValidationResult.Error("[red]Måste vara ≥ 0[/]")));

        private double opResult(Calculator rec) =>
            rec.Operator switch
            {
                "+" => Math.Round(rec.Operand1 + rec.Operand2!.Value, 2),
                "-" => Math.Round(rec.Operand1 - rec.Operand2!.Value, 2),
                "*" => Math.Round(rec.Operand1 * rec.Operand2!.Value, 2),
                "/" => rec.Operand2 == 0 ? throw new DivideByZeroException() : Math.Round(rec.Operand1 / rec.Operand2!.Value, 2),
                "√" => Math.Round(Math.Sqrt(rec.Operand1), 2),
                "%" => rec.Operand2 == 0 ? throw new DivideByZeroException() : Math.Round(rec.Operand1 % rec.Operand2!.Value, 2),
                _ => rec.Result
            };
    }
}
