using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddBunchInchAndMillimeterToUnitsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Description", "Plural", "System" },
                values: new object[] { "bunch", "bunches", null });

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Abbreviation", "Description", "Plural", "System" },
                values: new object[] { null, "inch", "inches", 1 });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "Abbreviation", "Description", "Plural", "System" },
                values: new object[] { 26, "mm", "millimeter", "millimeters", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Description", "Plural", "System" },
                values: new object[] { "inch", "inches", 1 });

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Abbreviation", "Description", "Plural", "System" },
                values: new object[] { "mm", "millimeter", "millimeters", 2 });
        }
    }
}
