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
                        .Title("Shapes Menu")
                        .AddChoices("1. Create new shape", "2. List all shapes", "3. Update a shape", "4. Delete a shape", "5. Back to Main Menu"));
                switch (choice[0])
                {
                    case '1':
                        CreateShape();
                        break;
                    case '2':
                        ListShapes();
                        break;
                    case '3':
                        UpdateShape();
                        break;
                    case '4':
                        DeleteShape();
                        break;
                    case '5':
                        back = true;
                        break;
                }
            }
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
                    SideA = PromptDouble("Side A"),
                    SideB = PromptDouble("Side B"),
                    SideC = PromptDouble("Side C")
                },
                "Rhombus" => new Rhombus
                {
                    SideLength = PromptDouble("Side length"),
                    Diagonal1 = PromptDouble("Diagonal 1"),
                    Diagonal2 = PromptDouble("Diagonal 2")
                },
                _ => throw new InvalidOperationException()
            };

            shape.Calculate();
            shape.DateCreated = DateTime.Now;
            _context.Add(shape);
            _context.SaveChanges();

            AnsiConsole.MarkupLine("[green]Shape saved![/]");
            AnsiConsole.Prompt(new TextPrompt<string>("Press enter to continue"));
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
                    Triangle t => $"A={t.SideA}, B={t.SideB}, C={t.SideC}",
                    Rhombus h => $"S={h.SideLength}, D1={h.Diagonal1}, D2={h.Diagonal2}",
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
            AnsiConsole.Prompt(new TextPrompt<string>("Press enter to continue"));
        }

        private void UpdateShape()
        {
            int id = AnsiConsole.Prompt(new TextPrompt<int>("Enter Id of shape to update:"));
            var shape = _context.Shapes.Find(id);
            if (shape == null)
            {
                AnsiConsole.MarkupLine("[red]Not found![/]");
                AnsiConsole.Prompt(new TextPrompt<string>("Press enter to continue"));
                return;
            }

            _context.Entry(shape).State = EntityState.Detached;
            CreateShape();
        }

        private void DeleteShape()
        {
            int id = AnsiConsole.Prompt(new TextPrompt<int>("Enter Id of shape to delete:"));
            var shape = _context.Shapes.Find(id);
            if (shape != null)
            {
                _context.Remove(shape);
                _context.SaveChanges();
                AnsiConsole.MarkupLine("[green]Deleted[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Not found![/]");
            }
            AnsiConsole.Prompt(new TextPrompt<string>("Press enter to continue"));
        }

        private double PromptDouble(string name)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<double>($"Enter {name}:")
                    .Validate(n => n > 0 ? ValidationResult.Success() : ValidationResult.Error("Must be > 0")));
        }
    }
}
