using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class UnitConversionTableSeedAddOunceMilliliterConversions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UnitConversion",
                columns: new[] { "Id", "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[,]
                {
                    { 23, 1, "Ounce", 29.5735f, 2, "Milliliter" },
                    { 24, 2, "Milliliter", 0.0338f, 1, "Ounce" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 24);
        }
    }
}
