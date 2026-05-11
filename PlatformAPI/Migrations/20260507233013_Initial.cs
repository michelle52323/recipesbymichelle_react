using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForgotPasswordRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Updated = table.Column<bool>(type: "bit", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForgotPasswordRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FractionDecimals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fraction = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Decimal = table.Column<float>(type: "real", nullable: true),
                    Primary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FractionDecimals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<float>(type: "real", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Instructions = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 50, nullable: true),
                    ShowAbbreviations = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    RecipeVisibility = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Steps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 50, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Steps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Themes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThemeVariables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeVariables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Abbreviation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    System = table.Column<int>(type: "int", nullable: true),
                    Plural = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredients",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredients", x => new { x.RecipeId, x.IngredientId });
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRecipes",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRecipes", x => new { x.UserId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_UserRecipes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeSteps",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    StepId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeSteps", x => new { x.RecipeId, x.StepId });
                    table.ForeignKey(
                        name: "FK_RecipeSteps_Steps_StepId",
                        column: x => x.StepId,
                        principalTable: "Steps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThemeVariablesColors",
                columns: table => new
                {
                    ThemeId = table.Column<int>(type: "int", nullable: false),
                    ThemeVariableId = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeVariablesColors", x => new { x.ThemeId, x.ThemeVariableId });
                    table.ForeignKey(
                        name: "FK_ThemeVariablesColors_ThemeVariables_ThemeVariableId",
                        column: x => x.ThemeVariableId,
                        principalTable: "ThemeVariables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Pronouns = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: true),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    ThemeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_UserTypes_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsMine = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoginAttempts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IPAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WasSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginAttempts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "FractionDecimals",
                columns: new[] { "Id", "Decimal", "Fraction", "Primary" },
                values: new object[,]
                {
                    { 1, 0.0625f, "1/16", true },
                    { 2, 0.125f, "1/8", true },
                    { 3, 0.1875f, "3/16", true },
                    { 4, 0.25f, "1/4", true },
                    { 5, 0.3125f, "5/16", true },
                    { 6, 0.375f, "3/8", true },
                    { 7, 0.4375f, "7/16", true },
                    { 8, 0.5f, "1/2", true },
                    { 9, 0.5625f, "9/16", true },
                    { 10, 0.625f, "5/8", true },
                    { 11, 0.6875f, "11/16", true },
                    { 12, 0.75f, "3/4", true },
                    { 13, 0.8125f, "13/16", true },
                    { 14, 0.875f, "7/8", true },
                    { 15, 0.9375f, "15/16", true },
                    { 16, 0.25f, "4/16", false },
                    { 17, 0.125f, "2/16", false },
                    { 18, 0.375f, "6/16", false },
                    { 19, 0.5f, "8/16", false },
                    { 20, 0.625f, "10/16", false },
                    { 21, 0.75f, "12/16", false },
                    { 22, 0.875f, "14/16", false },
                    { 23, 0.25f, "2/8", false },
                    { 24, 0.5f, "4/8", false },
                    { 25, 0.75f, "6/8", false },
                    { 26, 0.5f, "2/4", false },
                    { 27, 0.333f, "1/3", true },
                    { 28, 0.667f, "2/3", true },
                    { 29, 0.2f, "1/5", true },
                    { 30, 0.4f, "2/5", true },
                    { 31, 0.6f, "3/5", true },
                    { 32, 0.8f, "4/5", true }
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Code", "Description" },
                values: new object[,]
                {
                    { 1, "F", "Female" },
                    { 2, "M", "Male" },
                    { 3, "TG", "Transgender" },
                    { 4, "NB", "Non-binary" },
                    { 5, "X", "Other" }
                });

            migrationBuilder.InsertData(
                table: "ThemeVariables",
                columns: new[] { "Id", "Description", "GroupId", "SortOrder" },
                values: new object[,]
                {
                    { 1, "dropdownBackColor", 1, 1 },
                    { 2, "dropdownTextColor", 1, 3 },
                    { 3, "dropdownHoverBackColor", 1, 4 },
                    { 4, "dropdownHoverTextColor", 1, 6 },
                    { 5, "dropdownContentBackgroundColor", 1, 7 },
                    { 6, "dropdownContentTextColor", 1, 9 },
                    { 7, "textBoxLabelColor", 2, 1 },
                    { 8, "buttonBackgroundColor", 3, 1 },
                    { 9, "buttonTextColor", 3, 3 },
                    { 10, "buttonHoverBackgroundColor", 3, 4 },
                    { 11, "buttonHoverTextColor", 3, 6 },
                    { 12, "contentBackColor", 4, 1 },
                    { 13, "textBoxTextColor", 2, 2 },
                    { 14, "textBoxBackColor", 2, 3 },
                    { 15, "outsideBackColor", 5, 1 },
                    { 16, "textBoxBorderColor", 2, 5 },
                    { 17, "dropdownBackColorGradient", 1, 2 },
                    { 18, "dropdownHoverBackColorGradient", 1, 5 },
                    { 19, "dropdownContentBackgroundColorGradient", 1, 8 },
                    { 20, "buttonBackgroundColorGradient", 3, 2 },
                    { 21, "buttonHoverBackgroundColorGradient", 3, 5 },
                    { 22, "contentBackColorGradient", 4, 2 },
                    { 23, "textBoxBackColorGradient", 2, 4 },
                    { 24, "outsideBackColorGradient", 5, 2 },
                    { 25, "contentHeaderBackColor", 7, 1 },
                    { 26, "contentHeaderBackColorGradient", 7, 2 },
                    { 27, "contentHeaderTextColor", 7, 3 },
                    { 28, "contentTextColor", 7, 4 },
                    { 29, "linkTextColor", 7, 5 },
                    { 30, "radioLabelColor", 8, 1 },
                    { 31, "radioHoverColor", 8, 2 },
                    { 32, "radioCheckColor", 8, 3 },
                    { 33, "dialogHeaderBackColor", 9, 1 },
                    { 34, "dialogHeaderBackColorGradient", 9, 2 },
                    { 35, "dialogHeaderTextColor", 9, 3 },
                    { 36, "dialogContentBackColor", 9, 4 },
                    { 37, "dialogContentBackColorGradient", 9, 5 },
                    { 38, "dialogContentTextColor", 9, 6 },
                    { 39, "sortableBorder", 10, 1 },
                    { 40, "sortableHighlightBackColor", 10, 2 },
                    { 41, "sortableHighlightBackColorGradient", 10, 3 },
                    { 42, "textBoxBorderColorSelected", 2, 6 },
                    { 43, "passwordIconColor", 2, 7 }
                });

            migrationBuilder.InsertData(
                table: "Themes",
                columns: new[] { "Id", "Description", "IsActive", "SortOrder" },
                values: new object[,]
                {
                    { 1, "Light Blue", true, 1 },
                    { 2, "Dark", false, 2 },
                    { 3, "Dark Teal", true, 3 },
                    { 4, "Light Green", true, 4 },
                    { 5, "Dark Green", true, 5 }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "Abbreviation", "Description", "Plural", "System" },
                values: new object[,]
                {
                    { 1, "lb", "pound", "pounds", 1 },
                    { 2, "oz", "ounce", "ounces", 1 },
                    { 3, "gal", "gallon", "gallons", 1 },
                    { 4, "qt", "quart", "quarts", 1 },
                    { 5, "pt", "pint", "pints", 1 },
                    { 7, "fl oz", "fluid ounce", "fluid ounces", 1 },
                    { 8, "Tbsp", "tablespoon", "tablespoons", 1 },
                    { 9, "tsp", "teaspoon", "teaspoons", 1 },
                    { 10, null, "can", "cans", null },
                    { 11, null, "cup", "cups", 1 },
                    { 12, "ml", "millileter", "millileters", 2 },
                    { 13, "L", "liter", "liters", 2 },
                    { 14, "g", "gram", "grams", 2 },
                    { 15, "kg", "kilogram", "kilograms", 2 },
                    { 16, "cm", "centimeter", "centimeters", 2 },
                    { 17, null, "pinch", "pinch", null },
                    { 18, null, "clove", "cloves", null },
                    { 19, null, "piece", "pieces", null },
                    { 20, null, "stick", "sticks", null },
                    { 21, null, "drop", "drops", null },
                    { 22, null, "slice", "slices", null },
                    { 23, null, "dash", "dash", null }
                });

            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "Id", "Code", "Description" },
                values: new object[,]
                {
                    { 1, "SA", "Super Administrator" },
                    { 2, "A", "Administrator" },
                    { 3, "E", "Educator" },
                    { 4, "S", "Standard User" }
                });

            migrationBuilder.InsertData(
                table: "ThemeVariablesColors",
                columns: new[] { "ThemeId", "ThemeVariableId", "Color" },
                values: new object[,]
                {
                    { 1, 1, "#0058f2" },
                    { 1, 2, "#ffffff" },
                    { 1, 3, "#0030a0" },
                    { 1, 4, "#FFFFFF" },
                    { 1, 5, "#0058f2" },
                    { 1, 6, "#FFFFFF" },
                    { 1, 7, "#0048d1" },
                    { 1, 8, "#0058f2" },
                    { 1, 9, "#ffffff" },
                    { 1, 10, "#0030a0" },
                    { 1, 11, "#ffffff" },
                    { 1, 12, "#ffffff" },
                    { 1, 13, "#000000" },
                    { 1, 14, "#A0C2FF" },
                    { 1, 15, "#f3f5f7" },
                    { 1, 16, "#0000ee" },
                    { 1, 17, "#3178dd" },
                    { 1, 18, "#0042b8" },
                    { 1, 19, "#3178dd" },
                    { 1, 20, "#3178dd" },
                    { 1, 21, "#0042b8" },
                    { 1, 22, "#efefef" },
                    { 1, 23, "#78AFFF" },
                    { 1, 24, "#e3e5e7" },
                    { 1, 25, "#0054ff" },
                    { 1, 26, "#1074ef" },
                    { 1, 27, "#ffffff" },
                    { 1, 28, "#0038a1" },
                    { 1, 29, "#0038a1" },
                    { 1, 30, "#0048d1" },
                    { 1, 31, "#003cb5" },
                    { 1, 32, "#0048d1" },
                    { 1, 33, "#0054ff" },
                    { 1, 34, "#1074ef" },
                    { 1, 35, "#ffffff" },
                    { 1, 36, "#c3c5c7" },
                    { 1, 37, "#b3b5b7" },
                    { 1, 38, "#00143f" },
                    { 1, 39, "#0048d1" },
                    { 1, 40, "#0048d1" },
                    { 1, 41, "#0058f2" },
                    { 1, 42, "#101010" },
                    { 1, 43, "#000000" },
                    { 2, 1, "#131210" },
                    { 2, 2, "#ffffff" },
                    { 2, 3, "#050302" },
                    { 2, 4, "#ffffff" },
                    { 2, 5, "#1f1e1c" },
                    { 2, 6, "#ffffff" },
                    { 2, 7, "#ffffff" },
                    { 2, 8, "#131210" },
                    { 2, 9, "#ffffff" },
                    { 2, 10, "#050302" },
                    { 2, 11, "#ffffff" },
                    { 2, 12, "#343434" },
                    { 2, 13, "#ffffff" },
                    { 2, 14, "#141414" },
                    { 2, 15, "#0c0a08" },
                    { 2, 16, "#18b5ea" },
                    { 2, 17, "#232220" },
                    { 2, 18, "#151312" },
                    { 2, 19, "#2f2e2c" },
                    { 2, 20, "#232220" },
                    { 2, 21, "#151312" },
                    { 2, 22, "#444444" },
                    { 2, 23, "#242424" },
                    { 2, 24, "#1c1a18" },
                    { 2, 25, "#131210" },
                    { 2, 26, "#232220" },
                    { 2, 27, "#ffffff" },
                    { 2, 28, "#ffffff" },
                    { 2, 29, "#ffffff" },
                    { 2, 30, "#ffffff" },
                    { 2, 31, "#bbbbbb" },
                    { 2, 32, "#ffffff" },
                    { 2, 33, "#131210" },
                    { 2, 34, "#232220" },
                    { 2, 35, "#ffffff" },
                    { 2, 36, "#acaaa8" },
                    { 2, 37, "#bcbab8" },
                    { 2, 38, "#000000" },
                    { 2, 39, "#ffffff" },
                    { 2, 40, "#ffffff" },
                    { 2, 41, "#dfdfdf" },
                    { 2, 42, "#FFFFFF" },
                    { 2, 43, "#ffffff" },
                    { 3, 1, "#0B5257" },
                    { 3, 2, "#ffffff" },
                    { 3, 3, "#0E3733" },
                    { 3, 4, "#ffffff" },
                    { 3, 5, "#0B5257" },
                    { 3, 6, "#ffffff" },
                    { 3, 7, "#00B0A0" },
                    { 3, 8, "#0B5257" },
                    { 3, 9, "#ffffff" },
                    { 3, 10, "#0E2723" },
                    { 3, 11, "#ffffff" },
                    { 3, 12, "#000000" },
                    { 3, 13, "#00B0A0" },
                    { 3, 14, "#0B4A4F" },
                    { 3, 15, "#0C0A08" },
                    { 3, 16, "#18b5ea" },
                    { 3, 17, "#123D3A" },
                    { 3, 18, "#0E3733" },
                    { 3, 19, "#1A3F3A" },
                    { 3, 20, "#1A3F3A" },
                    { 3, 21, "#0E3733" },
                    { 3, 22, "#101010" },
                    { 3, 23, "#242424" },
                    { 3, 24, "#1C1A18" },
                    { 3, 25, "#0B5257" },
                    { 3, 26, "#1A3F3A" },
                    { 3, 27, "#00B8A8" },
                    { 3, 28, "#00B0A0" },
                    { 3, 29, "#00B0A0" },
                    { 3, 30, "#00B0A0" },
                    { 3, 31, "#006D5B" },
                    { 3, 32, "#00B0A0" },
                    { 3, 33, "#0B5257" },
                    { 3, 34, "#1A3F3A" },
                    { 3, 35, "#00B8A8" },
                    { 3, 36, "#acaaa8" },
                    { 3, 37, "#bcbab8" },
                    { 3, 38, "#001205" },
                    { 3, 39, "#008080" },
                    { 3, 40, "#008080" },
                    { 3, 41, "#20a295" },
                    { 3, 42, "#FFFFFF" },
                    { 3, 43, "#00B0A0" },
                    { 4, 1, "#009837" },
                    { 4, 2, "#ffffff" },
                    { 4, 3, "#115543" },
                    { 4, 4, "#ffffff" },
                    { 4, 5, "#00751F" },
                    { 4, 6, "#ffffff" },
                    { 4, 7, "#009837" },
                    { 4, 8, "#009837" },
                    { 4, 9, "#ffffff" },
                    { 4, 10, "#115543" },
                    { 4, 11, "#ffffff" },
                    { 4, 12, "#f7fffe" },
                    { 4, 13, "#00321A" },
                    { 4, 14, "#31C74D" },
                    { 4, 15, "#fafefd" },
                    { 4, 16, "#008000" },
                    { 4, 17, "#10a847" },
                    { 4, 18, "#107847" },
                    { 4, 19, "#10a847" },
                    { 4, 20, "#10a847" },
                    { 4, 21, "#107847" },
                    { 4, 22, "#e7efee" },
                    { 4, 23, "#37B74D" },
                    { 4, 24, "#eaeeed" },
                    { 4, 25, "#009837" },
                    { 4, 26, "#10a847" },
                    { 4, 27, "#ffffff" },
                    { 4, 28, "#006327" },
                    { 4, 29, "#006327" },
                    { 4, 30, "#009837" },
                    { 4, 31, "#006235" },
                    { 4, 32, "#009837" },
                    { 4, 33, "#009837" },
                    { 4, 34, "#10a847" },
                    { 4, 35, "#ffffff" },
                    { 4, 36, "#cacecd" },
                    { 4, 37, "#babebd" },
                    { 4, 38, "#003225" },
                    { 4, 39, "#009837" },
                    { 4, 40, "#009837" },
                    { 4, 41, "#208255" },
                    { 4, 42, "#101010" },
                    { 4, 43, "#00321A" },
                    { 5, 1, "#0e7733" },
                    { 5, 2, "#ffffff" },
                    { 5, 3, "#073411" },
                    { 5, 4, "#ffffff" },
                    { 5, 5, "#0e7733" },
                    { 5, 6, "#ffffff" },
                    { 5, 7, "#00BF73" },
                    { 5, 8, "#0e7733" },
                    { 5, 9, "#ffffff" },
                    { 5, 10, "#073411" },
                    { 5, 11, "#ffffff" },
                    { 5, 12, "#000000" },
                    { 5, 13, "#00BF73" },
                    { 5, 14, "#0C4D40" },
                    { 5, 15, "#0c0a08" },
                    { 5, 16, "#18aa45" },
                    { 5, 17, "#184615" },
                    { 5, 18, "#0F3510" },
                    { 5, 19, "#184615" },
                    { 5, 20, "#184615" },
                    { 5, 21, "#0F3510" },
                    { 5, 22, "#101010" },
                    { 5, 23, "#242424" },
                    { 5, 24, "#1c1a18" },
                    { 5, 25, "#0e7733" },
                    { 5, 26, "#184615" },
                    { 5, 27, "#002225" },
                    { 5, 28, "#00BF73" },
                    { 5, 29, "#00BF73" },
                    { 5, 30, "#00BF73" },
                    { 5, 31, "#007235" },
                    { 5, 32, "#00BF73" },
                    { 5, 33, "#0e7733" },
                    { 5, 34, "#184615" },
                    { 5, 35, "#001A25" },
                    { 5, 36, "#acaaa8" },
                    { 5, 37, "#bcbab8" },
                    { 5, 38, "#001A25" },
                    { 5, 39, "#00BF73" },
                    { 5, 40, "#00BF73" },
                    { 5, 41, "#149F53" },
                    { 5, 42, "#FFFFFF" },
                    { 5, 43, "#00BF73" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_RecipeId",
                table: "Favorites",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginAttempts_UserId",
                table: "LoginAttempts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSteps_StepId",
                table: "RecipeSteps",
                column: "StepId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThemeVariablesColors_ThemeVariableId",
                table: "ThemeVariablesColors",
                column: "ThemeVariableId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRecipes_RecipeId",
                table: "UserRecipes",
                column: "RecipeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_GenderId",
                table: "Users",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserTypeId",
                table: "Users",
                column: "UserTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "ForgotPasswordRequests");

            migrationBuilder.DropTable(
                name: "FractionDecimals");

            migrationBuilder.DropTable(
                name: "LoginAttempts");

            migrationBuilder.DropTable(
                name: "RecipeIngredients");

            migrationBuilder.DropTable(
                name: "RecipeSteps");

            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.DropTable(
                name: "ThemeVariablesColors");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "UserRecipes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Steps");

            migrationBuilder.DropTable(
                name: "ThemeVariables");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "UserTypes");
        }
    }
}
