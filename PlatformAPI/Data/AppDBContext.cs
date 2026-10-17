using Microsoft.EntityFrameworkCore;

using PlatformAPI.Models.Themes;
using PlatformAPI.Models.Users;
using PlatformAPI.Models.Recipe;
using PlatformAPI.Models.Categories;
using System;
using Microsoft.EntityFrameworkCore.Migrations;
using PlatformAPI.Enums;
using PlatformAPI.Models.Content;
using PlatformAPI.Models.UnitConversion;

namespace PlatformAPI.Data
{


    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }


        public DbSet<User> Users { get; set; }

        public DbSet<Gender> Genders { get; set; }

        public DbSet<UserType> UserTypes { get; set; }

        public DbSet<Theme> Themes { get; set; }

        public DbSet<ThemeVariable> ThemeVariables { get; set; }

        public DbSet<ThemeVariableColor> ThemeVariablesColors { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<UserRecipe> UserRecipes { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<Unit> Units { get; set; }

        public DbSet<FractionDecimal> FractionDecimals { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

        public DbSet<Step> Steps { get; set; }

        public DbSet<RecipeStep> RecipeSteps { get; set; }

        public DbSet<ForgotPasswordRequest> ForgotPasswordRequests { get; set; }

        public DbSet<LoginAttempt> LoginAttempts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<RecipeCategory> RecipeCategories { get; set; }

        public DbSet<CookingTip> CookingTips { get; set; }

        public DbSet<UnitConversion> UnitConversion { get; set; }

        public DbSet<UnitConversionException> UnitConversionExceptions { get; set; }

        public DbSet<IngredientUnit> IngredientUnits { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LoginAttempt>().ToTable("LoginAttempts");

            //Enforce text max length limitations
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    // Apply default length of 50 to all strings
                    if (property.ClrType == typeof(string))
                    {
                        // Skip specific overrides
                        if (property.Name.Contains("Code"))
                        {
                            property.SetMaxLength(2); // Override: Code fields get length 2
                        }
                        else if (property.Name.Contains("Color"))
                        {
                            property.SetMaxLength(50); // Override: Color fields get length 7
                        }
                        else if (property.Name.Contains("Password"))
                        {
                            property.SetMaxLength(255); // Override: Color fields get length 255
                        }
                        else if (property.Name.Contains("FailureReason"))
                        {
                            property.SetMaxLength(100);
                        }
                        else if (property.GetMaxLength() == null)
                        {
                            property.SetMaxLength(50); // Default: Apply 50 if unconstrained
                        }
                        
                    }
                }
            }

            // Override for Recipe.Description
            modelBuilder.Entity<Recipe>()
                .Property(r => r.Description)
                .HasColumnType("nvarchar(max)");

            // Override for Step.Description
            modelBuilder.Entity<Step>()
                .Property(s => s.Description)
                .HasColumnType("nvarchar(max)");

            // Override for CookingTip.Description
            modelBuilder.Entity<CookingTip>()
                .Property(s => s.Description)
                .HasColumnType("nvarchar(max)");


            //Set complex primary keys
            modelBuilder.Entity<ThemeVariableColor>()
                .HasKey(tv => new { tv.ThemeId, tv.ThemeVariableId });

            modelBuilder.Entity<UserRecipe>()
                .HasKey(ur => new { ur.UserId, ur.RecipeId });

            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            modelBuilder.Entity<RecipeStep>()
                .HasKey(rs => new { rs.RecipeId, rs.StepId });

            modelBuilder.Entity<RecipeCategory>()
                .HasKey(rc => new { rc.RecipeId, rc.CategoryId });


            modelBuilder.Entity<User>()
                .Property(u => u.ShowCategories)
                .HasDefaultValue(false);

            modelBuilder.Entity<User>()
                .Property(u => u.CategorySortBy)
                .HasDefaultValue((Enums.SortBy)SortBy.Alphabetical);




            //Seed default data

            modelBuilder.Entity<Gender>().HasData(
                new Gender { Id = 1, Description = "Female", Code="F" },
                new Gender { Id = 2, Description = "Male", Code="M" },
                new Gender { Id = 3, Description = "Transgender", Code="TG" },
                new Gender { Id = 4, Description = "Non-binary", Code="NB" },
                new Gender { Id = 5, Description = "Other", Code="X" }
            );

            modelBuilder.Entity<UserType>().HasData(
                new UserType { Id = 1, Description = "Super Administrator", Code = "SA" },
                new UserType { Id = 2, Description = "Administrator", Code = "A" },
                new UserType { Id = 3, Description = "Restaurant", Code = "R" },
                new UserType { Id = 4, Description = "Standard User", Code = "S" },
                new UserType { Id = 5, Description = "Guest User", Code = "G" }
            );

            modelBuilder.Entity<Theme>().HasData(
                new Theme { Id = 1, Description = "Light Blue", SortOrder = 1, IsActive = true },
                new Theme { Id = 2, Description = "Dark", SortOrder = 2, IsActive = false },
                new Theme { Id = 3, Description = "Dark Teal", SortOrder = 3, IsActive = true },
                new Theme { Id = 4, Description = "Light Green", SortOrder = 4, IsActive = true },
                new Theme { Id = 5, Description = "Dark Green", SortOrder = 5, IsActive = true }
            );

            modelBuilder.Entity<ThemeVariable>().HasData(
                new ThemeVariable { Id = 1, Description = "dropdownBackColor", GroupId = 1, SortOrder = 1 },
                new ThemeVariable { Id = 2, Description = "dropdownTextColor", GroupId = 1, SortOrder = 3 },
                new ThemeVariable { Id = 3, Description = "dropdownHoverBackColor", GroupId = 1, SortOrder = 4 },
                new ThemeVariable { Id = 4, Description = "dropdownHoverTextColor", GroupId = 1, SortOrder = 6 },
                new ThemeVariable { Id = 5, Description = "dropdownContentBackgroundColor", GroupId = 1, SortOrder = 7 },
                new ThemeVariable { Id = 6, Description = "dropdownContentTextColor", GroupId = 1, SortOrder = 9 },
                new ThemeVariable { Id = 7, Description = "textBoxLabelColor", GroupId = 2, SortOrder = 1 },
                new ThemeVariable { Id = 8, Description = "buttonBackgroundColor", GroupId = 3, SortOrder = 1 },
                new ThemeVariable { Id = 9, Description = "buttonTextColor", GroupId = 3, SortOrder = 3 },
                new ThemeVariable { Id = 10, Description = "buttonHoverBackgroundColor", GroupId = 3, SortOrder = 4 },
                new ThemeVariable { Id = 11, Description = "buttonHoverTextColor", GroupId = 3, SortOrder = 6 },
                new ThemeVariable { Id = 12, Description = "contentBackColor", GroupId = 4, SortOrder = 1 },
                new ThemeVariable { Id = 13, Description = "textBoxTextColor", GroupId = 2, SortOrder = 2 },
                new ThemeVariable { Id = 14, Description = "textBoxBackColor", GroupId = 2, SortOrder = 3 },
                new ThemeVariable { Id = 15, Description = "outsideBackColor", GroupId = 5, SortOrder = 1 },
                new ThemeVariable { Id = 16, Description = "textBoxBorderColor", GroupId = 2, SortOrder = 5 },
                new ThemeVariable { Id = 17, Description = "dropdownBackColorGradient", GroupId = 1, SortOrder = 2 },
                new ThemeVariable { Id = 18, Description = "dropdownHoverBackColorGradient", GroupId = 1, SortOrder = 5 },
                new ThemeVariable { Id = 19, Description = "dropdownContentBackgroundColorGradient", GroupId = 1, SortOrder = 8 },
                new ThemeVariable { Id = 20, Description = "buttonBackgroundColorGradient", GroupId = 3, SortOrder = 2 },
                new ThemeVariable { Id = 21, Description = "buttonHoverBackgroundColorGradient", GroupId = 3, SortOrder = 5 },
                new ThemeVariable { Id = 22, Description = "contentBackColorGradient", GroupId = 4, SortOrder = 2 },
                new ThemeVariable { Id = 23, Description = "textBoxBackColorGradient", GroupId = 2, SortOrder = 4 },
                new ThemeVariable { Id = 24, Description = "outsideBackColorGradient", GroupId = 5, SortOrder = 2 },
                new ThemeVariable { Id = 25, Description = "contentHeaderBackColor", GroupId = 7, SortOrder = 1 },
                new ThemeVariable { Id = 26, Description = "contentHeaderBackColorGradient", GroupId = 7, SortOrder = 2 },
                new ThemeVariable { Id = 27, Description = "contentHeaderTextColor", GroupId = 7, SortOrder = 3 },
                new ThemeVariable { Id = 28, Description = "contentTextColor", GroupId = 7, SortOrder = 4 },
                new ThemeVariable { Id = 29, Description = "linkTextColor", GroupId = 7, SortOrder = 5 },
                new ThemeVariable { Id = 30, Description = "radioLabelColor", GroupId = 8, SortOrder = 1 },
                new ThemeVariable { Id = 31, Description = "radioHoverColor", GroupId = 8, SortOrder = 2 },
                new ThemeVariable { Id = 32, Description = "radioCheckColor", GroupId = 8, SortOrder = 3 },
                new ThemeVariable { Id = 33, Description = "dialogHeaderBackColor", GroupId = 9, SortOrder = 1 },
                new ThemeVariable { Id = 34, Description = "dialogHeaderBackColorGradient", GroupId = 9, SortOrder = 2 },
                new ThemeVariable { Id = 35, Description = "dialogHeaderTextColor", GroupId = 9, SortOrder = 3 },
                new ThemeVariable { Id = 36, Description = "dialogContentBackColor", GroupId = 9, SortOrder = 4 },
                new ThemeVariable { Id = 37, Description = "dialogContentBackColorGradient", GroupId = 9, SortOrder = 5 },
                new ThemeVariable { Id = 38, Description = "dialogContentTextColor", GroupId = 9, SortOrder = 6 },
                new ThemeVariable { Id = 39, Description = "sortableBorder", GroupId = 10, SortOrder = 1 },
                new ThemeVariable { Id = 40, Description = "sortableHighlightBackColor", GroupId = 10, SortOrder = 2 },
                new ThemeVariable { Id = 41, Description = "sortableHighlightBackColorGradient", GroupId = 10, SortOrder = 3 },
                new ThemeVariable { Id = 42, Description = "textBoxBorderColorSelected", GroupId = 2, SortOrder = 6 },
                new ThemeVariable { Id = 43, Description = "passwordIconColor", GroupId = 2, SortOrder = 7 }
            );

            //Seed ThemeVariableColors table

            modelBuilder.Entity<ThemeVariableColor>().HasData(
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 1, Color = "#0058f2" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 2, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 3, Color = "#0030a0" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 4, Color = "#FFFFFF" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 5, Color = "#0058f2" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 6, Color = "#FFFFFF" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 7, Color = "#0048d1" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 8, Color = "#0058f2" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 9, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 10, Color = "#0030a0" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 11, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 12, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 13, Color = "#000000" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 14, Color = "#A0C2FF" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 15, Color = "#f3f5f7" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 16, Color = "#0000ee" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 17, Color = "#3178dd" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 18, Color = "#0042b8" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 19, Color = "#3178dd" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 20, Color = "#3178dd" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 21, Color = "#0042b8" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 22, Color = "#efefef" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 23, Color = "#78AFFF" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 24, Color = "#e3e5e7" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 25, Color = "#0054ff" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 26, Color = "#1074ef" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 27, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 28, Color = "#0038a1" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 29, Color = "#0038a1" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 30, Color = "#0048d1" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 31, Color = "#003cb5" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 32, Color = "#0048d1" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 33, Color = "#0054ff" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 34, Color = "#1074ef" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 35, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 36, Color = "#c3c5c7" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 37, Color = "#b3b5b7" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 38, Color = "#00143f" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 39, Color = "#0048d1" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 40, Color = "#0048d1" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 41, Color = "#0058f2" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 42, Color = "#101010" },
                new ThemeVariableColor { ThemeId = 1, ThemeVariableId = 43, Color = "#000000" }
            );

            modelBuilder.Entity<ThemeVariableColor>().HasData(
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 1, Color = "#131210" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 2, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 3, Color = "#050302" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 4, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 5, Color = "#1f1e1c" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 6, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 7, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 8, Color = "#131210" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 9, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 10, Color = "#050302" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 11, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 12, Color = "#343434" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 13, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 14, Color = "#141414" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 15, Color = "#0c0a08" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 16, Color = "#18b5ea" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 17, Color = "#232220" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 18, Color = "#151312" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 19, Color = "#2f2e2c" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 20, Color = "#232220" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 21, Color = "#151312" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 22, Color = "#444444" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 23, Color = "#242424" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 24, Color = "#1c1a18" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 25, Color = "#131210" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 26, Color = "#232220" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 27, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 28, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 29, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 30, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 31, Color = "#bbbbbb" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 32, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 33, Color = "#131210" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 34, Color = "#232220" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 35, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 36, Color = "#acaaa8" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 37, Color = "#bcbab8" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 38, Color = "#000000" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 39, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 40, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 41, Color = "#dfdfdf" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 42, Color = "#FFFFFF" },
                new ThemeVariableColor { ThemeId = 2, ThemeVariableId = 43, Color = "#ffffff" }
            );

            modelBuilder.Entity<ThemeVariableColor>().HasData(
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 1, Color = "#0B5257" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 2, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 3, Color = "#0E3733" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 4, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 5, Color = "#0B5257" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 6, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 7, Color = "#00B0A0" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 8, Color = "#0B5257" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 9, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 10, Color = "#0E2723" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 11, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 12, Color = "#000000" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 13, Color = "#00B0A0" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 14, Color = "#0B4A4F" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 15, Color = "#0C0A08" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 16, Color = "#18b5ea" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 17, Color = "#123D3A" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 18, Color = "#0E3733" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 19, Color = "#1A3F3A" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 20, Color = "#1A3F3A" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 21, Color = "#0E3733" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 22, Color = "#101010" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 23, Color = "#242424" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 24, Color = "#1C1A18" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 25, Color = "#0B5257" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 26, Color = "#1A3F3A" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 27, Color = "#00B8A8" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 28, Color = "#00B0A0" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 29, Color = "#00B0A0" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 30, Color = "#00B0A0" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 31, Color = "#006D5B" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 32, Color = "#00B0A0" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 33, Color = "#0B5257" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 34, Color = "#1A3F3A" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 35, Color = "#00B8A8" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 36, Color = "#acaaa8" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 37, Color = "#bcbab8" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 38, Color = "#001205" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 39, Color = "#008080" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 40, Color = "#008080" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 41, Color = "#20a295" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 42, Color = "#FFFFFF" },
                new ThemeVariableColor { ThemeId = 3, ThemeVariableId = 43, Color = "#00B0A0" }
            );

            modelBuilder.Entity<ThemeVariableColor>().HasData(
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 1, Color = "#009837" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 2, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 3, Color = "#115543" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 4, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 5, Color = "#00751F" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 6, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 7, Color = "#009837" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 8, Color = "#009837" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 9, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 10, Color = "#115543" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 11, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 12, Color = "#f7fffe" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 13, Color = "#00321A" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 14, Color = "#31C74D" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 15, Color = "#fafefd" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 16, Color = "#008000" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 17, Color = "#10a847" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 18, Color = "#107847" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 19, Color = "#10a847" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 20, Color = "#10a847" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 21, Color = "#107847" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 22, Color = "#e7efee" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 23, Color = "#37B74D" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 24, Color = "#eaeeed" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 25, Color = "#009837" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 26, Color = "#10a847" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 27, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 28, Color = "#006327" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 29, Color = "#006327" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 30, Color = "#009837" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 31, Color = "#006235" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 32, Color = "#009837" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 33, Color = "#009837" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 34, Color = "#10a847" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 35, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 36, Color = "#cacecd" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 37, Color = "#babebd" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 38, Color = "#003225" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 39, Color = "#009837" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 40, Color = "#009837" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 41, Color = "#208255" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 42, Color = "#101010" },
                new ThemeVariableColor { ThemeId = 4, ThemeVariableId = 43, Color = "#00321A" }
            );

            modelBuilder.Entity<ThemeVariableColor>().HasData(
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 1, Color = "#0e7733" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 2, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 3, Color = "#073411" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 4, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 5, Color = "#0e7733" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 6, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 7, Color = "#00BF73" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 8, Color = "#0e7733" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 9, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 10, Color = "#073411" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 11, Color = "#ffffff" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 12, Color = "#000000" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 13, Color = "#00BF73" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 14, Color = "#0C4D40" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 15, Color = "#0c0a08" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 16, Color = "#18aa45" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 17, Color = "#184615" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 18, Color = "#0F3510" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 19, Color = "#184615" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 20, Color = "#184615" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 21, Color = "#0F3510" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 22, Color = "#101010" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 23, Color = "#242424" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 24, Color = "#1c1a18" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 25, Color = "#0e7733" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 26, Color = "#184615" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 27, Color = "#002225" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 28, Color = "#00BF73" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 29, Color = "#00BF73" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 30, Color = "#00BF73" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 31, Color = "#007235" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 32, Color = "#00BF73" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 33, Color = "#0e7733" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 34, Color = "#184615" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 35, Color = "#001A25" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 36, Color = "#acaaa8" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 37, Color = "#bcbab8" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 38, Color = "#001A25" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 39, Color = "#00BF73" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 40, Color = "#00BF73" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 41, Color = "#149F53" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 42, Color = "#FFFFFF" },
                new ThemeVariableColor { ThemeId = 5, ThemeVariableId = 43, Color = "#00BF73" }
            );

            modelBuilder.Entity<Unit>().HasData(
                new Unit { Id = 1, Description = "pound", Abbreviation = "lb", System = 1, Plural = "pounds" },
                new Unit { Id = 2, Description = "ounce", Abbreviation = "oz", System = 1, Plural = "ounces" },
                new Unit { Id = 3, Description = "gallon", Abbreviation = "gal", System = 1, Plural = "gallons" },
                new Unit { Id = 4, Description = "quart", Abbreviation = "qt", System = 1, Plural = "quarts" },
                new Unit { Id = 5, Description = "pint", Abbreviation = "pt", System = 1, Plural = "pints" },
                new Unit { Id = 7, Description = "fluid ounce", Abbreviation = "fl oz", System = 1, Plural = "fluid ounces" },
                new Unit { Id = 8, Description = "tablespoon", Abbreviation = "Tbsp", System = 1, Plural = "tablespoons" },
                new Unit { Id = 9, Description = "teaspoon", Abbreviation = "tsp", System = 1, Plural = "teaspoons" },
                new Unit { Id = 10, Description = "can", Abbreviation = null, System = null, Plural = "cans" },
                new Unit { Id = 11, Description = "cup", Abbreviation = null, System = 1, Plural = "cups" },
                new Unit { Id = 12, Description = "milliliter", Abbreviation = "ml", System = 2, Plural = "milliliters" },
                new Unit { Id = 13, Description = "liter", Abbreviation = "L", System = 2, Plural = "liters" },
                new Unit { Id = 14, Description = "gram", Abbreviation = "g", System = 2, Plural = "grams" },
                new Unit { Id = 15, Description = "kilogram", Abbreviation = "kg", System = 2, Plural = "kilograms" },
                new Unit { Id = 16, Description = "centimeter", Abbreviation = "cm", System = 2, Plural = "centimeters" },
                new Unit { Id = 17, Description = "pinch", Abbreviation = null, System = null, Plural = "pinch" },
                new Unit { Id = 18, Description = "clove", Abbreviation = null, System = null, Plural = "cloves" },
                new Unit { Id = 19, Description = "piece", Abbreviation = null, System = null, Plural = "pieces" },
                new Unit { Id = 20, Description = "stick", Abbreviation = null, System = null, Plural = "sticks" },
                new Unit { Id = 21, Description = "drop", Abbreviation = null, System = null, Plural = "drops" },
                new Unit { Id = 22, Description = "slice", Abbreviation = null, System = null, Plural = "slices" },
                new Unit { Id = 23, Description = "dash", Abbreviation = null, System = null, Plural = "dash" },
                new Unit { Id = 24, Description = "bunch", Abbreviation = null, System = null, Plural = "bunches" },
                new Unit { Id = 25, Description = "inch", Abbreviation = null, System = 1, Plural = "inches" },
                new Unit { Id = 26, Description = "millimeter", Abbreviation = "mm", System = 2, Plural = "millimeters" }
            );

            modelBuilder.Entity<FractionDecimal>().HasData(
                new FractionDecimal { Id = 1, Fraction = "1/16", Decimal = 0.0625f, Primary = true },
                new FractionDecimal { Id = 2, Fraction = "1/8", Decimal = 0.125f, Primary = true },
                new FractionDecimal { Id = 3, Fraction = "3/16", Decimal = 0.1875f, Primary = true },
                new FractionDecimal { Id = 4, Fraction = "1/4", Decimal = 0.25f, Primary = true },
                new FractionDecimal { Id = 5, Fraction = "5/16", Decimal = 0.3125f, Primary = true },
                new FractionDecimal { Id = 6, Fraction = "3/8", Decimal = 0.375f, Primary = true },
                new FractionDecimal { Id = 7, Fraction = "7/16", Decimal = 0.4375f, Primary = true },
                new FractionDecimal { Id = 8, Fraction = "1/2", Decimal = 0.5f, Primary = true },
                new FractionDecimal { Id = 9, Fraction = "9/16", Decimal = 0.5625f, Primary = true },
                new FractionDecimal { Id = 10, Fraction = "5/8", Decimal = 0.625f, Primary = true },
                new FractionDecimal { Id = 11, Fraction = "11/16", Decimal = 0.6875f, Primary = true },
                new FractionDecimal { Id = 12, Fraction = "3/4", Decimal = 0.75f, Primary = true },
                new FractionDecimal { Id = 13, Fraction = "13/16", Decimal = 0.8125f, Primary = true },
                new FractionDecimal { Id = 14, Fraction = "7/8", Decimal = 0.875f, Primary = true },
                new FractionDecimal { Id = 15, Fraction = "15/16", Decimal = 0.9375f, Primary = true },
                new FractionDecimal { Id = 16, Fraction = "4/16", Decimal = 0.25f, Primary = false },
                new FractionDecimal { Id = 17, Fraction = "2/16", Decimal = 0.125f, Primary = false },
                new FractionDecimal { Id = 18, Fraction = "6/16", Decimal = 0.375f, Primary = false },
                new FractionDecimal { Id = 19, Fraction = "8/16", Decimal = 0.5f, Primary = false },
                new FractionDecimal { Id = 20, Fraction = "10/16", Decimal = 0.625f, Primary = false },
                new FractionDecimal { Id = 21, Fraction = "12/16", Decimal = 0.75f, Primary = false },
                new FractionDecimal { Id = 22, Fraction = "14/16", Decimal = 0.875f, Primary = false },
                new FractionDecimal { Id = 23, Fraction = "2/8", Decimal = 0.25f, Primary = false },
                new FractionDecimal { Id = 24, Fraction = "4/8", Decimal = 0.5f, Primary = false },
                new FractionDecimal { Id = 25, Fraction = "6/8", Decimal = 0.75f, Primary = false },
                new FractionDecimal { Id = 26, Fraction = "2/4", Decimal = 0.5f, Primary = false },
                new FractionDecimal { Id = 27, Fraction = "1/3", Decimal = 0.333f, Primary = true },
                new FractionDecimal { Id = 28, Fraction = "2/3", Decimal = 0.667f, Primary = true },
                new FractionDecimal { Id = 29, Fraction = "1/5", Decimal = 0.2f, Primary = true },
                new FractionDecimal { Id = 30, Fraction = "2/5", Decimal = 0.4f, Primary = true },
                new FractionDecimal { Id = 31, Fraction = "3/5", Decimal = 0.6f, Primary = true },
                new FractionDecimal { Id = 32, Fraction = "4/5", Decimal = 0.8f, Primary = true }
            );

            //Cooking Tips
            modelBuilder.Entity<CookingTip>().HasData(
            new CookingTip { Id = 1, Description = "Salt your pasta water until it tastes like the sea; it’s your only chance to season the noodles.", IsActive = true },
            new CookingTip { Id = 2, Description = "Let meat rest after cooking so the juices redistribute and the texture stays tender.", IsActive = true },
            new CookingTip { Id = 3, Description = "Toast spices in a dry pan for 30 seconds to unlock deeper aroma and flavor.", IsActive = true },
            new CookingTip { Id = 4, Description = "Add a splash of acid (lemon or vinegar) when a dish tastes flat — it brightens everything.", IsActive = true },
            new CookingTip { Id = 5, Description = "Preheat your pan fully before adding oil to prevent sticking and ensure even browning.", IsActive = true },
            new CookingTip { Id = 6, Description = "Cut ingredients to similar sizes so they cook evenly and finish at the same time.", IsActive = true },
            new CookingTip { Id = 7, Description = "Reserve some pasta water to adjust sauce consistency and help it cling to noodles.", IsActive = true },
            new CookingTip { Id = 8, Description = "Use room‑temperature butter and eggs for smoother baking batters and better rise.", IsActive = true },
            new CookingTip { Id = 9, Description = "Sear meat without moving it for the first minute — that’s how you get a proper crust.", IsActive = true },
            new CookingTip { Id = 10, Description = "Taste as you cook; seasoning gradually creates better balance than adding salt at the end.", IsActive = true },
            new CookingTip { Id = 11, Description = "Add aromatics (garlic, ginger, onions) after your oil heats to avoid burning and bitterness.", IsActive = true },
            new CookingTip { Id = 12, Description = "Chill cookie dough before baking to prevent spreading and deepen flavor.", IsActive = true },
            new CookingTip { Id = 13, Description = "Use a thermometer for meat and baked goods — guessing leads to overcooking.", IsActive = true },
            new CookingTip { Id = 14, Description = "Bloom cocoa powder in hot water or coffee to intensify chocolate flavor in desserts.", IsActive = true },
            new CookingTip { Id = 15, Description = "Deglaze your pan with wine, broth, or vinegar to create instant sauce and add depth.", IsActive = true },
            new CookingTip { Id = 16, Description = "Dry proteins with a paper towel before cooking — moisture prevents browning.", IsActive = true },
            new CookingTip { Id = 17, Description = "Add herbs at the right time: woody herbs early, delicate herbs at the end.", IsActive = true },
            new CookingTip { Id = 18, Description = "Don’t overcrowd the pan; steam builds and prevents proper browning.", IsActive = true },
            new CookingTip { Id = 19, Description = "Use unsalted butter in baking so you control the salt level precisely.", IsActive = true },
            new CookingTip { Id = 20, Description = "Warm tortillas before serving — heat improves texture and prevents cracking.", IsActive = true },
            new CookingTip { Id = 21, Description = "Add a pinch of sugar to tomato sauces to balance acidity without making it sweet.", IsActive = true },
            new CookingTip { Id = 22, Description = "Let rice rest for 5–10 minutes after cooking to finish steaming and improve fluffiness.", IsActive = true },
            new CookingTip { Id = 23, Description = "Use a microplane for garlic to create smoother sauces and dressings.", IsActive = true },
            new CookingTip { Id = 24, Description = "Stir‑fry ingredients in batches so each cooks properly and stays crisp.", IsActive = true },
            new CookingTip { Id = 25, Description = "Add cold butter at the end of pan sauces for a glossy finish and richer flavor.", IsActive = true }
        );

            //Unit Conversion Tables
            modelBuilder.Entity<UnitConversion>().HasData(
                new UnitConversion { Id = 1, BaseSystem = 1, BaseUnit = "Inch", TargetSystem = 2, TargetUnit = "Centimeter", Quantity = 2.54f },
                new UnitConversion { Id = 2, BaseSystem = 2, BaseUnit = "Centimeter", TargetSystem = 1, TargetUnit = "Inch", Quantity = 0.3937008f },
                new UnitConversion { Id = 3, BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 4.92892f },
                new UnitConversion { Id = 4, BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.202884f },
                new UnitConversion { Id = 5, BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 14.7868f },
                new UnitConversion { Id = 6, BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.067628f },
                new UnitConversion { Id = 7, BaseSystem = 1, BaseUnit = "Fluid Ounce", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 29.5735f },
                new UnitConversion { Id = 8, BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.033814f },
                new UnitConversion { Id = 9, BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 236.588f },
                new UnitConversion { Id = 10, BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.00422675f },
                new UnitConversion { Id = 11, BaseSystem = 1, BaseUnit = "Pint", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 473.176f },
                new UnitConversion { Id = 12, BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 1, TargetUnit = "Pint", Quantity = 0.00211338f },
                new UnitConversion { Id = 13, BaseSystem = 1, BaseUnit = "Quart", TargetSystem = 2, TargetUnit = "Liter", Quantity = 0.946353f },
                new UnitConversion { Id = 14, BaseSystem = 2, BaseUnit = "Liter", TargetSystem = 1, TargetUnit = "Quart", Quantity = 1.05669f },
                new UnitConversion { Id = 15, BaseSystem = 1, BaseUnit = "Gallon", TargetSystem = 2, TargetUnit = "Liter", Quantity = 3.78541f },
                new UnitConversion { Id = 16, BaseSystem = 2, BaseUnit = "Liter", TargetSystem = 1, TargetUnit = "Gallon", Quantity = 0.264172f },
                new UnitConversion { Id = 17, BaseSystem = 1, BaseUnit = "Ounce", TargetSystem = 2, TargetUnit = "Gram", Quantity = 28.3495f },
                new UnitConversion { Id = 18, BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Ounce", Quantity = 0.035274f },
                new UnitConversion { Id = 19, BaseSystem = 1, BaseUnit = "Pound", TargetSystem = 2, TargetUnit = "Kilogram", Quantity = 0.453592f },
                new UnitConversion { Id = 20, BaseSystem = 2, BaseUnit = "Kilogram", TargetSystem = 1, TargetUnit = "Pound", Quantity = 2.20462f },
                new UnitConversion { Id = 21, BaseSystem = 1, BaseUnit = "Inch", TargetSystem = 2, TargetUnit = "Millimeter", Quantity = 25.4f },
                new UnitConversion { Id = 22, BaseSystem = 2, BaseUnit = "Millimeter", TargetSystem = 1, TargetUnit = "Inch", Quantity = 0.0393701f }
            );



            // Unit Conversion Exceptions
            modelBuilder.Entity<UnitConversionException>().HasData(
                new UnitConversionException { Id = 1, Ingredient = "honey", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 340f },
                new UnitConversionException { Id = 2, Ingredient = "honey", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.002941f },
                new UnitConversionException { Id = 3, Ingredient = "honey", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 21.25f },
                new UnitConversionException { Id = 4, Ingredient = "honey", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.047059f },
                new UnitConversionException { Id = 5, Ingredient = "honey", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 7.08f },
                new UnitConversionException { Id = 6, Ingredient = "honey", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.141243f },
                new UnitConversionException { Id = 7, Ingredient = "honey", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 1.42f },
                new UnitConversionException { Id = 8, Ingredient = "honey", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 0.704225f },
                new UnitConversionException { Id = 9, Ingredient = "maple syrup", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 322f },
                new UnitConversionException { Id = 10, Ingredient = "maple syrup", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.003106f },
                new UnitConversionException { Id = 11, Ingredient = "maple syrup", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 20.13f },
                new UnitConversionException { Id = 12, Ingredient = "maple syrup", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.049677f },
                new UnitConversionException { Id = 13, Ingredient = "maple syrup", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 6.71f },
                new UnitConversionException { Id = 14, Ingredient = "maple syrup", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.149031f },
                new UnitConversionException { Id = 15, Ingredient = "maple syrup", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 1.33f },
                new UnitConversionException { Id = 16, Ingredient = "maple syrup", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 0.75188f },
                new UnitConversionException { Id = 17, Ingredient = "molasses", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 336f },
                new UnitConversionException { Id = 18, Ingredient = "molasses", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.002976f },
                new UnitConversionException { Id = 19, Ingredient = "molasses", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 21f },
                new UnitConversionException { Id = 20, Ingredient = "molasses", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.047619f },
                new UnitConversionException { Id = 21, Ingredient = "molasses", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 7f },
                new UnitConversionException { Id = 22, Ingredient = "molasses", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.142857f },
                new UnitConversionException { Id = 23, Ingredient = "molasses", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 1.4f },
                new UnitConversionException { Id = 24, Ingredient = "molasses", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 0.714286f },
                new UnitConversionException { Id = 25, Ingredient = "corn syrup", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 328f },
                new UnitConversionException { Id = 26, Ingredient = "corn syrup", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.003049f },
                new UnitConversionException { Id = 27, Ingredient = "corn syrup", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 20.5f },
                new UnitConversionException { Id = 28, Ingredient = "corn syrup", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.04878f },
                new UnitConversionException { Id = 29, Ingredient = "corn syrup", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 6.83f },
                new UnitConversionException { Id = 30, Ingredient = "corn syrup", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.146413f },
                new UnitConversionException { Id = 31, Ingredient = "corn syrup", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 1.37f },
                new UnitConversionException { Id = 32, Ingredient = "corn syrup", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 0.729927f },
                new UnitConversionException { Id = 33, Ingredient = "oregano", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 44f },
                new UnitConversionException { Id = 34, Ingredient = "oregano", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.022727f },
                new UnitConversionException { Id = 35, Ingredient = "oregano", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 2.75f },
                new UnitConversionException { Id = 36, Ingredient = "oregano", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.363636f },
                new UnitConversionException { Id = 37, Ingredient = "oregano", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.92f },
                new UnitConversionException { Id = 38, Ingredient = "oregano", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 1.086957f },
                new UnitConversionException { Id = 39, Ingredient = "oregano", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.18f },
                new UnitConversionException { Id = 40, Ingredient = "oregano", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 5.555556f },
                new UnitConversionException { Id = 41, Ingredient = "basil", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 48f },
                new UnitConversionException { Id = 42, Ingredient = "basil", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.020833f },
                new UnitConversionException { Id = 43, Ingredient = "basil", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 3f },
                new UnitConversionException { Id = 44, Ingredient = "basil", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.333333f },
                new UnitConversionException { Id = 45, Ingredient = "basil", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 1f },
                new UnitConversionException { Id = 46, Ingredient = "basil", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 1f },
                new UnitConversionException { Id = 47, Ingredient = "basil", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.2f },
                new UnitConversionException { Id = 48, Ingredient = "basil", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 5f },
                new UnitConversionException { Id = 49, Ingredient = "thyme", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 48f },
                new UnitConversionException { Id = 50, Ingredient = "thyme", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.020833f },
                new UnitConversionException { Id = 51, Ingredient = "thyme", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 3f },
                new UnitConversionException { Id = 52, Ingredient = "thyme", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.333333f },
                new UnitConversionException { Id = 53, Ingredient = "thyme", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 1f },
                new UnitConversionException { Id = 54, Ingredient = "thyme", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 1f },
                new UnitConversionException { Id = 55, Ingredient = "thyme", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.2f },
                new UnitConversionException { Id = 56, Ingredient = "thyme", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 5f },
                new UnitConversionException { Id = 57, Ingredient = "rosemary", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 52f },
                new UnitConversionException { Id = 58, Ingredient = "rosemary", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.019231f },
                new UnitConversionException { Id = 59, Ingredient = "rosemary", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 3.25f },
                new UnitConversionException { Id = 60, Ingredient = "rosemary", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.307692f },
                new UnitConversionException { Id = 61, Ingredient = "rosemary", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 1.08f },
                new UnitConversionException { Id = 62, Ingredient = "rosemary", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.925926f },
                new UnitConversionException { Id = 63, Ingredient = "rosemary", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.22f },
                new UnitConversionException { Id = 64, Ingredient = "rosemary", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 4.545455f },
                new UnitConversionException { Id = 65, Ingredient = "sage", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 32f },
                new UnitConversionException { Id = 66, Ingredient = "sage", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.03125f },
                new UnitConversionException { Id = 67, Ingredient = "sage", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 2f },
                new UnitConversionException { Id = 68, Ingredient = "sage", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.5f },
                new UnitConversionException { Id = 69, Ingredient = "sage", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.67f },
                new UnitConversionException { Id = 70, Ingredient = "sage", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 1.492537f },
                new UnitConversionException { Id = 71, Ingredient = "sage", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.13f },
                new UnitConversionException { Id = 72, Ingredient = "sage", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 7.692308f },
                new UnitConversionException { Id = 73, Ingredient = "parsley", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 32f },
                new UnitConversionException { Id = 74, Ingredient = "parsley", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.03125f },
                new UnitConversionException { Id = 75, Ingredient = "parsley", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 2f },
                new UnitConversionException { Id = 76, Ingredient = "parsley", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.5f },
                new UnitConversionException { Id = 77, Ingredient = "parsley", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.67f },
                new UnitConversionException { Id = 78, Ingredient = "parsley", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 1.492537f },
                new UnitConversionException { Id = 79, Ingredient = "parsley", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.13f },
                new UnitConversionException { Id = 80, Ingredient = "parsley", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 7.692308f },
                new UnitConversionException { Id = 81, Ingredient = "dill", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 44f },
                new UnitConversionException { Id = 82, Ingredient = "dill", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.022727f },
                new UnitConversionException { Id = 83, Ingredient = "dill", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 2.75f },
                new UnitConversionException { Id = 84, Ingredient = "dill", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.363636f },
                new UnitConversionException { Id = 85, Ingredient = "dill", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.92f },
                new UnitConversionException { Id = 86, Ingredient = "dill", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 1.086957f },
                new UnitConversionException { Id = 87, Ingredient = "dill", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.18f },
                new UnitConversionException { Id = 88, Ingredient = "dill", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 5.555556f },
                new UnitConversionException { Id = 89, Ingredient = "mint", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 40f },
                new UnitConversionException { Id = 90, Ingredient = "mint", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.025f },
                new UnitConversionException { Id = 91, Ingredient = "mint", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 2.5f },
                new UnitConversionException { Id = 92, Ingredient = "mint", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.4f },
                new UnitConversionException { Id = 93, Ingredient = "mint", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.83f },
                new UnitConversionException { Id = 94, Ingredient = "mint", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 1.204819f },
                new UnitConversionException { Id = 95, Ingredient = "mint", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.17f },
                new UnitConversionException { Id = 96, Ingredient = "mint", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 5.882353f },
                new UnitConversionException { Id = 97, Ingredient = "salt", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 273f },
                new UnitConversionException { Id = 98, Ingredient = "salt", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.003663f },
                new UnitConversionException { Id = 99, Ingredient = "salt", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 17.06f },
                new UnitConversionException { Id = 100, Ingredient = "salt", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.058617f },
                new UnitConversionException { Id = 101, Ingredient = "salt", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 5.69f },
                new UnitConversionException { Id = 102, Ingredient = "salt", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.175747f },
                new UnitConversionException { Id = 103, Ingredient = "salt", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 1.15f },
                new UnitConversionException { Id = 104, Ingredient = "salt", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 0.869565f },
                new UnitConversionException { Id = 105, Ingredient = "black pepper", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 101f },
                new UnitConversionException { Id = 106, Ingredient = "black pepper", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.009901f },
                new UnitConversionException { Id = 107, Ingredient = "black pepper", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 6.31f },
                new UnitConversionException { Id = 108, Ingredient = "black pepper", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.158479f },
                new UnitConversionException { Id = 109, Ingredient = "black pepper", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 2.1f },
                new UnitConversionException { Id = 110, Ingredient = "black pepper", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.47619f },
                new UnitConversionException { Id = 111, Ingredient = "black pepper", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.43f },
                new UnitConversionException { Id = 112, Ingredient = "black pepper", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 2.325581f },
                new UnitConversionException { Id = 113, Ingredient = "white pepper", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 112f },
                new UnitConversionException { Id = 114, Ingredient = "white pepper", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.008929f },
                new UnitConversionException { Id = 115, Ingredient = "white pepper", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 7f },
                new UnitConversionException { Id = 116, Ingredient = "white pepper", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.142857f },
                new UnitConversionException { Id = 117, Ingredient = "white pepper", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 2.33f },
                new UnitConversionException { Id = 118, Ingredient = "white pepper", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.429185f },
                new UnitConversionException { Id = 119, Ingredient = "white pepper", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.47f },
                new UnitConversionException { Id = 120, Ingredient = "white pepper", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 2.12766f },
                new UnitConversionException { Id = 121, Ingredient = "cayenne pepper", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 86f },
                new UnitConversionException { Id = 122, Ingredient = "cayenne pepper", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.011628f },
                new UnitConversionException { Id = 123, Ingredient = "cayenne pepper", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 5.38f },
                new UnitConversionException { Id = 124, Ingredient = "cayenne pepper", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.185874f },
                new UnitConversionException { Id = 125, Ingredient = "cayenne pepper", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 1.79f },
                new UnitConversionException { Id = 126, Ingredient = "cayenne pepper", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.558659f },
                new UnitConversionException { Id = 127, Ingredient = "cayenne pepper", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.36f },
                new UnitConversionException { Id = 128, Ingredient = "cayenne pepper", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 2.777778f },
                new UnitConversionException { Id = 129, Ingredient = "paprika", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 101f },
                new UnitConversionException { Id = 130, Ingredient = "paprika", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.009901f },
                new UnitConversionException { Id = 131, Ingredient = "paprika", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 6.31f },
                new UnitConversionException { Id = 132, Ingredient = "paprika", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.158479f },
                new UnitConversionException { Id = 133, Ingredient = "paprika", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 2.1f },
                new UnitConversionException { Id = 134, Ingredient = "paprika", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.47619f },
                new UnitConversionException { Id = 135, Ingredient = "paprika", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.43f },
                new UnitConversionException { Id = 136, Ingredient = "paprika", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 2.325581f },
                new UnitConversionException { Id = 137, Ingredient = "smoked paprika", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 101f },
                new UnitConversionException { Id = 138, Ingredient = "smoked paprika", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.009901f },
                new UnitConversionException { Id = 139, Ingredient = "smoked paprika", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 6.31f },
                new UnitConversionException { Id = 140, Ingredient = "smoked paprika", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.158479f },
                new UnitConversionException { Id = 141, Ingredient = "smoked paprika", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 2.1f },
                new UnitConversionException { Id = 142, Ingredient = "smoked paprika", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.47619f },
                new UnitConversionException { Id = 143, Ingredient = "smoked paprika", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.43f },
                new UnitConversionException { Id = 144, Ingredient = "smoked paprika", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 2.325581f },
                new UnitConversionException { Id = 145, Ingredient = "chili powder", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 96f },
                new UnitConversionException { Id = 146, Ingredient = "chili powder", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.010417f },
                new UnitConversionException { Id = 147, Ingredient = "chili powder", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 6f },
                new UnitConversionException { Id = 148, Ingredient = "chili powder", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.166667f },
                new UnitConversionException { Id = 149, Ingredient = "chili powder", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 2f },
                new UnitConversionException { Id = 150, Ingredient = "chili powder", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.5f },
                new UnitConversionException { Id = 151, Ingredient = "chili powder", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.41f },
                new UnitConversionException { Id = 152, Ingredient = "chili powder", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 2.439024f },
                new UnitConversionException { Id = 153, Ingredient = "cumin", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 96f },
                new UnitConversionException { Id = 154, Ingredient = "cumin", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.010417f },
                new UnitConversionException { Id = 155, Ingredient = "cumin", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 6f },
                new UnitConversionException { Id = 156, Ingredient = "cumin", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.166667f },
                new UnitConversionException { Id = 157, Ingredient = "cumin", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 2f },
                new UnitConversionException { Id = 158, Ingredient = "cumin", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.5f },
                new UnitConversionException { Id = 159, Ingredient = "cumin", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.41f },
                new UnitConversionException { Id = 160, Ingredient = "cumin", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 2.439024f },
                new UnitConversionException { Id = 161, Ingredient = "coriander", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 86f },
                new UnitConversionException { Id = 162, Ingredient = "coriander", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.011628f },
                new UnitConversionException { Id = 163, Ingredient = "coriander", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 5.38f },
                new UnitConversionException { Id = 164, Ingredient = "coriander", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.185874f },
                new UnitConversionException { Id = 165, Ingredient = "coriander", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 1.79f },
                new UnitConversionException { Id = 166, Ingredient = "coriander", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.558659f },
                new UnitConversionException { Id = 167, Ingredient = "coriander", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.36f },
                new UnitConversionException { Id = 168, Ingredient = "coriander", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 2.777778f },
                new UnitConversionException { Id = 169, Ingredient = "turmeric", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 144f },
                new UnitConversionException { Id = 170, Ingredient = "turmeric", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.006944f },
                new UnitConversionException { Id = 171, Ingredient = "turmeric", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 9f },
                new UnitConversionException { Id = 172, Ingredient = "turmeric", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.111111f },
                new UnitConversionException { Id = 173, Ingredient = "turmeric", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 3f },
                new UnitConversionException { Id = 174, Ingredient = "turmeric", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.333333f },
                new UnitConversionException { Id = 175, Ingredient = "turmeric", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.61f },
                new UnitConversionException { Id = 176, Ingredient = "turmeric", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 1.639344f },
                new UnitConversionException { Id = 177, Ingredient = "cinnamon", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 125f },
                new UnitConversionException { Id = 178, Ingredient = "cinnamon", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.008f },
                new UnitConversionException { Id = 179, Ingredient = "cinnamon", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 7.81f },
                new UnitConversionException { Id = 180, Ingredient = "cinnamon", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.128041f },
                new UnitConversionException { Id = 181, Ingredient = "cinnamon", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 2.6f },
                new UnitConversionException { Id = 182, Ingredient = "cinnamon", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.384615f },
                new UnitConversionException { Id = 183, Ingredient = "cinnamon", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.53f },
                new UnitConversionException { Id = 184, Ingredient = "cinnamon", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 1.886792f },
                new UnitConversionException { Id = 185, Ingredient = "nutmeg", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 106f },
                new UnitConversionException { Id = 186, Ingredient = "nutmeg", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.009434f },
                new UnitConversionException { Id = 187, Ingredient = "nutmeg", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 6.63f },
                new UnitConversionException { Id = 188, Ingredient = "nutmeg", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.15083f },
                new UnitConversionException { Id = 189, Ingredient = "nutmeg", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 2.21f },
                new UnitConversionException { Id = 190, Ingredient = "nutmeg", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.452489f },
                new UnitConversionException { Id = 191, Ingredient = "nutmeg", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.45f },
                new UnitConversionException { Id = 192, Ingredient = "nutmeg", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 2.222222f },
                new UnitConversionException { Id = 193, Ingredient = "clove", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 101f },
                new UnitConversionException { Id = 194, Ingredient = "clove", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.009901f },
                new UnitConversionException { Id = 195, Ingredient = "clove", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 6.31f },
                new UnitConversionException { Id = 196, Ingredient = "clove", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.158479f },
                new UnitConversionException { Id = 197, Ingredient = "clove", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 2.1f },
                new UnitConversionException { Id = 198, Ingredient = "clove", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.47619f },
                new UnitConversionException { Id = 199, Ingredient = "clove", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.43f },
                new UnitConversionException { Id = 200, Ingredient = "clove", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 2.325581f },
                new UnitConversionException { Id = 201, Ingredient = "allspice", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 91f },
                new UnitConversionException { Id = 202, Ingredient = "allspice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.010989f },
                new UnitConversionException { Id = 203, Ingredient = "allspice", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 5.69f },
                new UnitConversionException { Id = 204, Ingredient = "allspice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.175747f },
                new UnitConversionException { Id = 205, Ingredient = "allspice", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 1.9f },
                new UnitConversionException { Id = 206, Ingredient = "allspice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.526316f },
                new UnitConversionException { Id = 207, Ingredient = "allspice", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.38f },
                new UnitConversionException { Id = 208, Ingredient = "allspice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 2.631579f },
                new UnitConversionException { Id = 209, Ingredient = "cardamom", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 96f },
                new UnitConversionException { Id = 210, Ingredient = "cardamom", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.010417f },
                new UnitConversionException { Id = 211, Ingredient = "cardamom", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 6f },
                new UnitConversionException { Id = 212, Ingredient = "cardamom", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.166667f },
                new UnitConversionException { Id = 213, Ingredient = "cardamom", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 2f },
                new UnitConversionException { Id = 214, Ingredient = "cardamom", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.5f },
                new UnitConversionException { Id = 215, Ingredient = "cardamom", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.41f },
                new UnitConversionException { Id = 216, Ingredient = "cardamom", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 2.439024f },
                new UnitConversionException { Id = 217, Ingredient = "garam masala", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 96f },
                new UnitConversionException { Id = 218, Ingredient = "garam masala", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.010417f },
                new UnitConversionException { Id = 219, Ingredient = "garam masala", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 6f },
                new UnitConversionException { Id = 220, Ingredient = "garam masala", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.166667f },
                new UnitConversionException { Id = 221, Ingredient = "garam masala", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 2f },
                new UnitConversionException { Id = 222, Ingredient = "garam masala", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.5f },
                new UnitConversionException { Id = 223, Ingredient = "garam masala", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.41f },
                new UnitConversionException { Id = 224, Ingredient = "garam masala", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 2.439024f },
                new UnitConversionException { Id = 225, Ingredient = "rice", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 185f },
                new UnitConversionException { Id = 226, Ingredient = "rice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.005405f },
                new UnitConversionException { Id = 227, Ingredient = "rice", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 11.56f },
                new UnitConversionException { Id = 228, Ingredient = "rice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.086505f },
                new UnitConversionException { Id = 229, Ingredient = "rice", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 3.85f },
                new UnitConversionException { Id = 230, Ingredient = "rice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.25974f },
                new UnitConversionException { Id = 231, Ingredient = "rice", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.78f },
                new UnitConversionException { Id = 232, Ingredient = "rice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 1.282051f },
                new UnitConversionException { Id = 233, Ingredient = "brown rice", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 190f },
                new UnitConversionException { Id = 234, Ingredient = "brown rice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.005263f },
                new UnitConversionException { Id = 235, Ingredient = "brown rice", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 11.88f },
                new UnitConversionException { Id = 236, Ingredient = "brown rice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.084175f },
                new UnitConversionException { Id = 237, Ingredient = "brown rice", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 3.96f },
                new UnitConversionException { Id = 238, Ingredient = "brown rice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.252525f },
                new UnitConversionException { Id = 239, Ingredient = "brown rice", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.8f },
                new UnitConversionException { Id = 240, Ingredient = "brown rice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 1.25f },
                new UnitConversionException { Id = 241, Ingredient = "jasmine rice", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 185f },
                new UnitConversionException { Id = 242, Ingredient = "jasmine rice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.005405f },
                new UnitConversionException { Id = 243, Ingredient = "jasmine rice", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 11.56f },
                new UnitConversionException { Id = 244, Ingredient = "jasmine rice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.086505f },
                new UnitConversionException { Id = 245, Ingredient = "jasmine rice", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 3.85f },
                new UnitConversionException { Id = 246, Ingredient = "jasmine rice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.25974f },
                new UnitConversionException { Id = 247, Ingredient = "jasmine rice", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.78f },
                new UnitConversionException { Id = 248, Ingredient = "jasmine rice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 1.282051f },
                new UnitConversionException { Id = 249, Ingredient = "basmati rice", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 180f },
                new UnitConversionException { Id = 250, Ingredient = "basmati rice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.005556f },
                new UnitConversionException { Id = 251, Ingredient = "basmati rice", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 11.25f },
                new UnitConversionException { Id = 252, Ingredient = "basmati rice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.088889f },
                new UnitConversionException { Id = 253, Ingredient = "basmati rice", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 3.75f },
                new UnitConversionException { Id = 254, Ingredient = "basmati rice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.266667f },
                new UnitConversionException { Id = 255, Ingredient = "basmati rice", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.76f },
                new UnitConversionException { Id = 256, Ingredient = "basmati rice", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 1.315789f },
                new UnitConversionException { Id = 257, Ingredient = "quinoa", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 170f },
                new UnitConversionException { Id = 258, Ingredient = "quinoa", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.005882f },
                new UnitConversionException { Id = 259, Ingredient = "quinoa", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 10.63f },
                new UnitConversionException { Id = 260, Ingredient = "quinoa", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.094073f },
                new UnitConversionException { Id = 261, Ingredient = "quinoa", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 3.54f },
                new UnitConversionException { Id = 262, Ingredient = "quinoa", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.282486f },
                new UnitConversionException { Id = 263, Ingredient = "quinoa", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.72f },
                new UnitConversionException { Id = 264, Ingredient = "quinoa", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 1.388889f },
                new UnitConversionException { Id = 265, Ingredient = "barley", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 184f },
                new UnitConversionException { Id = 266, Ingredient = "barley", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.005435f },
                new UnitConversionException { Id = 267, Ingredient = "barley", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 11.5f },
                new UnitConversionException { Id = 268, Ingredient = "barley", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.086957f },
                new UnitConversionException { Id = 269, Ingredient = "barley", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 3.83f },
                new UnitConversionException { Id = 270, Ingredient = "barley", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.261097f },
                new UnitConversionException { Id = 271, Ingredient = "barley", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.78f },
                new UnitConversionException { Id = 272, Ingredient = "barley", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 1.282051f },
                new UnitConversionException { Id = 273, Ingredient = "oatmeal", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 80f },
                new UnitConversionException { Id = 274, Ingredient = "oatmeal", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.0125f },
                new UnitConversionException { Id = 275, Ingredient = "oatmeal", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 5f },
                new UnitConversionException { Id = 276, Ingredient = "oatmeal", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.2f },
                new UnitConversionException { Id = 277, Ingredient = "oatmeal", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 1.67f },
                new UnitConversionException { Id = 278, Ingredient = "oatmeal", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.598802f },
                new UnitConversionException { Id = 279, Ingredient = "oatmeal", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.34f },
                new UnitConversionException { Id = 280, Ingredient = "oatmeal", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 2.941176f },
                new UnitConversionException { Id = 281, Ingredient = "rolled oats", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 90f },
                new UnitConversionException { Id = 282, Ingredient = "rolled oats", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.011111f },
                new UnitConversionException { Id = 283, Ingredient = "rolled oats", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 5.63f },
                new UnitConversionException { Id = 284, Ingredient = "rolled oats", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.17762f },
                new UnitConversionException { Id = 285, Ingredient = "rolled oats", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 1.88f },
                new UnitConversionException { Id = 286, Ingredient = "rolled oats", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.531915f },
                new UnitConversionException { Id = 287, Ingredient = "rolled oats", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.38f },
                new UnitConversionException { Id = 288, Ingredient = "rolled oats", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 2.631579f },
                new UnitConversionException { Id = 289, Ingredient = "oats", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 90f },
                new UnitConversionException { Id = 290, Ingredient = "oats", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.011111f },
                new UnitConversionException { Id = 291, Ingredient = "oats", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 5.63f },
                new UnitConversionException { Id = 292, Ingredient = "oats", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.17762f },
                new UnitConversionException { Id = 293, Ingredient = "oats", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 1.88f },
                new UnitConversionException { Id = 294, Ingredient = "oats", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.531915f },
                new UnitConversionException { Id = 295, Ingredient = "oats", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.38f },
                new UnitConversionException { Id = 296, Ingredient = "oats", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 2.631579f },
                new UnitConversionException { Id = 297, Ingredient = "lentils", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 192f },
                new UnitConversionException { Id = 298, Ingredient = "lentils", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.005208f },
                new UnitConversionException { Id = 299, Ingredient = "lentils", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 12f },
                new UnitConversionException { Id = 300, Ingredient = "lentils", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.083333f },
                new UnitConversionException { Id = 301, Ingredient = "lentils", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 4f },
                new UnitConversionException { Id = 302, Ingredient = "lentils", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.25f },
                new UnitConversionException { Id = 303, Ingredient = "lentils", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.81f },
                new UnitConversionException { Id = 304, Ingredient = "lentils", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 1.234568f },
                new UnitConversionException { Id = 305, Ingredient = "chickpeas", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 200f },
                new UnitConversionException { Id = 306, Ingredient = "chickpeas", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.005f },
                new UnitConversionException { Id = 307, Ingredient = "chickpeas", BaseSystem = 1, BaseUnit = "Tablespoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 12.5f },
                new UnitConversionException { Id = 308, Ingredient = "chickpeas", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Tablespoon", Quantity = 0.08f },
                new UnitConversionException { Id = 309, Ingredient = "chickpeas", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 4.17f },
                new UnitConversionException { Id = 310, Ingredient = "chickpeas", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.239808f },
                new UnitConversionException { Id = 311, Ingredient = "chickpeas", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.85f },
                new UnitConversionException { Id = 312, Ingredient = "chickpeas", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 1.176471f },
                new UnitConversionException { Id = 313, Ingredient = "black beans", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 194f },
                new UnitConversionException { Id = 314, Ingredient = "black beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.005155f },
                new UnitConversionException { Id = 315, Ingredient = "black beans", BaseSystem = 1, BaseUnit = "Ounce", TargetSystem = 2, TargetUnit = "Gram", Quantity = 24.25f },
                new UnitConversionException { Id = 316, Ingredient = "black beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Ounce", Quantity = 0.041237f },
                new UnitConversionException { Id = 317, Ingredient = "black beans", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 4.04f },
                new UnitConversionException { Id = 318, Ingredient = "black beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.247525f },
                new UnitConversionException { Id = 319, Ingredient = "black beans", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.82f },
                new UnitConversionException { Id = 320, Ingredient = "black beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 1.219512f },
                new UnitConversionException { Id = 321, Ingredient = "pinto beans", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 193f },
                new UnitConversionException { Id = 322, Ingredient = "pinto beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.005181f },
                new UnitConversionException { Id = 323, Ingredient = "pinto beans", BaseSystem = 1, BaseUnit = "Ounce", TargetSystem = 2, TargetUnit = "Gram", Quantity = 24.12f },
                new UnitConversionException { Id = 324, Ingredient = "pinto beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Ounce", Quantity = 0.041451f },
                new UnitConversionException { Id = 325, Ingredient = "pinto beans", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 4.02f },
                new UnitConversionException { Id = 326, Ingredient = "pinto beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.248756f },
                new UnitConversionException { Id = 327, Ingredient = "pinto beans", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.82f },
                new UnitConversionException { Id = 328, Ingredient = "pinto beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 1.219512f },
                new UnitConversionException { Id = 329, Ingredient = "kidney beans", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 184f },
                new UnitConversionException { Id = 330, Ingredient = "kidney beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.005435f },
                new UnitConversionException { Id = 331, Ingredient = "kidney beans", BaseSystem = 1, BaseUnit = "Ounce", TargetSystem = 2, TargetUnit = "Gram", Quantity = 23f },
                new UnitConversionException { Id = 332, Ingredient = "kidney beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Ounce", Quantity = 0.043478f },
                new UnitConversionException { Id = 333, Ingredient = "kidney beans", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 3.83f },
                new UnitConversionException { Id = 334, Ingredient = "kidney beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.261097f },
                new UnitConversionException { Id = 335, Ingredient = "kidney beans", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.78f },
                new UnitConversionException { Id = 336, Ingredient = "kidney beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 1.282051f },
                new UnitConversionException { Id = 337, Ingredient = "great northern beans", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 182f },
                new UnitConversionException { Id = 338, Ingredient = "great northern beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.005495f },
                new UnitConversionException { Id = 339, Ingredient = "great northern beans", BaseSystem = 1, BaseUnit = "Ounce", TargetSystem = 2, TargetUnit = "Gram", Quantity = 22.75f },
                new UnitConversionException { Id = 340, Ingredient = "great northern beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Ounce", Quantity = 0.043956f },
                new UnitConversionException { Id = 341, Ingredient = "great northern beans", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 3.79f },
                new UnitConversionException { Id = 342, Ingredient = "great northern beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.263852f },
                new UnitConversionException { Id = 343, Ingredient = "great northern beans", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.77f },
                new UnitConversionException { Id = 344, Ingredient = "great northern beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 2, TargetUnit = "Milliliter", Quantity = 1.298701f },
                new UnitConversionException { Id = 345, Ingredient = "navy beans", BaseSystem = 1, BaseUnit = "Cup", TargetSystem = 2, TargetUnit = "Gram", Quantity = 190f },
                new UnitConversionException { Id = 346, Ingredient = "navy beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Cup", Quantity = 0.005263f },
                new UnitConversionException { Id = 347, Ingredient = "navy beans", BaseSystem = 1, BaseUnit = "Ounce", TargetSystem = 2, TargetUnit = "Gram", Quantity = 23.75f },
                new UnitConversionException { Id = 348, Ingredient = "navy beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Ounce", Quantity = 0.042105f },
                new UnitConversionException { Id = 349, Ingredient = "navy beans", BaseSystem = 1, BaseUnit = "Teaspoon", TargetSystem = 2, TargetUnit = "Gram", Quantity = 3.96f },
                new UnitConversionException { Id = 350, Ingredient = "navy beans", BaseSystem = 2, BaseUnit = "Gram", TargetSystem = 1, TargetUnit = "Teaspoon", Quantity = 0.252525f },
                new UnitConversionException { Id = 351, Ingredient = "navy beans", BaseSystem = 2, BaseUnit = "Milliliter", TargetSystem = 2, TargetUnit = "Gram", Quantity = 0.8f }


            );

            //IngredientUnit
            modelBuilder.Entity<IngredientUnit>().HasData(
                new IngredientUnit { Id = 1, Ingredient = "salt", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 2, Ingredient = "black pepper", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 3, Ingredient = "white pepper", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 4, Ingredient = "cayenne pepper", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 5, Ingredient = "paprika", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 6, Ingredient = "smoked paprika", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 7, Ingredient = "chili powder", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 8, Ingredient = "cumin", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 9, Ingredient = "coriander", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 10, Ingredient = "cumin seeds", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 11, Ingredient = "mustard seeds", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 12, Ingredient = "fennel seeds", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 13, Ingredient = "caraway seeds", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 14, Ingredient = "turmeric", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 15, Ingredient = "cinnamon", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 16, Ingredient = "nutmeg", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 17, Ingredient = "clove", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 18, Ingredient = "allspice", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 19, Ingredient = "cardamom", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 20, Ingredient = "garam masala", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 21, Ingredient = "oregano", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 22, Ingredient = "basil", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 23, Ingredient = "thyme", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 24, Ingredient = "rosemary", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 25, Ingredient = "sage", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 26, Ingredient = "parsley", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 27, Ingredient = "dill", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 28, Ingredient = "bay leaf", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 29, Ingredient = "mint", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 30, Ingredient = "vanilla extract", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 31, Ingredient = "almond extract", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 32, Ingredient = "lemon juice", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 33, Ingredient = "lime juice", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 34, Ingredient = "vinegar", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 35, Ingredient = "olive oil", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 36, Ingredient = "canola oil", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 37, Ingredient = "vegetable oil", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 38, Ingredient = "sesame oil", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 39, Ingredient = "coconut oil", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 40, Ingredient = "butter", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 41, Ingredient = "flour", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 2 },
                new IngredientUnit { Id = 42, Ingredient = "all purpose flour", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 43, Ingredient = "bread flour", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 44, Ingredient = "cake flour", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 45, Ingredient = "whole wheat flour", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 46, Ingredient = "cornmeal", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 47, Ingredient = "oatmeal", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 48, Ingredient = "rolled oats", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 49, Ingredient = "oats", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 2 },
                new IngredientUnit { Id = 50, Ingredient = "rice", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 2 },
                new IngredientUnit { Id = 51, Ingredient = "brown rice", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 52, Ingredient = "jasmine rice", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 53, Ingredient = "basmati rice", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 54, Ingredient = "quinoa", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 55, Ingredient = "barley", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 56, Ingredient = "lentils", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 57, Ingredient = "chickpeas", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 58, Ingredient = "black beans", LargeImperialUnit = "cup", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 59, Ingredient = "pinto beans", LargeImperialUnit = "cup", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 60, Ingredient = "kidney beans", LargeImperialUnit = "cup", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 61, Ingredient = "great northern beans", LargeImperialUnit = "cup", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 62, Ingredient = "navy beans", LargeImperialUnit = "cup", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 63, Ingredient = "beans", LargeImperialUnit = "cup", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 2 },
                new IngredientUnit { Id = 64, Ingredient = "sugar", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 2 },
                new IngredientUnit { Id = 65, Ingredient = "brown sugar", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 66, Ingredient = "powdered sugar", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 67, Ingredient = "honey", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 68, Ingredient = "maple syrup", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 69, Ingredient = "molasses", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 70, Ingredient = "corn syrup", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 71, Ingredient = "milk", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 2, IsLiquid = true },
                new IngredientUnit { Id = 72, Ingredient = "whole milk", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1, IsLiquid = true },
                new IngredientUnit { Id = 73, Ingredient = "skim milk", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1, IsLiquid = true },
                new IngredientUnit { Id = 74, Ingredient = "heavy cream", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1, IsLiquid = true },
                new IngredientUnit { Id = 75, Ingredient = "half and half", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1, IsLiquid = true },
                new IngredientUnit { Id = 76, Ingredient = "buttermilk", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 77, Ingredient = "sour cream", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 78, Ingredient = "yogurt", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 2, IsLiquid = true },
                new IngredientUnit { Id = 79, Ingredient = "greek yogurt", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1, IsLiquid = true },
                new IngredientUnit { Id = 80, Ingredient = "cream cheese", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 81, Ingredient = "cottage cheese", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 82, Ingredient = "ricotta cheese", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 83, Ingredient = "parmesan cheese", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 84, Ingredient = "cheddar cheese", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 85, Ingredient = "mozzarella cheese", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 86, Ingredient = "swiss cheese", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 87, Ingredient = "provolone cheese", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 88, Ingredient = "feta cheese", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 89, Ingredient = "egg", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 2 },
                new IngredientUnit { Id = 90, Ingredient = "egg white", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 91, Ingredient = "egg yolk", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1 },
                new IngredientUnit { Id = 92, Ingredient = "chicken breast", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 93, Ingredient = "chicken thigh", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 94, Ingredient = "ground chicken", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 95, Ingredient = "chicken", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 2 },
                new IngredientUnit { Id = 96, Ingredient = "turkey", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 2 },
                new IngredientUnit { Id = 97, Ingredient = "ground turkey", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 98, Ingredient = "ground beef", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 99, Ingredient = "beef chuck", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 100, Ingredient = "beef roast", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 101, Ingredient = "beef", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 2 },
                new IngredientUnit { Id = 102, Ingredient = "steak", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 103, Ingredient = "pork chop", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 104, Ingredient = "ground pork", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 105, Ingredient = "pork", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 2 },
                new IngredientUnit { Id = 106, Ingredient = "bacon", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 107, Ingredient = "sausage", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 108, Ingredient = "ham", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 109, Ingredient = "lamb", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 110, Ingredient = "salmon", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 111, Ingredient = "tuna", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 112, Ingredient = "cod", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 113, Ingredient = "halibut", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 114, Ingredient = "shrimp", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 115, Ingredient = "crab", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 116, Ingredient = "lobster", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 117, Ingredient = "scallop", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 118, Ingredient = "potato", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 2 },
                new IngredientUnit { Id = 119, Ingredient = "sweet potato", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 120, Ingredient = "onion", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 2 },
                new IngredientUnit { Id = 121, Ingredient = "red onion", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 122, Ingredient = "green onion", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 123, Ingredient = "shallot", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 124, Ingredient = "garlic", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 125, Ingredient = "ginger", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 126, Ingredient = "carrot", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 127, Ingredient = "celery", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 128, Ingredient = "broccoli", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 129, Ingredient = "cauliflower", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 130, Ingredient = "cabbage", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 131, Ingredient = "spinach", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 132, Ingredient = "kale", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 133, Ingredient = "lettuce", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 134, Ingredient = "zucchini", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 135, Ingredient = "cucumber", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 136, Ingredient = "eggplant", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 137, Ingredient = "bell pepper", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 138, Ingredient = "jalapeno", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 139, Ingredient = "serrano pepper", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 140, Ingredient = "mushroom", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 141, Ingredient = "tomato", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 2 },
                new IngredientUnit { Id = 142, Ingredient = "cherry tomato", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 143, Ingredient = "apple", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 144, Ingredient = "banana", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 145, Ingredient = "orange", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 146, Ingredient = "lemon", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 147, Ingredient = "lime", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 148, Ingredient = "strawberry", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 149, Ingredient = "blueberry", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 150, Ingredient = "raspberry", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 151, Ingredient = "blackberry", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 152, Ingredient = "berry", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 2 },
                new IngredientUnit { Id = 153, Ingredient = "grape", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 154, Ingredient = "peach", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 155, Ingredient = "pear", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 156, Ingredient = "avocado", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 157, Ingredient = "pineapple", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 158, Ingredient = "mango", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 159, Ingredient = "papaya", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 160, Ingredient = "cantaloupe", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 161, Ingredient = "watermelon", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 162, Ingredient = "honeydew", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 163, Ingredient = "kiwi", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 164, Ingredient = "pomegranate", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 165, Ingredient = "coconut", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 166, Ingredient = "cranberry", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 167, Ingredient = "cherry", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 168, Ingredient = "apricot", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 169, Ingredient = "plum", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "kg", Pass = 1 },
                new IngredientUnit { Id = 170, Ingredient = "fig", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 171, Ingredient = "date", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 172, Ingredient = "raisin", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 173, Ingredient = "walnut", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 174, Ingredient = "pecan", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 175, Ingredient = "almond", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 176, Ingredient = "cashew", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 177, Ingredient = "pistachio", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 178, Ingredient = "peanut", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 179, Ingredient = "pine nut", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 180, Ingredient = "hazelnut", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 181, Ingredient = "macadamia nut", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 182, Ingredient = "nut", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 2 },
                new IngredientUnit { Id = 183, Ingredient = "chia seed", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 184, Ingredient = "flax seed", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 185, Ingredient = "sunflower seed", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 186, Ingredient = "pumpkin seed", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 187, Ingredient = "sesame seed", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 188, Ingredient = "seed", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 2 },
                new IngredientUnit { Id = 189, Ingredient = "cocoa powder", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 190, Ingredient = "chocolate chip", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 191, Ingredient = "dark chocolate", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 192, Ingredient = "milk chocolate", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 193, Ingredient = "white chocolate", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 194, Ingredient = "chocolate", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 2 },
                new IngredientUnit { Id = 195, Ingredient = "baking soda", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 196, Ingredient = "baking powder", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 197, Ingredient = "yeast", LargeImperialUnit = "tbsp", SmallImperialUnit = "tsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 198, Ingredient = "cornstarch", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 199, Ingredient = "corn starch", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 200, Ingredient = "cocoa nib", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 201, Ingredient = "peanut butter", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1, IsLiquid = true },
                new IngredientUnit { Id = 202, Ingredient = "almond butter", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1, IsLiquid = true },
                new IngredientUnit { Id = 203, Ingredient = "jam", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 204, Ingredient = "jelly", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 205, Ingredient = "pumpkin puree", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1, IsLiquid = true },
                new IngredientUnit { Id = 206, Ingredient = "pumpkin", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 2 },
                new IngredientUnit { Id = 207, Ingredient = "crushed tomato", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 208, Ingredient = "tomato sauce", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1, IsLiquid = true },
                new IngredientUnit { Id = 209, Ingredient = "tomato paste", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 210, Ingredient = "chicken broth", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1, IsLiquid = true },
                new IngredientUnit { Id = 211, Ingredient = "beef broth", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 1, IsLiquid = true },
                new IngredientUnit { Id = 212, Ingredient = "broth", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 2, IsLiquid = true },
                new IngredientUnit { Id = 213, Ingredient = "vegan cheese", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 1 },
                new IngredientUnit { Id = 214, Ingredient = "cheese", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 2 },
                new IngredientUnit { Id = 215, Ingredient = "parmesan", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 2 },
                new IngredientUnit { Id = 216, Ingredient = "mozzarella", LargeImperialUnit = "lb", SmallImperialUnit = "oz", MetricUnit = "g", Pass = 2 },
                new IngredientUnit { Id = 217, Ingredient = "water", LargeImperialUnit = "gallon", SmallImperialUnit = "cup", MetricUnit = "ml", Pass = 2 },
                new IngredientUnit { Id = 218, Ingredient = "syrup", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 2 },
                new IngredientUnit { Id = 219, Ingredient = "oil", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 2, IsLiquid = true },
                new IngredientUnit { Id = 220, Ingredient = "sauce", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 2, IsLiquid = true },
                new IngredientUnit { Id = 221, Ingredient = "puree", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 2, IsLiquid = true },
                new IngredientUnit { Id = 222, Ingredient = "butter", LargeImperialUnit = "cup", SmallImperialUnit = "tbsp", MetricUnit = "ml", Pass = 2, IsLiquid = true },
                new IngredientUnit { Id = 223, Ingredient = "soup", LargeImperialUnit = "cup", SmallImperialUnit = "oz", MetricUnit = "ml", Pass = 1, IsLiquid = true }
            );




        }
    }
}
