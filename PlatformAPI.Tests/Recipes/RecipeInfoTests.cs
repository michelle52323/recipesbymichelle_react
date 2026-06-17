using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PlatformAPI.Data;
using PlatformAPI.DTO;
using PlatformAPI.Models.Recipe;
using Microsoft.Extensions.DependencyInjection;
using PlatformAPI.Tests;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using PlatformAPI.Tests.MockData.Recipes;
using PlatformAPI.Tests.MockData.UserRecipes;
using PlatformAPI.Controllers.Recipes;
using PlatformAPI.Enums;

namespace UnitTests
{
    public class RecipeInfoTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly AppDbContext _db;
        private readonly CustomWebApplicationFactory _factory;

        public RecipeInfoTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _db = factory.Services.GetRequiredService<AppDbContext>();

            // Reset DB
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
            _db.ChangeTracker.Clear();
        }

        [Fact]
        public async Task CreateRecipe_Should_Create_Recipe_And_UserRecipe()
        {
            // Arrange
            var userId = 123;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");

            // Build DTO for POST
            var dto = new CreateRecipeDto
            {
                Name = "Chocolate Cake",
                Description = "Rich and moist",
                ShowAbbreviations = false,
                RecipeVisibility = RecipeVisibility.MeOnly.ToString(),
                RecipeFont = RecipeFont.SansSerif.ToString()
            };

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/RecipeInfo/create-recipe", content);

            // Assert response
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseBody);

            ((bool)result.success).Should().BeTrue();
            int returnedRecipeId = (int)result.recipeId;

            // Assert DB: Recipe exists
            var recipe = await _db.Recipes.FirstOrDefaultAsync(r => r.Id == returnedRecipeId);
            recipe.Should().NotBeNull();

            recipe.Name.Should().Be("Chocolate Cake");
            recipe.Description.Should().Be("Rich and moist");
            recipe.ShowAbbreviations.Should().BeFalse();
            recipe.RecipeVisibility.Should().Be(RecipeVisibility.MeOnly);
            recipe.RecipeFont.Should().Be(RecipeFont.SansSerif);
            recipe.IsActive.Should().BeTrue();
            recipe.SortOrder.Should().Be(1); // first recipe for this user

            // Assert DB: UserRecipe link created
            var userRecipe = await _db.UserRecipes
                .FirstOrDefaultAsync(ur => ur.RecipeId == returnedRecipeId && ur.UserId == userId);

            userRecipe.Should().NotBeNull();
        }
    }
}
