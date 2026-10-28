using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class UnitConversionsAddConversionsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UnitConversion",
                columns: new[] { "Id", "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[,]
                {
                    { 25, 1, "Ounce", 0.02835f, 2, "Kilogram" },
                    { 26, 2, "Kilogram", 35.274f, 1, "Ounce" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 26);
        }
    }
}
