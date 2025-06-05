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
                        ShowStatistics();
                        break;
                    case 4:
                        back = true;
                        break;
                    default:
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
                "2. Visa alla tidigare spel",
                "3. Visa statistik",
                "4. Tillbaka till huvudmenyn"
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
                .AddColumn("Ditt drag")
                .AddColumn("Datorns drag")
                .AddColumn("Resultat")
                .AddColumn("Vinst%")
                .AddColumn("Datum");

            foreach (var g in allGames)
            {
                table.AddRow(
                    g.Id.ToString(),
                    g.PlayerMove,
                    g.ComputerMove,
                    g.Outcome,
                    $"{g.WinRate:F2} %",
                    g.DatePlayed.ToString("yyyy-MM-dd HH:mm")
                );
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
            Console.ReadLine();
        }


        private void ShowStatistics()
        {
            var allGames = _context.RpsGames.ToList();

            if (!allGames.Any())
            {
                AnsiConsole.MarkupLine("[yellow]Inga spel registrerade ännu – inga statistikdata att visa.[/]");
                AnsiConsole.MarkupLine("[grey]Tryck enter för att fortsätta...[/]");
                Console.ReadLine();
                return;
            }

            // Total statistik
            int totalGames = allGames.Count;
            int totalWins = allGames.Count(g => g.Outcome == "Vinst");
            int totalLosses = allGames.Count(g => g.Outcome == "Förlust");
            decimal overallWinRate = totalGames > 0
                ? Math.Round((decimal)totalWins / totalGames * 100, 2)
                : 0m;

            // Statistik per drag: Sten
            var stoneGames = allGames.Where(g => g.PlayerMove == "Sten").ToList();
            int stoneCount = stoneGames.Count;
            int stoneWins = stoneGames.Count(g => g.Outcome == "Vinst");
            int stoneLosses = stoneGames.Count(g => g.Outcome == "Förlust");
            decimal stoneWinRate = stoneCount > 0
                ? Math.Round((decimal)stoneWins / stoneCount * 100, 2)
                : 0m;

            // Statistik per drag: Sax
            var scissorGames = allGames.Where(g => g.PlayerMove == "Sax").ToList();
            int scissorCount = scissorGames.Count;
            int scissorWins = scissorGames.Count(g => g.Outcome == "Vinst");
            int scissorLosses = scissorGames.Count(g => g.Outcome == "Förlust");
            decimal scissorWinRate = scissorCount > 0
                ? Math.Round((decimal)scissorWins / scissorCount * 100, 2)
                : 0m;

            // Statistik per drag: Påse
            var paperGames = allGames.Where(g => g.PlayerMove == "Påse").ToList();
            int paperCount = paperGames.Count;
            int paperWins = paperGames.Count(g => g.Outcome == "Vinst");
            int paperLosses = paperGames.Count(g => g.Outcome == "Förlust");
            decimal paperWinRate = paperCount > 0
                ? Math.Round((decimal)paperWins / paperCount * 100, 2)
                : 0m;

            var table = new Table().Border(TableBorder.Rounded)
                .AddColumn("Kategori")
                .AddColumn("Antal spel")
                .AddColumn("Antal vinster")
                .AddColumn("Antal förluster")
                .AddColumn("Vinstprocent");

            table.AddRow(
                "Totalt",
                totalGames.ToString(),
                totalWins.ToString(),
                totalLosses.ToString(),
                $"{overallWinRate:F2} %"
            );

            table.AddRow(
                "Sten",
                stoneCount.ToString(),
                stoneWins.ToString(),
                stoneLosses.ToString(),
                $"{stoneWinRate:F2} %"
            );

            table.AddRow(
                "Sax",
                scissorCount.ToString(),
                scissorWins.ToString(),
                scissorLosses.ToString(),
                $"{scissorWinRate:F2} %"
            );

            table.AddRow(
                "Påse",
                paperCount.ToString(),
                paperWins.ToString(),
                paperLosses.ToString(),
                $"{paperWinRate:F2} %"
            );

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
