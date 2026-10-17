using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class UnitConversionAddColumnIsLiquidAndSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLiquid",
                table: "IngredientUnits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 8,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 9,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 10,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 11,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 12,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 13,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 14,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 15,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 16,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 17,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 18,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 19,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 20,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 21,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 22,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 23,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 24,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 25,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 26,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 27,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 28,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 29,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 30,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 31,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 32,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 33,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 34,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 35,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 36,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 37,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 38,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 39,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 40,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 41,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 42,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 43,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 44,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 45,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 46,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 47,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 48,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 49,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 50,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 51,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 52,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 53,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 54,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 55,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 56,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 57,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 58,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 59,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 60,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 61,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 62,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 63,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 64,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 65,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 66,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 67,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 68,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 69,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 70,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 71,
                column: "IsLiquid",
                value: true);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 72,
                column: "IsLiquid",
                value: true);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 73,
                column: "IsLiquid",
                value: true);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 74,
                column: "IsLiquid",
                value: true);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 75,
                column: "IsLiquid",
                value: true);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 76,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 77,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 78,
                column: "IsLiquid",
                value: true);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 79,
                column: "IsLiquid",
                value: true);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 80,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 81,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 82,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 83,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 84,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 85,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 86,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 87,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 88,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 89,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 90,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 91,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 92,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 93,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 94,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 95,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 96,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 97,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 98,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 99,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 100,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 101,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 102,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 103,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 104,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 105,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 106,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 107,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 108,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 109,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 110,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 111,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 112,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 113,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 114,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 115,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 116,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 117,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 118,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 119,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 120,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 121,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 122,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 123,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 124,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 125,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 126,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 127,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 128,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 129,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 130,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 131,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 132,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 133,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 134,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 135,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 136,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 137,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 138,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 139,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 140,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 141,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 142,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 143,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 144,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 145,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 146,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 147,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 148,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 149,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 150,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 151,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 152,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 153,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 154,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 155,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 156,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 157,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 158,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 159,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 160,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 161,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 162,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 163,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 164,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 165,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 166,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 167,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 168,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 169,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 170,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 171,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 172,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 173,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 174,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 175,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 176,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 177,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 178,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 179,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 180,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 181,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 182,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 183,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 184,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 185,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 186,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 187,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 188,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 189,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 190,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 191,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 192,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 193,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 194,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 195,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 196,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 197,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 198,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 199,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 200,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 201,
                column: "IsLiquid",
                value: true);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 202,
                column: "IsLiquid",
                value: true);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 203,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 204,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 205,
                column: "IsLiquid",
                value: true);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 206,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 207,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 208,
                column: "IsLiquid",
                value: true);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 209,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 210,
                column: "IsLiquid",
                value: true);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 211,
                column: "IsLiquid",
                value: true);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 212,
                column: "IsLiquid",
                value: true);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 213,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 214,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 215,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 216,
                column: "IsLiquid",
                value: false);

            migrationBuilder.UpdateData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 217,
                column: "IsLiquid",
                value: false);

            migrationBuilder.InsertData(
                table: "IngredientUnits",
                columns: new[] { "Id", "Ingredient", "IsLiquid", "LargeImperialUnit", "MetricUnit", "Pass", "SmallImperialUnit" },
                values: new object[,]
                {
                    { 218, "syrup", false, "cup", "ml", 2, "tbsp" },
                    { 219, "oil", true, "cup", "ml", 2, "tbsp" },
                    { 220, "sauce", true, "cup", "ml", 2, "tbsp" },
                    { 221, "puree", true, "cup", "ml", 2, "tbsp" },
                    { 222, "butter", true, "cup", "ml", 2, "tbsp" }
                });

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 8,
                column: "TargetUnit",
                value: "Teaspoon");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "IngredientUnits",
                keyColumn: "Id",
                keyValue: 222);

            migrationBuilder.DropColumn(
                name: "IsLiquid",
                table: "IngredientUnits");

            migrationBuilder.UpdateData(
                table: "UnitConversion",
                keyColumn: "Id",
                keyValue: 8,
                column: "TargetUnit",
                value: "Fluid Ounce");
        }
    }
}
