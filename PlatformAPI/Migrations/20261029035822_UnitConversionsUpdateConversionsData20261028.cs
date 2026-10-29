using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class UnitConversionsUpdateConversionsData20261028 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 2, "Gram", 0.33814f, 1, "Teaspoon" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Gram", 0.035274f, "Ounce" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 2, "Kilogram", 338.14f, 1, "Teaspoon" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Kilogram", 35.274f, "Ounce" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 2, "Milliliter", 0.202884f, 1, "Teaspoon" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 8,
                column: "TargetUnit",
                value: "Ounce");

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 2, "Liter", 202.884f, 1, "Teaspoon" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Liter", 33.814f, "Ounce" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Teaspoon", 2.95735f, "Gram" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 1, "Teaspoon", 4.92892f, 2, "Milliliter" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Tablespoon", 8.87206f, "Gram" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 1, "Tablespoon", 14.7868f, 2, "Milliliter" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Ounce", 28.3495f, "Gram" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 1, "Ounce", 29.5735f, 2, "Milliliter" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "BaseUnit", "Quantity" },
                values: new object[] { "Cup", 141.953f });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 1, "Cup", 236.588f, 2, "Milliliter" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Gallon", 2271.25f, "Gram" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 1, "Gallon", 3785.41f, 2, "Milliliter" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Pint", 283.906f, "Gram" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 1, "Pint", 473.176f, 2, "Milliliter" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Quart", 567.812f, "Gram" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 1, "Quart", 946.353f, 2, "Milliliter" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Fluid Ounce", 17.7441f, "Gram" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 1, "Fluid Ounce", 29.5735f, 2, "Milliliter" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 1, "Teaspoon", 4.92892f, 2, "Milliliter" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Milliliter", 0.202884f, "Teaspoon" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 1, "Tablespoon", 14.7868f, 2, "Milliliter" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Milliliter", 0.067628f, "Tablespoon" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 1, "Fluid Ounce", 29.5735f, 2, "Milliliter" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 8,
                column: "TargetUnit",
                value: "Teaspoon");

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 1, "Cup", 236.588f, 2, "Milliliter" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Milliliter", 0.00422675f, "Cup" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Pint", 473.176f, "Milliliter" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 2, "Milliliter", 0.00211338f, 1, "Pint" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Quart", 0.946353f, "Liter" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 2, "Liter", 1.05669f, 1, "Quart" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Gallon", 3.78541f, "Liter" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 2, "Liter", 0.264172f, 1, "Gallon" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "BaseUnit", "Quantity" },
                values: new object[] { "Ounce", 28.3495f });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 2, "Gram", 0.035274f, 1, "Ounce" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Pound", 0.453592f, "Kilogram" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 2, "Kilogram", 2.20462f, 1, "Pound" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Inch", 25.4f, "Millimeter" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 2, "Millimeter", 0.0393701f, 1, "Inch" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Ounce", 29.5735f, "Milliliter" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 2, "Milliliter", 0.0338f, 1, "Ounce" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "BaseUnit", "Quantity", "TargetUnit" },
                values: new object[] { "Ounce", 0.02835f, "Kilogram" });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[] { 2, "Kilogram", 35.274f, 1, "Ounce" });
        }
    }
}
