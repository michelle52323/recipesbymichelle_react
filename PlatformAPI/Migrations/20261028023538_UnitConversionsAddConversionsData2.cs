using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class UnitConversionsAddConversionsData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UnitConversionExceptions",
                columns: new[] { "Id", "BaseSystem", "BaseUnit", "Ingredient", "Quantity", "TargetSystem", "TargetUnit" },
                values: new object[,]
                {
                    { 352, 1, "Cup", "ground pepper", 101f, 2, "Gram" },
                    { 353, 2, "Gram", "ground pepper", 0.009901f, 1, "Cup" },
                    { 354, 1, "Tablespoon", "ground pepper", 6.31f, 2, "Gram" },
                    { 355, 2, "Gram", "ground pepper", 0.158479f, 1, "Tablespoon" },
                    { 356, 1, "Teaspoon", "ground pepper", 2.1f, 2, "Gram" },
                    { 357, 2, "Gram", "ground pepper", 0.47619f, 1, "Teaspoon" },
                    { 358, 2, "Milliliter", "ground pepper", 0.43f, 2, "Gram" },
                    { 359, 2, "Gram", "ground pepper", 2.325581f, 2, "Milliliter" },
                    { 360, 1, "Cup", "crushed pepper", 96f, 2, "Gram" },
                    { 361, 2, "Gram", "crushed pepper", 0.010417f, 1, "Cup" },
                    { 362, 1, "Tablespoon", "crushed pepper", 6f, 2, "Gram" },
                    { 363, 2, "Gram", "crushed pepper", 0.166667f, 1, "Tablespoon" },
                    { 364, 1, "Teaspoon", "crushed pepper", 2f, 2, "Gram" },
                    { 365, 2, "Gram", "crushed pepper", 0.5f, 1, "Teaspoon" },
                    { 366, 2, "Milliliter", "crushed pepper", 0.41f, 2, "Gram" },
                    { 367, 2, "Gram", "crushed pepper", 2.439024f, 2, "Milliliter" },
                    { 368, 1, "Cup", "garlic powder", 128f, 2, "Gram" },
                    { 369, 2, "Gram", "garlic powder", 0.007813f, 1, "Cup" },
                    { 370, 1, "Tablespoon", "garlic powder", 8f, 2, "Gram" },
                    { 371, 2, "Gram", "garlic powder", 0.125f, 1, "Tablespoon" },
                    { 372, 1, "Teaspoon", "garlic powder", 2.67f, 2, "Gram" },
                    { 373, 2, "Gram", "garlic powder", 0.374531f, 1, "Teaspoon" },
                    { 374, 2, "Milliliter", "garlic powder", 0.54f, 2, "Gram" },
                    { 375, 2, "Gram", "garlic powder", 1.851852f, 2, "Milliliter" },
                    { 376, 1, "Cup", "ginger powder", 96f, 2, "Gram" },
                    { 377, 2, "Gram", "ginger powder", 0.010417f, 1, "Cup" },
                    { 378, 1, "Tablespoon", "ginger powder", 6f, 2, "Gram" },
                    { 379, 2, "Gram", "ginger powder", 0.166667f, 1, "Tablespoon" },
                    { 380, 1, "Teaspoon", "ginger powder", 2f, 2, "Gram" },
                    { 381, 2, "Gram", "ginger powder", 0.5f, 1, "Teaspoon" },
                    { 382, 2, "Milliliter", "ginger powder", 0.41f, 2, "Gram" },
                    { 383, 2, "Gram", "ginger powder", 2.439024f, 2, "Milliliter" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 352);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 353);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 354);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 355);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 356);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 357);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 358);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 359);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 360);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 361);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 362);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 363);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 364);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 365);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 366);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 367);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 368);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 369);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 370);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 371);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 372);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 373);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 374);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 375);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 376);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 377);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 378);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 379);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 380);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 381);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 382);

            migrationBuilder.DeleteData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 383);
        }
    }
}
