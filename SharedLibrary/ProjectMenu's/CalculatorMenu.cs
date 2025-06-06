using System;
using System.Globalization;
using System.Linq;
using DataAcessLayer;
using DataAcessLayer.ModelsCalculator;
using Spectre.Console;
using SharedLibrary.ViewModels;
using SharedLibrary.Mappings;

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
                ShowHeader();
                int choice = PromptChoice();
                Console.Clear();

                switch (choice)
                {
                    case 1: StartCalculations(); break;
                    case 2: ListCalculations(); break;
                    case 3: UpdateCalculation(); break;
                    case 4: DeleteCalculation(); break;
                    case 5: back = true; break;
                }
            }
        }

        private void ShowHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText("CALCULATOR")
                    .Centered()
                    .Color(Color.Red));
            AnsiConsole.Write(new Rule());
        }

        private int PromptChoice()
        {
            var options = new[]
            {
                "1. Start calculation",
                "2. List all calculations",
                "3. Update a calculation",
                "4. Delete a calculation",
                "5. Back to Main Menu"
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

            var trimmed = selection.TrimStart();
            return int.Parse(trimmed.Split('.')[0]);
        }

        private void RenderTable()
        {
            var all = _context.Calculations
                .OrderByDescending(c => c.DateCreated)
                .Select(c => c.ToViewModel())
                .ToList();

            var table = new Table().Border(TableBorder.Rounded)
                .AddColumn("Id")
                .AddColumn("Tal1")
                .AddColumn("Tal2")
                .AddColumn("Op")
                .AddColumn("Result")
                .AddColumn("Datum");

            foreach (var c in all)
            {
                table.AddRow(
                    c.Id.ToString(),
                    c.Operand1.ToString("0.##"),
                    c.Operand2?.ToString("0.##") ?? "-",
                    c.Operator,
                    c.Result.ToString("0.##"),
                    c.DateCreated
                );
            }

            AnsiConsole.Write(table);
        }

        private void StartCalculations()
        {
            while (true)
            {
                bool didCalculation = CreateCalculation();
                if (!didCalculation)
                    break;

                var next = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Vad vill du göra nu?")
                        .AddChoices(new[] {
                            "Ny kalkylation",
                            "Tillbaka till menyn"
                        })
                );

                if (next != "Ny kalkylation")
                    break;

                Console.Clear();
            }
        }

        private bool CreateCalculation()
        {
            string op = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Välj operator eller 'Tillbaka till menyn':")
                    .AddChoices("+", "-", "*", "/", "√", "%", "Tillbaka till menyn")
            );

            if (op == "Tillbaka till menyn")
                return false;

            double t1 = PromptDouble("Tal 1", nonNegative: op == "√");
            double? t2 = null;

            if (op != "√")
            {
                if (op == "/" || op == "%")
                {
                    t2 = PromptDoubleNotZero("Tal 2");
                }
                else
                {
                    t2 = PromptDouble("Tal 2");
                }
            }

            double result = op switch
            {
                "+" => Math.Round(t1 + t2!.Value, 2),
                "-" => Math.Round(t1 - t2!.Value, 2),
                "*" => Math.Round(t1 * t2!.Value, 2),
                "/" => Math.Round(t1 / t2!.Value, 2),
                "√" => Math.Round(Math.Sqrt(t1), 2),
                "%" => Math.Round(t1 % t2!.Value, 2),
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

            var vm = rec.ToViewModel();

            var table = new Table().Border(TableBorder.Rounded)
                .AddColumn("Id")
                .AddColumn("Tal1")
                .AddColumn("Tal2")
                .AddColumn("Op")
                .AddColumn("Result")
                .AddColumn("Datum");

            table.AddRow(
                vm.Id.ToString(),
                vm.Operand1.ToString("0.##"),
                vm.Operand2?.ToString("0.##") ?? "-",
                vm.Operator,
                vm.Result.ToString("0.##"),
                vm.DateCreated
            );

            AnsiConsole.MarkupLine("[green]Beräkningen är sparad:[/]");
            AnsiConsole.Write(table);

            AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
            Console.ReadLine();
            return true;
        }

        void ListCalculations()
        {
            RenderTable();
            AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
            Console.ReadLine();
        }

        void UpdateCalculation()
        {
            RenderTable();

            var validIds = _context.Calculations
                                   .Select(c => c.Id)
                                   .ToList();

            int id = AnsiConsole.Prompt(
                new TextPrompt<int>("[yellow]Ange [green]Id[/] på den kalkylation som du vill uppdatera (eller [red]0[/] för att avbryta):[/]")
                    .ValidationErrorMessage("[red]Fel: Du måste ange ett giltigt Id-nummer (heltal). Försök igen![/]")
                    .Validate(i =>
                    {
                        if (i == 0) return ValidationResult.Success();
                        if (!validIds.Contains(i))
                            return ValidationResult.Error($"[red]Fel: Id {i} finns inte i listan. Välj ett Id från tabellen ovan.[/]");
                        return ValidationResult.Success();
                    })
            );
            if (id == 0)
                return;

            var rec = _context.Calculations.Find(id)!;

            string op = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Välj ny operator (eller 'Tillbaka till menyn'):")
                    .AddChoices("+", "-", "*", "/", "√", "%", "Tillbaka till menyn")
            );

            if (op == "Tillbaka till menyn")
                return;

            rec.Operand1 = PromptDouble("Nytt Tal 1", nonNegative: op == "√");

            if (op != "√")
            {
                if (op == "/" || op == "%")
                {
                    rec.Operand2 = PromptDoubleNotZero("Nytt Tal 2");
                }
                else
                {
                    rec.Operand2 = PromptDouble("Nytt Tal 2");
                }
            }
            else
            {
                rec.Operand2 = null;
            }

            rec.Operator = op;
            rec.Result = opResult(rec);
            rec.DateCreated = DateTime.Now;
            _context.SaveChanges();

            var vm = rec.ToViewModel();

            var table = new Table().Border(TableBorder.Rounded)
                .AddColumn("Id")
                .AddColumn("Tal1")
                .AddColumn("Tal2")
                .AddColumn("Op")
                .AddColumn("Result")
                .AddColumn("Datum");

            table.AddRow(
                vm.Id.ToString(),
                vm.Operand1.ToString("0.##"),
                vm.Operand2?.ToString("0.##") ?? "-",
                vm.Operator,
                vm.Result.ToString("0.##"),
                vm.DateCreated
            );

            AnsiConsole.MarkupLine("[green]Uppdaterad![/]");
            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
            Console.ReadLine();
        }

        void DeleteCalculation()
        {
            RenderTable();

            var validIds = _context.Calculations
                                   .Select(c => c.Id)
                                   .ToList();

            int id = AnsiConsole.Prompt(
                new TextPrompt<int>("[yellow]Ange [green]Id[/] på den kalkylation som du vill radera (eller [red]0[/] för att avbryta):[/]")
                    .ValidationErrorMessage("[red]Fel: Du måste ange ett giltigt Id-nummer (heltal). Försök igen![/]")
                    .Validate(i =>
                    {
                        if (i == 0) return ValidationResult.Success();
                        if (!validIds.Contains(i))
                            return ValidationResult.Error($"[red]Fel: Id {i} finns inte i listan. Välj ett Id från tabellen ovan.[/]");
                        return ValidationResult.Success();
                    })
            );
            if (id == 0)
                return;

            var rec = _context.Calculations.Find(id)!;

            var confirm = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Är du säker på att du vill radera denna kalkylation?")
                    .AddChoices(new[] { "Ja", "Nej (Tillbaka till menyn)" })
            );

            if (confirm == "Nej (Tillbaka till menyn)")
                return;

            rec.IsDeleted = true;
            _context.SaveChanges();

            AnsiConsole.MarkupLine("[green]Raderad![/]");
            AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
            Console.ReadLine();
        }

        double PromptDouble(string label, bool nonNegative = false)
        {
            string raw = AnsiConsole.Prompt(
                new TextPrompt<string>($"{label}:")
                    .ValidationErrorMessage("[red]Fel: Du måste ange ett giltigt tal (t.ex. 42, 3.14 eller 3,14).[/]")
                    .Validate(input =>
                    {
                        var normalized = input.Replace(',', '.');
                        if (!double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                        {
                            return ValidationResult.Error("[red]Fel: Ogiltigt tal. Försök igen![/]");
                        }

                        if (double.IsInfinity(value))
                            return ValidationResult.Error("[red]Fel: Talet är för stort. Ange ett mindre tal![/]");
                        if (double.IsNaN(value))
                            return ValidationResult.Error("[red]Fel: Ogiltigt tal. Försök igen![/]");

                        if (nonNegative && value < 0)
                            return ValidationResult.Error("[red]Fel: Talet måste vara positivt eller noll (≥ 0) för kvadratrot![/]");

                        return ValidationResult.Success();
                    })
            );

            raw = raw.Replace(',', '.');
            double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out double result);
            return result;
        }

        double PromptDoubleNotZero(string label)
        {
            string raw = AnsiConsole.Prompt(
                new TextPrompt<string>($"{label}:")
                    .ValidationErrorMessage("[red]Fel: Du måste ange ett giltigt tal (t.ex. 42, 3.14 eller 3,14).[/]")
                    .Validate(input =>
                    {
                        var normalized = input.Replace(',', '.');
                        if (!double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                        {
                            return ValidationResult.Error("[red]Fel: Ogiltigt tal. Försök igen![/]");
                        }

                        if (value == 0)
                            return ValidationResult.Error("[red]Fel: Division med noll är inte tillåtet! Ange ett tal som inte är noll.[/]");

                        if (double.IsInfinity(value))
                            return ValidationResult.Error("[red]Fel: Talet är för stort. Ange ett mindre tal![/]");
                        if (double.IsNaN(value))
                            return ValidationResult.Error("[red]Fel: Ogiltigt tal. Försök igen![/]");

                        return ValidationResult.Success();
                    })
            );

            raw = raw.Replace(',', '.');
            double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out double result);
            return result;
        }

        private double opResult(Calculator rec) =>
            rec.Operator switch
            {
                "+" => Math.Round(rec.Operand1 + rec.Operand2!.Value, 2),
                "-" => Math.Round(rec.Operand1 - rec.Operand2!.Value, 2),
                "*" => Math.Round(rec.Operand1 * rec.Operand2!.Value, 2),
                "/" => Math.Round(rec.Operand1 / rec.Operand2!.Value, 2),
                "√" => Math.Round(Math.Sqrt(rec.Operand1), 2),
                "%" => Math.Round(rec.Operand1 % rec.Operand2!.Value, 2),
                _ => rec.Result
            };
    }
}
