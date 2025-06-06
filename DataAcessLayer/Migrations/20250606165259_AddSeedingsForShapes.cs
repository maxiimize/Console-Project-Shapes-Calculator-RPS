using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedingsForShapes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Shapes",
                columns: new[] { "Id", "Area", "DateCreated", "Rectangle_Height", "IsDeleted", "Perimeter", "ShapeType", "Width" },
                values: new object[] { 1, 50.0, new DateTime(2024, 6, 7, 10, 0, 0, 0, DateTimeKind.Unspecified), 5.0, false, 30.0, "Rectangle", 10.0 });

            migrationBuilder.InsertData(
                table: "Shapes",
                columns: new[] { "Id", "Area", "BaseLength", "DateCreated", "Parallelogram_Height", "IsDeleted", "Perimeter", "ShapeType", "Parallelogram_SideLength" },
                values: new object[] { 2, 32.0, 8.0, new DateTime(2024, 6, 7, 10, 5, 0, 0, DateTimeKind.Unspecified), 4.0, false, 28.0, "Parallelogram", 6.0 });

            migrationBuilder.InsertData(
                table: "Shapes",
                columns: new[] { "Id", "Area", "Triangle_BaseLength", "DateCreated", "Triangle_Height", "IsDeleted", "Perimeter", "ShapeType", "SideA", "SideB" },
                values: new object[] { 3, 12.0, 6.0, new DateTime(2024, 6, 7, 10, 10, 0, 0, DateTimeKind.Unspecified), 4.0, false, 16.0, "Triangle", 5.0, 5.0 });

            migrationBuilder.InsertData(
                table: "Shapes",
                columns: new[] { "Id", "Area", "DateCreated", "Height", "IsDeleted", "Perimeter", "ShapeType", "SideLength" },
                values: new object[] { 4, 21.0, new DateTime(2024, 6, 7, 10, 15, 0, 0, DateTimeKind.Unspecified), 3.0, false, 28.0, "Rhombus", 7.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Shapes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Shapes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Shapes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Shapes",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
