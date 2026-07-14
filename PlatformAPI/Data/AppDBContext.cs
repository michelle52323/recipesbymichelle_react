using Microsoft.EntityFrameworkCore;

using PlatformAPI.Models.Themes;
using PlatformAPI.Models.Users;
using PlatformAPI.Models.Recipe;
using System;

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
                            property.SetMaxLength(30); // Override: Color fields get length 7
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


            //Set complex primary keys
            modelBuilder.Entity<ThemeVariableColor>()
                .HasKey(tv => new { tv.ThemeId, tv.ThemeVariableId });

            modelBuilder.Entity<UserRecipe>()
                .HasKey(ur => new { ur.UserId, ur.RecipeId });

            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            modelBuilder.Entity<RecipeStep>()
                .HasKey(rs => new { rs.RecipeId, rs.StepId });



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
        }
    }
}
