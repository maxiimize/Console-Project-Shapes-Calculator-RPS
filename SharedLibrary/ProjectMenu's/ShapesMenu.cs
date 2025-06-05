using System;
using System.Linq;
using DataAcessLayer;
using DataAcessLayer.ModelsShapes;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

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

                if (!back)
                {

                }
            }
        }

        private void ShowHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText("SHAPES")
                    .Centered()
                    .Color(Color.Aqua));
            AnsiConsole.Write(new Rule());
        }

        private int PromptChoice()
        {
            var options = new[]
            {
                "1. Create new shape",
                "2. List all shapes",
                "3. Update a shape",
                "4. Delete a shape",
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


        private void CreateShape()
        {
            var types = new[] { "Rectangle", "Parallelogram", "Triangle", "Rhombus", "Back to Shapes menu" };
            var type = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select shape type or choose 'Back' to go back to Shapes menu:")
                    .AddChoices(types));

            if (type == "Back to Shapes menu")
                return;

            Shape shape = type switch
            {
                "Rectangle" => new Rectangle
                {
                    Width = PromptDouble("Width", "rektangelns bredd"),
                    Height = PromptDouble("Height", "rektangelns höjd")
                },
                "Parallelogram" => new Parallelogram
                {
                    BaseLength = PromptDouble("Base length", "parallellogrammets baslängd"),
                    SideLength = PromptDouble("Side length", "parallellogrammets sidlängd"),
                    Height = PromptDouble("Height", "parallellogrammets höjd")
                },
                "Triangle" => new Triangle
                {
                    BaseLength = PromptDouble("Base length", "triangelns baslängd"),
                    Height = PromptDouble("Height", "triangelns höjd"),
                    SideA = PromptDouble("Side A", "triangelns sida A"),
                    SideB = PromptDouble("Side B", "triangelns sida B")
                },
                "Rhombus" => new Rhombus
                {
                    SideLength = PromptDouble("Side length", "rombens sidlängd"),
                    Height = PromptDouble("Height", "rombens höjd")
                },
                _ => throw new InvalidOperationException()
            };

            shape.Calculate();
            shape.DateCreated = DateTime.Now;
            _context.Add(shape);
            _context.SaveChanges();

            AnsiConsole.MarkupLine("[green]Shape saved![/]");

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("Id");
            table.AddColumn("Type");
            table.AddColumn("Params");
            table.AddColumn("Area");
            table.AddColumn("Perimeter");
            table.AddColumn("Created");

            string paramDesc = shape switch
            {
                Rectangle r => $"W={r.Width}, H={r.Height}",
                Parallelogram p => $"B={p.BaseLength}, S={p.SideLength}, H={p.Height}",
                Triangle t => $"Base={t.BaseLength}, H={t.Height}, S1={t.SideA}, S2={t.SideB}",
                Rhombus h => $"S={h.SideLength}, H={h.Height}",
                _ => string.Empty
            };

            table.AddRow(
                shape.Id.ToString(),
                shape.GetType().Name,
                paramDesc,
                shape.Area.ToString("F2"),
                shape.Perimeter.ToString("F2"),
                shape.DateCreated.ToString("yyyy-MM-dd HH:mm"));

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("[grey]Press enter to continue...[/]");
            Console.ReadLine();
        }

        private void ListShapes()
        {
            var all = _context.Shapes.OrderByDescending(c => c.DateCreated).ToList();

            if (!all.Any())
            {
                AnsiConsole.MarkupLine("[yellow]Inga shapes har skapats än. Skapa en shape först![/]");
                AnsiConsole.MarkupLine("[grey]Press enter to continue...[/]");
                Console.ReadLine();
                return;
            }

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("Id");
            table.AddColumn("Type");
            table.AddColumn("Params");
            table.AddColumn("Area");
            table.AddColumn("Perimeter");
            table.AddColumn("Created");

            foreach (var s in all)
            {
                string paramDesc = s switch
                {
                    Rectangle r => $"W={r.Width}, H={r.Height}",
                    Parallelogram p => $"B={p.BaseLength}, S={p.SideLength}, H={p.Height}",
                    Triangle t => $"Base={t.BaseLength}, H={t.Height}, S1={t.SideA}, S2={t.SideB}",
                    Rhombus h => $"S={h.SideLength}, H={h.Height}",
                    _ => string.Empty
                };

                table.AddRow(
                    s.Id.ToString(),
                    s.GetType().Name,
                    paramDesc,
                    s.Area.ToString("F2"),
                    s.Perimeter.ToString("F2"),
                    s.DateCreated.ToString("yyyy-MM-dd HH:mm"));
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("[grey]Press enter to continue...[/]");
            Console.ReadLine();
        }

        private void UpdateShape()
        {
            var all = _context.Shapes
                .OrderBy(s => s.DateCreated)
                .ToList();

            if (!all.Any())
            {
                AnsiConsole.MarkupLine("[red]Inga shapes att uppdatera![/]");
                AnsiConsole.MarkupLine("[grey]Press enter to continue...[/]");
                Console.ReadLine();
                return;
            }

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("Id");
            table.AddColumn("Type");
            table.AddColumn("Params");
            table.AddColumn("Area");
            table.AddColumn("Perimeter");
            table.AddColumn("Created");

            foreach (var s in all)
            {
                string paramDesc = s switch
                {
                    Rectangle r => $"W={r.Width}, H={r.Height}",
                    Parallelogram p => $"B={p.BaseLength}, S={p.SideLength}, H={p.Height}",
                    Triangle t => $"Base={t.BaseLength}, H={t.Height}, S1={t.SideA}, S2={t.SideB}",
                    Rhombus h => $"S={h.SideLength}, H={h.Height}",
                    _ => string.Empty
                };

                table.AddRow(
                    s.Id.ToString(),
                    s.GetType().Name,
                    paramDesc,
                    s.Area.ToString("F2"),
                    s.Perimeter.ToString("F2"),
                    s.DateCreated.ToString("yyyy-MM-dd HH:mm")
                );
            }

            AnsiConsole.Write(table);

            int id = AnsiConsole.Prompt(
                new TextPrompt<int>("[yellow]Ange [green]Id[/] på den shape du vill uppdatera (eller [red]0[/] för att avbryta):[/]")
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
                    r.Width = PromptDouble("New Width", "rektangelns nya bredd");
                    r.Height = PromptDouble("New Height", "rektangelns nya höjd");
                    break;
                case Parallelogram p:
                    p.BaseLength = PromptDouble("New Base length", "parallellogrammets nya baslängd");
                    p.SideLength = PromptDouble("New Side length", "parallellogrammets nya sidlängd");
                    p.Height = PromptDouble("New Height", "parallellogrammets nya höjd");
                    break;
                case Triangle t:
                    t.BaseLength = PromptDouble("New Base length", "triangelns nya baslängd");
                    t.Height = PromptDouble("New Height", "triangelns nya höjd");
                    t.SideA = PromptDouble("New Side A", "triangelns nya sida A");
                    t.SideB = PromptDouble("New Side B", "triangelns nya sida B");
                    break;
                case Rhombus h:
                    h.SideLength = PromptDouble("New Side length", "rombens nya sidlängd");
                    h.Height = PromptDouble("New Height", "rombens nya höjd");
                    break;
            }

            shape.Calculate();
            _context.SaveChanges();

            AnsiConsole.MarkupLine("[green]Shape updated![/]");
            AnsiConsole.MarkupLine("[grey]Press enter to continue...[/]");
            Console.ReadLine();
        }


        private void DeleteShape()
        {
            var all = _context.Shapes
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.DateCreated)
                .ToList();

            if (!all.Any())
            {
                AnsiConsole.MarkupLine("[red]Inga shapes att radera![/]");
                AnsiConsole.MarkupLine("[grey]Press enter to continue...[/]");
                Console.ReadLine();
                return;
            }

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("Id");
            table.AddColumn("Type");
            table.AddColumn("Params");
            table.AddColumn("Area");
            table.AddColumn("Perimeter");
            table.AddColumn("Created");

            foreach (var s in all)
            {
                string paramDesc = s switch
                {
                    Rectangle r => $"W={r.Width}, H={r.Height}",
                    Parallelogram p => $"B={p.BaseLength}, S={p.SideLength}, H={p.Height}",
                    Triangle t => $"Base={t.BaseLength}, H={t.Height}, S1={t.SideA}, S2={t.SideB}",
                    Rhombus h => $"S={h.SideLength}, H={h.Height}",
                    _ => string.Empty
                };

                table.AddRow(
                    s.Id.ToString(),
                    s.GetType().Name,
                    paramDesc,
                    s.Area.ToString("F2"),
                    s.Perimeter.ToString("F2"),
                    s.DateCreated.ToString("yyyy-MM-dd HH:mm")
                );
            }

            AnsiConsole.Write(table);

            int id = AnsiConsole.Prompt(
                new TextPrompt<int>("[yellow]Ange [green]Id[/] på shape att radera (eller [red]0[/] för att avbryta):[/]")
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
            shape.IsDeleted = true;
            _context.SaveChanges();

            AnsiConsole.MarkupLine("[green]Shape deleted![/]");
            AnsiConsole.MarkupLine("[grey]Press enter to continue...[/]");
            Console.ReadLine();
        }

        private double PromptDouble(string name, string swedishDescription = null)
        {
            string description = swedishDescription ?? name.ToLower();

            return AnsiConsole.Prompt(
                new TextPrompt<double>($"Enter {name}:")
                    .ValidationErrorMessage($"[red]Fel: Du måste ange ett giltigt tal för {description}. Text som 'bajskorv' fungerar inte![/]")
                    .Validate(n =>
                    {
                        if (n <= 0)
                            return ValidationResult.Error($"[red]Fel: {char.ToUpper(description[0]) + description.Substring(1)} måste vara större än 0![/]");

                        if (double.IsInfinity(n))
                            return ValidationResult.Error($"[red]Fel: Talet för {description} är för stort. Ange ett mindre tal![/]");

                        if (double.IsNaN(n))
                            return ValidationResult.Error($"[red]Fel: Ogiltigt tal för {description}. Försök igen![/]");

                        // Extra validering för rimliga värden
                        if (n > 1000000)
                            return ValidationResult.Error($"[red]Fel: {char.ToUpper(description[0]) + description.Substring(1)} verkar orimligt stor (>1000000). Kontrollera värdet![/]");

                        return ValidationResult.Success();
                    })
            );
        }
    }
}