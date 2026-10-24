using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class UnitIngredientAddNewIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "IngredientUnits",
                columns: new[] { "Id", "Ingredient", "IsLiquid", "LargeImperialUnit", "MetricUnit", "Pass", "SmallImperialUnit" },
                values: new object[,]
                {
                    { 224, "ground pepper", false, "tbsp", "g", 1, "tsp" },
                    { 225, "crushed pepper", false, "tbsp", "g", 1, "tsp" },
                    { 226, "garlic powder", false, "tbsp", "g", 1, "tsp" },
                    { 227, "ginger powder", false, "tbsp", "g", 1, "tsp" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 227);
        }
    }
}
