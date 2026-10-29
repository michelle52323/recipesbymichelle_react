using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class UnitConversionsUpdateConversionsData_2_20261028 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UnitConversion",
                columns: new[] { "Id", "BaseSystem", "BaseUnit", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[,]
                {
                    { 27, 1, "Pound", 453.592f, 2, "Gram" },
                    { 28, 1, "Pound", 755.987f, 2, "Milliliter" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 28);
        }
    }
}
