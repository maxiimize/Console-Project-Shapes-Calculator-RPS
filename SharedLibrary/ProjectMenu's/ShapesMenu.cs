using System;
using System.Globalization;
using System.Linq;
using DataAcessLayer;
using DataAcessLayer.ModelsShapes;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using SharedLibrary.ViewModels;
using SharedLibrary.Mappings;

namespace SharedLibrary
{
    public class ShapesMenu
    {
        private readonly AllDbContext _context;

        public ShapesMenu(AllDbContext context)
        {
            _context = context;
        }

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
                    case 1: CreateShape(); break;
                    case 2: ListShapes(); break;
                    case 3: UpdateShape(); break;
                    case 4: DeleteShape(); break;
                    case 5: back = true; break;
                }
            }
        }

        private void ShowHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText("FORMER")
                    .Centered()
                    .Color(Color.Aqua));
            AnsiConsole.Write(new Rule());
        }

        private int PromptChoice()
        {
            var options = new[]
            {
                "1. Skapa ny form",
                "2. Visa alla former",
                "3. Uppdatera en form",
                "4. Radera en form",
                "5. Tillbaka till huvudmenyn"
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

        private void CreateShape()
        {
            var types = new[] { "Rektangel", "Parallelogram", "Triangel", "Romb", "Avbryt" };
            var type = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Välj en form (Eller [red]Avbryt[/]):")
                    .AddChoices(types));

            if (type == "Avbryt")
                return;

            Shape shape = type switch
            {
                "Rektangel" => new Rectangle
                {
                    Width = PromptDouble("Bredd", "rektangelns bredd"),
                    Height = PromptDouble("Höjd", "rektangelns höjd")
                },
                "Parallelogram" => new Parallelogram
                {
                    BaseLength = PromptDouble("Baslängd", "parallellogrammets baslängd"),
                    SideLength = PromptDouble("Sidlängd", "parallellogrammets sidlängd"),
                    Height = PromptDouble("Höjd", "parallellogrammets höjd")
                },
                "Triangel" => new Triangle
                {
                    BaseLength = PromptDouble("Baslängd", "triangelns baslängd"),
                    Height = PromptDouble("Höjd", "triangelns höjd"),
                    SideA = PromptDouble("Sida A", "triangelns sida A"),
                    SideB = PromptDouble("Sida B", "triangelns sida B")
                },
                "Romb" => new Rhombus
                {
                    SideLength = PromptDouble("Sidlängd", "rombens sidlängd"),
                    Height = PromptDouble("Höjd", "rombens höjd")
                },
                _ => throw new InvalidOperationException()
            };

            shape.Calculate();
            shape.DateCreated = DateTime.Now;
            _context.Add(shape);
            _context.SaveChanges();

            AnsiConsole.MarkupLine("[green]Form sparad![/]");

            var vm = shape.ToViewModel();

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("Id");
            table.AddColumn("Typ");
            table.AddColumn("Parametrar");
            table.AddColumn("Area");
            table.AddColumn("Omkrets");
            table.AddColumn("Datum");

            table.AddRow(
                vm.Id.ToString(),
                vm.ShapeType,
                vm.Parameters,
                vm.Area.ToString("F2"),
                vm.Perimeter.ToString("F2"),
                vm.DateCreated);

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("[grey]Tryck på enter för att fortsätta...[/]");
            Console.ReadLine();
        }

        private void ListShapes()
        {
            var all = _context.Shapes
                .OrderByDescending(c => c.DateCreated)
                .Select(s => s.ToViewModel())
                .ToList();

            if (!all.Any())
            {
                AnsiConsole.MarkupLine("[yellow]Inga former har skapats än. Skapa en shape först![/]");
                AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
                Console.ReadLine();
                return;
            }

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("Id");
            table.AddColumn("Typ");
            table.AddColumn("Parametrar");
            table.AddColumn("Area");
            table.AddColumn("Omkrets");
            table.AddColumn("Datum");

            foreach (var s in all)
            {
                table.AddRow(
                    s.Id.ToString(),
                    s.ShapeType,
                    s.Parameters,
                    s.Area.ToString("F2"),
                    s.Perimeter.ToString("F2"),
                    s.DateCreated);
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
            Console.ReadLine();
        }

        private void UpdateShape()
        {
            var all = _context.Shapes
                .OrderByDescending(s => s.DateCreated)
                .ToList();

            if (!all.Any())
            {
                AnsiConsole.MarkupLine("[red]Inga former att uppdatera![/]");
                AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
                Console.ReadLine();
                return;
            }

            var allVm = all.Select(s => s.ToViewModel()).ToList();

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("Id");
            table.AddColumn("Typ");
            table.AddColumn("Parametrar");
            table.AddColumn("Area");
            table.AddColumn("Omkrets");
            table.AddColumn("Datum");

            foreach (var s in allVm)
            {
                table.AddRow(
                    s.Id.ToString(),
                    s.ShapeType,
                    s.Parameters,
                    s.Area.ToString("F2"),
                    s.Perimeter.ToString("F2"),
                    s.DateCreated
                );
            }

            AnsiConsole.Write(table);

            int id = AnsiConsole.Prompt(
                new TextPrompt<int>("[yellow]Ange [green]Id[/] på den form du vill uppdatera (eller [red]0[/] för att avbryta):[/]")
                    .ValidationErrorMessage("[red]Fel: Du måste ange ett giltigt Id-nummer (heltal). Text som 'bajskorv' fungerar inte![/]")
                    .Validate(i =>
                    {
                        if (i == 0) return ValidationResult.Success();
                        if (!all.Any(s => s.Id == i))
                            return ValidationResult.Error($"[red]Fel: Id {i} finns inte i listan. Välj ett Id från tabellen ovan.[/]");
                        return ValidationResult.Success();
                    })
            );

            if (id == 0)
                return;

            var shape = _context.Shapes.Find(id)!;

            switch (shape)
            {
                case Rectangle r:
                    r.Width = PromptDouble("Ny bredd", "rektangelns nya bredd");
                    r.Height = PromptDouble("Ny höjd", "rektangelns nya höjd");
                    break;
                case Parallelogram p:
                    p.BaseLength = PromptDouble("Ny baslängd", "parallellogrammets nya baslängd");
                    p.SideLength = PromptDouble("Ny sidlängd", "parallellogrammets nya sidlängd");
                    p.Height = PromptDouble("Ny höjd", "parallellogrammets nya höjd");
                    break;
                case Triangle t:
                    t.BaseLength = PromptDouble("Ny baslängd", "triangelns nya baslängd");
                    t.Height = PromptDouble("Ny höjd", "triangelns nya höjd");
                    t.SideA = PromptDouble("Ny sida A", "triangelns nya sida A");
                    t.SideB = PromptDouble("Ny Sida B", "triangelns nya sida B");
                    break;
                case Rhombus h:
                    h.SideLength = PromptDouble("Ny sidlängd", "rombens nya sidlängd");
                    h.Height = PromptDouble("New höjd", "rombens nya höjd");
                    break;
            }

            shape.Calculate();
            _context.SaveChanges();

            AnsiConsole.MarkupLine("[green]Form uppdaterad![/]");
            AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
            Console.ReadLine();
        }

        private void DeleteShape()
        {
            var all = _context.Shapes
                .Where(s => !s.IsDeleted)
                .OrderByDescending(s => s.DateCreated)
                .ToList();

            if (!all.Any())
            {
                AnsiConsole.MarkupLine("[red]Inga former att radera![/]");
                AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
                Console.ReadLine();
                return;
            }

            var allVm = all.Select(s => s.ToViewModel()).ToList();

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("Id");
            table.AddColumn("Typ");
            table.AddColumn("Parametrar");
            table.AddColumn("Area");
            table.AddColumn("Omkrets");
            table.AddColumn("Datum");

            foreach (var s in allVm)
            {
                table.AddRow(
                    s.Id.ToString(),
                    s.ShapeType,
                    s.Parameters,
                    s.Area.ToString("F2"),
                    s.Perimeter.ToString("F2"),
                    s.DateCreated
                );
            }

            AnsiConsole.Write(table);

            int id = AnsiConsole.Prompt(
                new TextPrompt<int>("[yellow]Ange [green]Id[/] på form som du vill radera (eller [red]0[/] för att avbryta):[/]")
                    .ValidationErrorMessage("[red]Fel: Du måste ange ett giltigt Id-nummer (heltal). Text som 'bajskorv' fungerar inte![/]")
                    .Validate(i =>
                    {
                        if (i == 0) return ValidationResult.Success();
                        if (!all.Any(s => s.Id == i))
                            return ValidationResult.Error($"[red]Fel: Id {i} finns inte i listan. Välj ett Id från tabellen ovan.[/]");
                        return ValidationResult.Success();
                    })
            );

            if (id == 0)
                return;

            var shape = _context.Shapes.Find(id)!;

            var confirm = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Är du säker på att du vill radera denna form?")
                    .AddChoices(new[] { "Ja", "Nej (Tillbaka till menyn)" })
            );

            if (confirm != "Ja")
                return;

            shape.IsDeleted = true;
            _context.SaveChanges();

            AnsiConsole.MarkupLine("[green]Form raderad![/]");
            AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
            Console.ReadLine();
        }


        private double PromptDouble(string name, string swedishDescription = null)
        {
            string description = swedishDescription ?? name.ToLower();

            string raw = AnsiConsole.Prompt(
                new TextPrompt<string>($"Ange {name}:")
                    .ValidationErrorMessage($"[red]Fel: Du måste ange ett giltigt tal för {description}. Text som 'bajskorv' fungerar inte![/]")
                    .Validate(input =>
                    {
                        var normalized = input.Replace(',', '.');
                        if (!double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                        {
                            return ValidationResult.Error($"[red]Fel: Ogiltigt tal för {description}. Försök igen![/]");
                        }

                        if (value <= 0)
                            return ValidationResult.Error($"[red]Fel: {char.ToUpper(description[0]) + description.Substring(1)} måste vara större än 0![/]");

                        if (double.IsInfinity(value))
                            return ValidationResult.Error($"[red]Fel: Talet för {description} är för stort. Ange ett mindre tal![/]");

                        if (double.IsNaN(value))
                            return ValidationResult.Error($"[red]Fel: Ogiltigt tal för {description}. Försök igen![/]");

                        if (value > 1_000_000)
                            return ValidationResult.Error($"[red]Fel: {char.ToUpper(description[0]) + description.Substring(1)} verkar orimligt stor (>1000000). Kontrollera värdet![/]");

                        return ValidationResult.Success();
                    })
            );

            raw = raw.Replace(',', '.');
            double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out double result);
            return result;
        }
    }
}
