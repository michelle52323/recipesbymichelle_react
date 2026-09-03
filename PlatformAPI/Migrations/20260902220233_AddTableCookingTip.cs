using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTableCookingTip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CookingTips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Season = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CookingTips", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeCategories_CategoryId",
                table: "RecipeCategories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeCategories_Categories_CategoryId",
                table: "RecipeCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeCategories_Recipes_RecipeId",
                table: "RecipeCategories",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeCategories_Categories_CategoryId",
                table: "RecipeCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeCategories_Recipes_RecipeId",
                table: "RecipeCategories");

            migrationBuilder.DropTable(
                name: "CookingTips");

            migrationBuilder.DropIndex(
                name: "IX_RecipeCategories_CategoryId",
                table: "RecipeCategories");
        }
    }
}
