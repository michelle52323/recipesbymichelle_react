using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryColumnsToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategorySortBy",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<bool>(
                name: "ShowCategories",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategorySortBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ShowCategories",
                table: "Users");
        }
    }
}
