using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class UnitConversionSeedingDatModificationsBeans2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 315,
                column: "Quantity",
                value: 24.25f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 316,
                column: "Quantity",
                value: 0.041237f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 323,
                column: "Quantity",
                value: 24.12f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 324,
                column: "Quantity",
                value: 0.041451f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 331,
                column: "Quantity",
                value: 23f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 332,
                column: "Quantity",
                value: 0.043478f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 339,
                column: "Quantity",
                value: 22.75f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 340,
                column: "Quantity",
                value: 0.043956f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 347,
                column: "Quantity",
                value: 23.75f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 348,
                column: "Quantity",
                value: 0.042105f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 315,
                column: "Quantity",
                value: 12.13f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 316,
                column: "Quantity",
                value: 0.08244f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 323,
                column: "Quantity",
                value: 12.06f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 324,
                column: "Quantity",
                value: 0.082919f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 331,
                column: "Quantity",
                value: 11.5f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 332,
                column: "Quantity",
                value: 0.086957f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 339,
                column: "Quantity",
                value: 11.38f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 340,
                column: "Quantity",
                value: 0.087873f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 347,
                column: "Quantity",
                value: 11.88f);

            migrationBuilder.UpdateData(
                table: "UnitConversionExceptions",
                keyColumn: "Id",
                keyValue: 348,
                column: "Quantity",
                value: 0.084175f);
        }
    }
}
