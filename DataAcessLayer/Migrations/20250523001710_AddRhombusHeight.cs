using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddRhombusHeight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SideC",
                table: "Shapes",
                newName: "Triangle_Height");

            migrationBuilder.RenameColumn(
                name: "Diagonal2",
                table: "Shapes",
                newName: "Triangle_BaseLength");

            migrationBuilder.RenameColumn(
                name: "Diagonal1",
                table: "Shapes",
                newName: "Rectangle_Height");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Triangle_Height",
                table: "Shapes",
                newName: "SideC");

            migrationBuilder.RenameColumn(
                name: "Triangle_BaseLength",
                table: "Shapes",
                newName: "Diagonal2");

            migrationBuilder.RenameColumn(
                name: "Rectangle_Height",
                table: "Shapes",
                newName: "Diagonal1");
        }
    }
}
