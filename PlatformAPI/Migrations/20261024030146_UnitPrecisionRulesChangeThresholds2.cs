using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class UnitPrecisionRulesChangeThresholds2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 9,
                column: "Threshold",
                value: 15.875f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 10,
                column: "Threshold",
                value: 0.9921f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "ConversionFactor", "Threshold" },
                values: new object[] { 2.4019f, 0.972f });

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "ConversionFactor", "Threshold" },
                values: new object[] { 0.6f, 1.556f });

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "ConversionFactor", "Threshold" },
                values: new object[] { 1.66f, 1.124f });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 9,
                column: "Threshold",
                value: 3.875f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 10,
                column: "Threshold",
                value: 0.968f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "ConversionFactor", "Threshold" },
                values: new object[] { 2f, 0.984f });

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "ConversionFactor", "Threshold" },
                values: new object[] { 0.5f, 1.875f });

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "ConversionFactor", "Threshold" },
                values: new object[] { 2f, 0.937f });
        }
    }
}
