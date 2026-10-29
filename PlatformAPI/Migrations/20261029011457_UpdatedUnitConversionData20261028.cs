using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUnitConversionData20261028 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IngredientType",
                table: "UnitPrecisionRules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IngredientType",
                table: "IngredientUnits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 3, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 4, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "IngredientType", "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { 4, "cup", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 32,
                column: "IngredientType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 33,
                column: "IngredientType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 34,
                column: "IngredientType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 35,
                column: "IngredientType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 36,
                column: "IngredientType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 37,
                column: "IngredientType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "Ingredient", "IngredientType" },
                values: new object[] { "Sesame oil", 4 });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 39,
                column: "IngredientType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 40,
                column: "IngredientType",
                value: 5);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 1, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 58,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 59,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 60,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 61,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 62,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 63,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 4, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 4, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 4, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 4, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 2, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 2, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 2, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "Ingredient", "IngredientType", "SmallImperialUnit" },
                values: new object[] { "Heavy cream", 2, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 2, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 2, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 1, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 1, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 1, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 80,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 1, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 1, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 83,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 84,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 85,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 86,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 87,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 88,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 89,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 2, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 2, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "Ingredient", "IngredientType" },
                values: new object[] { "green beans", 1 });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 93,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 94,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 95,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 96,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 97,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 98,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 99,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 100,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 101,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 102,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 103,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 104,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 105,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 106,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 107,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 108,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 109,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 110,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 111,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 112,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 113,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 114,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 115,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 116,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 117,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 118,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 119,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 120,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 121,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 122,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 123,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 124,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 125,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 126,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 127,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 128,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 129,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 130,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 131,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 132,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 133,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 134,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 135,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 136,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 137,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 138,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 139,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 140,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 141,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 142,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 143,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 144,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 145,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 146,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 147,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 148,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 149,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 150,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 151,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 152,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 153,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 154,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 155,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 156,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 157,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 158,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 159,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 160,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 161,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 162,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 163,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 164,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 165,
                columns: new[] { "IngredientType", "Pass" },
                values: new object[] { 1, 2 });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 166,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 167,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 168,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 169,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 170,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 171,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 172,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 173,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 174,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 175,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 176,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 177,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 178,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 179,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 180,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 181,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 182,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 183,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 184,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 185,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 186,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 187,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 188,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 189,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 3, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 190,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 1, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 191,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 192,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 193,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 194,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 195,
                column: "IngredientType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 196,
                column: "IngredientType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 197,
                column: "IngredientType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 198,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 3, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 199,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 3, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 200,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 201,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 202,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 203,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 204,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 205,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 1, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 206,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 1, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 207,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 1, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 208,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 2, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 209,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 5, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 210,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 2, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 211,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 2, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 212,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 2, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 213,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 214,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 215,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 216,
                column: "IngredientType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 217,
                columns: new[] { "IngredientType", "SmallImperialUnit" },
                values: new object[] { 2, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 218,
                columns: new[] { "IngredientType", "Pass", "SmallImperialUnit" },
                values: new object[] { 4, 1, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 219,
                columns: new[] { "IngredientType", "Pass", "SmallImperialUnit" },
                values: new object[] { 4, 1, "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 220,
                columns: new[] { "IngredientType", "LargeMetricUnit", "MetricUnit", "Pass", "SmallImperialUnit" },
                values: new object[] { 2, "kg", "g", 1, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 221,
                columns: new[] { "IngredientType", "LargeMetricUnit", "MetricUnit", "Pass", "SmallImperialUnit" },
                values: new object[] { 1, "kg", "g", 1, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 222,
                columns: new[] { "Ingredient", "IngredientType", "Pass", "SmallImperialUnit" },
                values: new object[] { "soup", 2, 1, "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 223,
                columns: new[] { "Ingredient", "IngredientType", "IsLiquid", "LargeImperialUnit", "LargeMetricUnit", "MetricUnit", "SmallImperialUnit" },
                values: new object[] { "ground pepper", 3, false, "tbsp", "kg", "g", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 224,
                columns: new[] { "Ingredient", "IngredientType" },
                values: new object[] { "crushed pepper", 3 });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 225,
                columns: new[] { "Ingredient", "IngredientType" },
                values: new object[] { "garlic powder", 3 });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 226,
                columns: new[] { "Ingredient", "IngredientType" },
                values: new object[] { "ginger powder", 3 });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 227,
                columns: new[] { "Ingredient", "IngredientType", "IsLiquid", "LargeImperialUnit", "LargeMetricUnit", "MetricUnit", "SmallImperialUnit" },
                values: new object[] { "coconut milk", 2, true, "cup", "L", "ml", "oz" });

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 1,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 2,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 3,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 4,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 5,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 6,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 7,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 8,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 9,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 10,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 11,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 12,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 13,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 14,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 15,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 16,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 17,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 18,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 19,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 20,
                column: "IngredientType",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 21,
                column: "IngredientType",
                value: null);

            migrationBuilder.InsertData(
                table: "UnitPrecisionRules",
                columns: new[] { "Id", "Comparison", "Condition", "ConversionFactor", "IngredientType", "NewUnit", "RoundToConditional", "RoundToElse", "Threshold", "Unit" },
                values: new object[,]
                {
                    { 22, ">=", null, 0.125f, 1, "cup", null, null, 7.875f, "oz" },
                    { 23, ">=", null, 0.0625f, 1, "lb", null, null, 15.875f, "cup" },
                    { 24, ">=", null, 0.33333334f, 3, "Tbsp", null, null, 2.875f, "tsp" },
                    { 25, ">=", null, 0.5f, 3, "oz", null, null, 2.875f, "Tbsp" },
                    { 26, ">=", null, 0.33333334f, 4, "Tbsp", null, null, 2.875f, "tsp" },
                    { 27, ">=", null, 0.33333334f, 2, "Tbsp", null, null, 2.875f, "tsp" },
                    { 28, ">=", null, 0.5f, 2, "oz", null, null, 1.875f, "Tbsp" },
                    { 29, ">=", null, 0.125f, 2, "cup", null, null, 7.875f, "oz" },
                    { 30, ">=", null, 0.0625f, 2, "gal", null, null, 15.875f, "cup" },
                    { 31, ">=", null, 0.33333334f, 5, "Tbsp", null, null, 2.875f, "tsp" },
                    { 32, ">=", null, 0.5f, 5, "oz", null, null, 1.875f, "Tbsp" },
                    { 33, ">=", null, 0.125f, 5, "cup", null, null, 7.875f, "oz" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DropColumn(
                name: "IngredientType",
                table: "UnitPrecisionRules");

            migrationBuilder.DropColumn(
                name: "IngredientType",
                table: "IngredientUnits");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "LargeImperialUnit", "SmallImperialUnit" },
                values: new object[] { "tbsp", "tsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 38,
                column: "Ingredient",
                value: "sesame oil");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 41,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 42,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 43,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 44,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 45,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 46,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 47,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 48,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 49,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 50,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 51,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 52,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 53,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 54,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 55,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 56,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 57,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 64,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 65,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 66,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 67,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 68,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 69,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 70,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 71,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 72,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 73,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "Ingredient", "SmallImperialUnit" },
                values: new object[] { "heavy cream", "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 75,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 76,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 77,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 78,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 79,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 81,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 82,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 90,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 91,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 92,
                column: "Ingredient",
                value: "chicken breast");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 165,
                column: "Pass",
                value: 1);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 183,
                column: "SmallImperialUnit",
                value: "oz");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 184,
                column: "SmallImperialUnit",
                value: "oz");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 185,
                column: "SmallImperialUnit",
                value: "oz");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 186,
                column: "SmallImperialUnit",
                value: "oz");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 187,
                column: "SmallImperialUnit",
                value: "oz");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 188,
                column: "SmallImperialUnit",
                value: "oz");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 189,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 190,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 198,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 199,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 200,
                column: "SmallImperialUnit",
                value: "oz");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 201,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 202,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 203,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 204,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 205,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 206,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 207,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 208,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 209,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 210,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 211,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 212,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 217,
                column: "SmallImperialUnit",
                value: "cup");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 218,
                columns: new[] { "Pass", "SmallImperialUnit" },
                values: new object[] { 2, "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 219,
                columns: new[] { "Pass", "SmallImperialUnit" },
                values: new object[] { 2, "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 220,
                columns: new[] { "LargeMetricUnit", "MetricUnit", "Pass", "SmallImperialUnit" },
                values: new object[] { "L", "ml", 2, "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 221,
                columns: new[] { "LargeMetricUnit", "MetricUnit", "Pass", "SmallImperialUnit" },
                values: new object[] { "L", "ml", 2, "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 222,
                columns: new[] { "Ingredient", "Pass", "SmallImperialUnit" },
                values: new object[] { "butter", 2, "tbsp" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 223,
                columns: new[] { "Ingredient", "IsLiquid", "LargeImperialUnit", "LargeMetricUnit", "MetricUnit", "SmallImperialUnit" },
                values: new object[] { "soup", true, "cup", "L", "ml", "oz" });

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 224,
                column: "Ingredient",
                value: "ground pepper");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 225,
                column: "Ingredient",
                value: "crushed pepper");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 226,
                column: "Ingredient",
                value: "garlic powder");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 227,
                columns: new[] { "Ingredient", "IsLiquid", "LargeImperialUnit", "LargeMetricUnit", "MetricUnit", "SmallImperialUnit" },
                values: new object[] { "ginger powder", false, "tbsp", "kg", "g", "tsp" });
        }
    }
}
