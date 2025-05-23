using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcessLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Operand1 = table.Column<double>(type: "float", nullable: false),
                    Operand2 = table.Column<double>(type: "float", nullable: true),
                    Operator = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Result = table.Column<double>(type: "float", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RpsGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerMove = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComputerMove = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Outcome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatePlayed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RpsGames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shapes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Area = table.Column<double>(type: "float", nullable: false),
                    Perimeter = table.Column<double>(type: "float", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShapeType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    BaseLength = table.Column<double>(type: "float", nullable: true),
                    Parallelogram_SideLength = table.Column<double>(type: "float", nullable: true),
                    Parallelogram_Height = table.Column<double>(type: "float", nullable: true),
                    Width = table.Column<double>(type: "float", nullable: true),
                    Height = table.Column<double>(type: "float", nullable: true),
                    SideLength = table.Column<double>(type: "float", nullable: true),
                    Diagonal1 = table.Column<double>(type: "float", nullable: true),
                    Diagonal2 = table.Column<double>(type: "float", nullable: true),
                    SideA = table.Column<double>(type: "float", nullable: true),
                    SideB = table.Column<double>(type: "float", nullable: true),
                    SideC = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shapes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calculations");

            migrationBuilder.DropTable(
                name: "RpsGames");

            migrationBuilder.DropTable(
                name: "Shapes");
        }
    }
}
