using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddInchAndMillimeterToUnitsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Description", "Plural" },
                values: new object[] { "milliliter", "milliliters" });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "Abbreviation", "Description", "Plural", "System" },
                values: new object[,]
                {
                    { 24, null, "inch", "inches", 1 },
                    { 25, "mm", "millimeter", "millimeters", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Description", "Plural" },
                values: new object[] { "millileter", "millileters" });
        }
    }
}
