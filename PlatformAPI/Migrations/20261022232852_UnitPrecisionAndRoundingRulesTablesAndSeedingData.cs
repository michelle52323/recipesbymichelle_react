using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class UnitPrecisionAndRoundingRulesTablesAndSeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImperialRoundingRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lower = table.Column<float>(type: "real", nullable: false),
                    Upper = table.Column<float>(type: "real", nullable: false),
                    MapToFraction = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayOutput = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImperialRoundingRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitPrecisionRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Comparison = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Threshold = table.Column<float>(type: "real", nullable: true),
                    NewUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Condition = table.Column<int>(type: "int", nullable: true),
                    RoundToConditional = table.Column<float>(type: "real", nullable: true),
                    RoundToElse = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitPrecisionRules", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ImperialRoundingRules",
                columns: new[] { "Id", "DisplayOutput", "Lower", "MapToFraction", "Upper" },
                values: new object[,]
                {
                    { 1, "(blank)", 0f, "0", 0.06f },
                    { 2, "1/8", 0.06f, "1/8", 0.19f },
                    { 3, "1/4", 0.19f, "1/4", 0.29f },
                    { 4, "1/3", 0.29f, "1/3", 0.35f },
                    { 5, "3/8", 0.35f, "3/8", 0.44f },
                    { 6, "1/2", 0.44f, "1/2", 0.56f },
                    { 7, "5/8", 0.56f, "5/8", 0.65f },
                    { 8, "2/3", 0.65f, "2/3", 0.71f },
                    { 9, "3/4", 0.71f, "3/4", 0.88f },
                    { 10, "7/8", 0.88f, "7/8", 0.94f },
                    { 11, "(add 1 to whole number)", 0.94f, "1", 1f }
                });

            migrationBuilder.InsertData(
                table: "UnitPrecisionRules",
                columns: new[] { "Id", "Comparison", "Condition", "NewUnit", "RoundToConditional", "RoundToElse", "Threshold", "Unit" },
                values: new object[,]
                {
                    { 1, ">=", 100, "kg", 1f, 10f, 1000f, "g" },
                    { 2, "<", null, "g", null, 0.05f, 1f, "kg" },
                    { 3, ">=", 100, "l", 1f, 10f, 1000f, "ml" },
                    { 4, "<", null, "ml", null, 0.05f, 1f, "l" },
                    { 5, null, null, null, null, 0.5f, null, "cm" },
                    { 6, null, null, null, null, 1f, null, "mm" },
                    { 7, ">=", null, "Tbsp", null, null, 3f, "tsp" },
                    { 8, "<", null, "tsp", null, null, 1f, "Tbsp" },
                    { 9, ">=", null, "cup", null, null, 4f, "Tbsp" },
                    { 10, "<", null, "Tbsp", null, null, 0.25f, "cup" },
                    { 11, ">=", null, "qt", null, null, 4f, "cup" },
                    { 12, "<", null, "Tbsp", null, null, 1f, "fl oz" },
                    { 13, ">=", null, "cup", null, null, 8f, "fl oz" },
                    { 14, "<", null, "cup", null, null, 1f, "pt" },
                    { 15, ">=", null, "qt", null, null, 2f, "pt" },
                    { 16, "<", null, "pt", null, null, 1f, "qt" },
                    { 17, ">=", null, "gal", null, null, 4f, "qt" },
                    { 18, "<", null, "qt", null, null, 1f, "gal" },
                    { 19, ">=", null, "lb", null, null, 16f, "oz" },
                    { 20, "<", null, "oz", null, null, 1f, "lb" },
                    { 21, null, null, null, null, null, null, "inch" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImperialRoundingRules");

            migrationBuilder.DropTable(
                name: "UnitPrecisionRules");
        }
    }
}
