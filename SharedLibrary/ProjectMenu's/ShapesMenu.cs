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
            var type = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select shape type:")
                    .AddChoices("Rectangle", "Parallelogram", "Triangle", "Rhombus"));

            Shape shape = type switch
            {
                "Rectangle" => new Rectangle
                {
                    Width = PromptDouble("Width"),
                    Height = PromptDouble("Height")
                },
                "Parallelogram" => new Parallelogram
                {
                    BaseLength = PromptDouble("Base length"),
                    SideLength = PromptDouble("Side length"),
                    Height = PromptDouble("Height")
                },
                "Triangle" => new Triangle
                {
                    BaseLength = PromptDouble("Base length"),
                    Height = PromptDouble("Height"),
                    SideA = PromptDouble("Side A"),
                    SideB = PromptDouble("Side B")
                },
                "Rhombus" => new Rhombus
                {
                    SideLength = PromptDouble("Side length"),
                    Height = PromptDouble("Height")
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
            var all = _context.Shapes.OrderBy(s => s.DateCreated).ToList();
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
            var allShapes = _context.Shapes.OrderBy(s => s.DateCreated).ToList();
            if (!allShapes.Any())
            {
                AnsiConsole.MarkupLine("[red]Inga shapes att uppdatera![/]");
                AnsiConsole.MarkupLine("[grey]Press enter to continue...[/]");
                Console.ReadLine();
                return;
            }

            var options = allShapes
                .Select(s =>
                {
                    string paramDesc = s switch
                    {
                        Rectangle r => $"W={r.Width}, H={r.Height}",
                        Parallelogram p => $"B={p.BaseLength}, S={p.SideLength}, H={p.Height}",
                        Triangle t => $"Base={t.BaseLength}, H={t.Height}, S1={t.SideA}, S2={t.SideB}",
                        Rhombus h => $"S={h.SideLength}, H={h.Height}",
                        _ => ""
                    };
                    return string.Format(
                        "{0,3} | {1,-12} | {2,-30} | {3:yyyy-MM-dd HH:mm}",
                        s.Id,
                        s.GetType().Name,
                        paramDesc,
                        s.DateCreated
                    );
                })
                .ToList();

            options.Insert(0, "  0 | Back");

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Choose a shape to update (0 = Back):[/]")
                    .PageSize(Math.Min(options.Count, 10))
                    .AddChoices(options)
            );

            if (selection.StartsWith("  0"))
                return;

            var id = int.Parse(selection.Split('|')[0]);
            var shape = _context.Shapes.Find(id)!;

            switch (shape)
            {
                case Rectangle r:
                    r.Width = PromptDouble("New Width");
                    r.Height = PromptDouble("New Height");
                    break;
                case Parallelogram p:
                    p.BaseLength = PromptDouble("New Base length");
                    p.SideLength = PromptDouble("New Side length");
                    p.Height = PromptDouble("New Height");
                    break;
                case Triangle t:
                    t.BaseLength = PromptDouble("New Base length");
                    t.Height = PromptDouble("New Height");
                    t.SideA = PromptDouble("New Side A");
                    t.SideB = PromptDouble("New Side B");
                    break;
                case Rhombus h:
                    h.SideLength = PromptDouble("New Side length");
                    h.Height = PromptDouble("New Height");
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
            var allShapes = _context.Shapes.OrderBy(s => s.DateCreated).ToList();
            if (!allShapes.Any())
            {
                AnsiConsole.MarkupLine("[red]Inga shapes att radera![/]");
                AnsiConsole.MarkupLine("[grey]Press enter to continue...[/]");
                Console.ReadLine();
                return;
            }

            var options = allShapes
                .Select(s =>
                {
                    string paramDesc = s switch
                    {
                        Rectangle r => $"W={r.Width}, H={r.Height}",
                        Parallelogram p => $"B={p.BaseLength}, S={p.SideLength}, H={p.Height}",
                        Triangle t => $"Base={t.BaseLength}, H={t.Height}, S1={t.SideA}, S2={t.SideB}",
                        Rhombus h => $"S={h.SideLength}, H={h.Height}",
                        _ => ""
                    };
                    return string.Format(
                        "{0,3} | {1,-12} | {2,-30} | {3:yyyy-MM-dd HH:mm}",
                        s.Id,
                        s.GetType().Name,
                        paramDesc,
                        s.DateCreated
                    );
                })
                .ToList();

            options.Insert(0, "  0 | Back");

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Choose a shape to delete (0 = Back):[/]")
                    .PageSize(Math.Min(options.Count, 10))
                    .AddChoices(options)
            );

            if (selection.StartsWith("  0"))
                return;

            var id = int.Parse(selection.Split('|')[0]);
            var shape = _context.Shapes.Find(id)!;

            shape.IsDeleted = true;
            _context.SaveChanges();

            AnsiConsole.MarkupLine("[green]Shape deleted![/]");
            AnsiConsole.MarkupLine("[grey]Press enter to continue...[/]");
            Console.ReadLine();
        }

        private double PromptDouble(string name)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<double>($"Enter {name}:")
                    .Validate(n => n > 0 ? ValidationResult.Success() : ValidationResult.Error("Must be > 0")));
        }
    }
}
