using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class UnitConversionAddSeedSoup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "IngredientUnits",
                columns: new[] { "Id", "Ingredient", "IsLiquid", "LargeImperialUnit", "MetricUnit", "Pass", "SmallImperialUnit" },
                values: new object[] { 223, "soup", true, "cup", "ml", 1, "tbsp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 223);
        }
    }
}
