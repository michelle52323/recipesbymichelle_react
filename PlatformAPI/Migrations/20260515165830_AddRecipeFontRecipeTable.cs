using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeFontRecipeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "RecipeFont",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasSelectedMeasurementSystem",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsLegacyPassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LegacyPassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MeasurementSystem",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RecipeFont",
                table: "Recipes");
        }
    }
}
