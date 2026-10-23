using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class UnitPrecisionTableAddColumnConversionFactorAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ConversionFactor",
                table: "UnitPrecisionRules",
                type: "real",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConversionFactor",
                value: 0.001f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConversionFactor",
                value: 1000f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConversionFactor",
                value: 0.001f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConversionFactor",
                value: 1000f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConversionFactor",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConversionFactor",
                value: null);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 7,
                column: "ConversionFactor",
                value: 0.33333334f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 8,
                column: "ConversionFactor",
                value: 3f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 9,
                column: "ConversionFactor",
                value: 0.0625f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 10,
                column: "ConversionFactor",
                value: 16f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 11,
                column: "ConversionFactor",
                value: 0.25f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 12,
                column: "ConversionFactor",
                value: 2f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 13,
                column: "ConversionFactor",
                value: 0.125f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 14,
                column: "ConversionFactor",
                value: 2f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 15,
                column: "ConversionFactor",
                value: 0.5f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 16,
                column: "ConversionFactor",
                value: 2f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 17,
                column: "ConversionFactor",
                value: 0.25f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 18,
                column: "ConversionFactor",
                value: 4f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 19,
                column: "ConversionFactor",
                value: 0.0625f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 20,
                column: "ConversionFactor",
                value: 16f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 21,
                column: "ConversionFactor",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversionFactor",
                table: "UnitPrecisionRules");
        }
    }
}
