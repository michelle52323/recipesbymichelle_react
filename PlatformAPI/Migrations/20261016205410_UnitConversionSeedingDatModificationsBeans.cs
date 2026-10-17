using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class UnitConversionSeedingDatModificationsBeans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 58,
                column: "SmallImperialUnit",
                value: "oz");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 59,
                column: "SmallImperialUnit",
                value: "oz");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 60,
                column: "SmallImperialUnit",
                value: "oz");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 61,
                column: "SmallImperialUnit",
                value: "oz");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 62,
                column: "SmallImperialUnit",
                value: "oz");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 63,
                column: "SmallImperialUnit",
                value: "oz");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 315,
                column: "BaseUnit",
                value: "Ounce");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 316,
                column: "TargetUnit",
                value: "Ounce");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 323,
                column: "BaseUnit",
                value: "Ounce");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 324,
                column: "TargetUnit",
                value: "Ounce");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 331,
                column: "BaseUnit",
                value: "Ounce");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 332,
                column: "TargetUnit",
                value: "Ounce");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 339,
                column: "BaseUnit",
                value: "Ounce");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 340,
                column: "TargetUnit",
                value: "Ounce");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 347,
                column: "BaseUnit",
                value: "Ounce");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 348,
                column: "TargetUnit",
                value: "Ounce");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 58,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 59,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 60,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 61,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 62,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 63,
                column: "SmallImperialUnit",
                value: "tbsp");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 315,
                column: "BaseUnit",
                value: "Tablespoon");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 316,
                column: "TargetUnit",
                value: "Tablespoon");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 323,
                column: "BaseUnit",
                value: "Tablespoon");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 324,
                column: "TargetUnit",
                value: "Tablespoon");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 331,
                column: "BaseUnit",
                value: "Tablespoon");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 332,
                column: "TargetUnit",
                value: "Tablespoon");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 339,
                column: "BaseUnit",
                value: "Tablespoon");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 340,
                column: "TargetUnit",
                value: "Tablespoon");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 347,
                column: "BaseUnit",
                value: "Tablespoon");

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 348,
                column: "TargetUnit",
                value: "Tablespoon");
        }
    }
}
