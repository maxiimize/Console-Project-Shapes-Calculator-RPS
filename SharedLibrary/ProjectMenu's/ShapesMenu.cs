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
                AnsiConsole.Clear();
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[yellow]Shapes Menu[/]")
                        .AddChoices(new[]
                        {
                            "1. Create new shape",
                            "2. List all shapes",
                            "3. Update a shape",
                            "4. Delete a shape",
                            "5. Back to Main Menu"
                        }));

                switch (choice[0])
                {
                    case '1': CreateShape(); break;
                    case '2': ListShapes(); break;
                    case '3': UpdateShape(); break;
                    case '4': DeleteShape(); break;
                    case '5': back = true; break;
                }
            }
        }

        private void CreateShape()
        {
            // 1) Välj typ
            var type = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select shape type:")
                    .AddChoices("Rectangle", "Parallelogram", "Triangle", "Rhombus"));

            Shape shape = type switch
            {
                "Rectangle" => new Rectangle(),
                "Parallelogram" => new Parallelogram(),
                "Triangle" => new Triangle(),
                "Rhombus" => new Rhombus(),
                _ => null
            };

            // 2) Läs in parametrar och beräkna area/omkrets
            switch (shape)
            {
                case Rectangle r:
                    r.Width = PromptDouble("Width");
                    r.Height = PromptDouble("Height");
                    r.Area = r.Width * r.Height;
                    r.Perimeter = 2 * (r.Width + r.Height);
                    break;

                case Parallelogram p:
                    p.BaseLength = PromptDouble("Base length");
                    p.Height = PromptDouble("Height");
                    p.SideLength = PromptDouble("Side length");
                    p.Area = p.BaseLength * p.Height;
                    p.Perimeter = 2 * (p.BaseLength + p.SideLength);
                    break;

                case Triangle t:
                    t.SideA = PromptDouble("Side A");
                    t.SideB = PromptDouble("Side B");
                    t.SideC = PromptDouble("Side C");
                    var s = (t.SideA + t.SideB + t.SideC) / 2;
                    t.Area = Math.Sqrt(s * (s - t.SideA) * (s - t.SideB) * (s - t.SideC));
                    t.Perimeter = t.SideA + t.SideB + t.SideC;
                    break;

                case Rhombus h:
                    h.SideLength = PromptDouble("Side length");
                    h.Diagonal1 = PromptDouble("Diagonal 1");
                    h.Diagonal2 = PromptDouble("Diagonal 2");
                    h.Area = (h.Diagonal1 * h.Diagonal2) / 2;
                    h.Perimeter = 4 * h.SideLength;
                    break;
            }

            shape.DateCreated = DateTime.Now;
            _context.Add(shape);
            _context.SaveChanges();
            AnsiConsole.MarkupLine("[green]Shape saved![/]");
            AnsiConsole.Prompt(new TextPrompt<string>("Press [grey]enter[/] to continue"));
        }

        private void ListShapes()
        {
            var all = _context.Shapes
                .OrderBy(s => s.DateCreated)
                .ToList();

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
                    Triangle t => $"A={t.SideA}, B={t.SideB}, C={t.SideC}",
                    Rhombus h => $"S={h.SideLength}, D1={h.Diagonal1}, D2={h.Diagonal2}",
                    _ => ""
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
            AnsiConsole.Prompt(new TextPrompt<string>("Press [grey]enter[/] to continue"));
        }

        private void UpdateShape()
        {
            int id = (int)AnsiConsole.Prompt(new TextPrompt<int>("Enter [yellow]Id[/] of shape to update:"));
            var shape = _context.Shapes.Find(id);
            if (shape == null)
            {
                AnsiConsole.MarkupLine("[red]Not found![/]");
                return;
            }            
            _context.Entry(shape).State = EntityState.Detached;
            CreateShape();
        }

        private void DeleteShape()
        {
            int id = (int)AnsiConsole.Prompt(new TextPrompt<int>("Enter [yellow]Id[/] of shape to delete:"));
            var shape = _context.Shapes.Find(id);
            if (shape == null)
            {
                AnsiConsole.MarkupLine("[red]Not found![/]");
            }
            else
            {
                _context.Remove(shape);
                _context.SaveChanges();
                AnsiConsole.MarkupLine("[green]Deleted[/]");
            }
            AnsiConsole.Prompt(new TextPrompt<string>("Press [grey]enter[/] to continue"));
        }

        private double PromptDouble(string name)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<double>($"Enter [yellow]{name}[/]:")
                    .Validate(n => n > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]Must be > 0[/]")));
        }
    }
}
