using System;
using System.Linq;
using DataAcessLayer;
using DataAcessLayer.ModelsRPS;
using Spectre.Console;

namespace SharedLibrary
{
    public class RpsMenu
    {
        private readonly AllDbContext _context;
        private readonly Random _rng;

        public RpsMenu(AllDbContext context)
        {
            _context = context;
            _rng = new Random();
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
                    case 1:
                        PlayNewGame();
                        break;
                    case 2:
                        ListAllGames();
                        break;
                    case 3:
                        back = true;
                        break;
                }
            }
        }

        private void ShowHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText("STEN-SAX-PÅSE")
                    .Centered()
                    .Color(Color.Green));
            AnsiConsole.Write(new Rule());
        }

        private int PromptChoice()
        {
            var options = new[]
            {
                "1. Spela nytt spel",
                "2. Lista alla tidigare spel",
                "3. Tillbaka till huvudmenyn"
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

        private void PlayNewGame()
        {
            // 1) Låt användaren välja Sten/Sax/Påse eller Avbryt
            string playerMove = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Välj ditt drag (eller [red]Avbryt[/]):")
                    .AddChoices(new[] { "Sten", "Sax", "Påse", "Avbryt" }));

            if (playerMove == "Avbryt")
                return;

            // 2) Datorn väljer slumpmässigt
            var moves = new[] { "Sten", "Sax", "Påse" };
            string computerMove = moves[_rng.Next(moves.Length)];

            // 3) Avgör resultat (Vinst/Förlust/Oavgjort)
            string outcome = DetermineOutcome(playerMove, computerMove);

            // 4) Hämta befintligt antal spel & vinster i DB (före det här)
            int previousTotal = _context.RpsGames.Count();
            int previousWins = _context.RpsGames.Count(g => g.Outcome == "Vinst");

            // 5) Beräkna ny total + ny winRate (inkl. detta spel)
            int newTotal = previousTotal + 1;
            int newWinCount = previousWins + (outcome == "Vinst" ? 1 : 0);
            decimal winRate = newTotal > 0
                ? Math.Round((decimal)newWinCount / newTotal * 100, 2)
                : 0m;

            // 6) Skapa och spara entitet
            var game = new RPS
            {
                PlayerMove = playerMove,
                ComputerMove = computerMove,
                Outcome = outcome,
                DatePlayed = DateTime.Now,
                WinRate = winRate
            };
            _context.RpsGames.Add(game);
            _context.SaveChanges();

            // 7) Visa resultatet i en tabell
            var table = new Table().Border(TableBorder.Rounded)
                .AddColumn("Ditt drag")
                .AddColumn("Datorns drag")
                .AddColumn("Resultat")
                .AddColumn("Vinst% (alla spel)");

            table.AddRow(
                game.PlayerMove,
                game.ComputerMove,
                game.Outcome,
                $"{game.WinRate:F2} %"
            );

            switch (outcome)
            {
                case "Vinst":
                    AnsiConsole.MarkupLine("  [bold green]Grattis! Du vann![/]");
                    break;
                case "Förlust":
                    AnsiConsole.MarkupLine("  [bold red]Tyvärr! Du förlorade![/]");
                    break;
                case "Oavgjort":
                    AnsiConsole.MarkupLine("  [yellow]Det blev oavgjort![/]");
                    break;
            }

            AnsiConsole.Write(table);

            AnsiConsole.MarkupLine("[green]Spelet är sparat![/]");

            AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
            Console.ReadLine();
        }



        private void ListAllGames()
        {
            var allGames = _context.RpsGames
                .OrderByDescending(g => g.DatePlayed)
                .ToList();

            if (!allGames.Any())
            {
                AnsiConsole.MarkupLine("[yellow]Inga spel har spelats än. Spela ett spel först![/]");
                AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
                Console.ReadLine();
                return;
            }

            var table = new Table().Border(TableBorder.Rounded)
                .AddColumn("Id")
                .AddColumn("Datum")
                .AddColumn("Ditt drag")
                .AddColumn("Datorns drag")
                .AddColumn("Resultat")
                .AddColumn("Vinst%");

            foreach (var g in allGames)
            {
                table.AddRow(
                    g.Id.ToString(),
                    g.DatePlayed.ToString("yyyy-MM-dd HH:mm"),
                    g.PlayerMove,
                    g.ComputerMove,
                    g.Outcome,
                    $"{g.WinRate:F2} %"
                );
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
            Console.ReadLine();
        }

        private string DetermineOutcome(string player, string computer)
        {
            if (player == computer)
                return "Oavgjort";

            if ((player == "Sten" && computer == "Sax") ||
                (player == "Sax" && computer == "Påse") ||
                (player == "Påse" && computer == "Sten"))
            {
                return "Vinst";
            }
            else
            {
                return "Förlust";
            }
        }
    }
}
