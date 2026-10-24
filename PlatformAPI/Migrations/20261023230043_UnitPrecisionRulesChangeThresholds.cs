using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class UnitPrecisionRulesChangeThresholds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 7,
                column: "Threshold",
                value: 2.875f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 8,
                column: "Threshold",
                value: 0.958f);

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
                keyValue: 11,
                column: "Threshold",
                value: 3.875f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 12,
                column: "Threshold",
                value: 0.968f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 13,
                column: "Threshold",
                value: 7.875f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 14,
                column: "Threshold",
                value: 0.984f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 15,
                column: "Threshold",
                value: 1.875f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 16,
                column: "Threshold",
                value: 0.937f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 17,
                column: "Threshold",
                value: 3.875f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 18,
                column: "Threshold",
                value: 0.968f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 19,
                column: "Threshold",
                value: 15.875f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 20,
                column: "Threshold",
                value: 0.9921f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 7,
                column: "Threshold",
                value: 3f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 8,
                column: "Threshold",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 9,
                column: "Threshold",
                value: 4f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 10,
                column: "Threshold",
                value: 0.25f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 11,
                column: "Threshold",
                value: 4f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 12,
                column: "Threshold",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 13,
                column: "Threshold",
                value: 8f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 14,
                column: "Threshold",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 15,
                column: "Threshold",
                value: 2f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 16,
                column: "Threshold",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 17,
                column: "Threshold",
                value: 4f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 18,
                column: "Threshold",
                value: 1f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 19,
                column: "Threshold",
                value: 16f);

            migrationBuilder.UpdateData(
                table: "UnitPrecisionRules",
                keyColumn: "Id",
                keyValue: 20,
                column: "Threshold",
                value: 1f);
        }
    }
}
