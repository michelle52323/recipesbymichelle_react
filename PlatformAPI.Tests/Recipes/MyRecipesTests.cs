using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PlatformAPI.Data;
using PlatformAPI.DTO;
using PlatformAPI.Models.Recipe;
using Microsoft.Extensions.DependencyInjection;
using PlatformAPI.Tests;
using Microsoft.AspNetCore.Mvc.Testing;
using PlatformAPI.Tests.MockData.Recipes;
using PlatformAPI.Tests.MockData.UserRecipes;
using System.Net.Http.Headers;
using PlatformAPI.Controllers.Recipes;

namespace UnitTests
{
    public class MyRecipesControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly AppDbContext _db;
        private readonly CustomWebApplicationFactory _factory;

        public MyRecipesControllerTests(CustomWebApplicationFactory factory)
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
        public async Task GetRecipes_ReturnsRecipesForAuthenticatedUser()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");

            // 1. Recipes
            _db.Recipes.AddRange(RecipeFactory.CreateList());

            // 2. UserRecipe links
            _db.UserRecipes.AddRange(UserRecipeFactory.CreateList());

            await _db.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/MyRecipes/getRecipes");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var recipes = await response.Content.ReadFromJsonAsync<List<MyRecipesDto>>();

            recipes.Should().NotBeNull();
            recipes.Should().HaveCount(3); // IDs 1, 2, 5

            recipes.Select(r => r.Id).Should().BeEquivalentTo(new[] { 1, 2, 5 });

            recipes.Should().ContainSingle(r => r.Name == "Active Recipe 1");
            recipes.Should().ContainSingle(r => r.Name == "Active Recipe 2");
            recipes.Should().ContainSingle(r => r.Name == "Active Recipe 3");

            recipes.Should().BeInAscendingOrder(r => r.SortOrder);

            recipes.Should().SatisfyRespectively(
                first =>
                {
                    first.Id.Should().Be(2);
                },
                second =>
                {
                    second.Id.Should().Be(1);
                },
                third =>
                {
                    third.Id.Should().Be(5);
                }
            );
        }

        [Fact]
        public async Task GetRecipes_WhenRecipesBelongToOtherUser_ReturnsEmptyList()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");

            // Arrange
            var otherUserRecipe = RecipeFactory.Create(id: 99, name: "Other User Recipe", userId: 999);
            _db.Recipes.Add(otherUserRecipe);

            _db.UserRecipes.Add(UserRecipeFactory.Create(userId: 999, recipeId: 99));

            await _db.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/MyRecipes/getRecipes");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var recipes = await response.Content.ReadFromJsonAsync<List<MyRecipesDto>>();

            recipes.Should().NotBeNull();
            recipes.Should().BeEmpty();
        }

        [Fact]
        public async Task GetRecipes_WhenRecipeIsInactive_ReturnsEmptyList()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");

            // Arrange
            var inactiveRecipe = RecipeFactory.Create(
                id: 10,
                name: "Inactive Recipe",
                isActive: false,
                userId: 123
            );

            _db.Recipes.Add(inactiveRecipe);
            _db.UserRecipes.Add(UserRecipeFactory.Create(userId: 123, recipeId: 10));

            await _db.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/MyRecipes/getRecipes");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var recipes = await response.Content.ReadFromJsonAsync<List<MyRecipesDto>>();

            recipes.Should().NotBeNull();
            recipes.Should().BeEmpty();
        }

        [Fact]
        public async Task GetRecipes_WhenUnauthenticated_Returns401Unauthorized()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = null;

            var response = await client.GetAsync("/api/MyRecipes/getRecipes");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
