using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Ingredients;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class IngredientsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_data_when_ingredients_exist()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "Honey Lager" };
            await DbContext.AddAsync(beerSort);
            var batch = new BeerBatch { BeerSortId = beerSort.Id, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var ingredient = new Ingredient { BeerBatchId = batch.Id, Name = "Local Honey", Quantity = 500, Unit = "g", UnitPrice = 10 };
            await DbContext.AddAsync(ingredient);
            await DbContext.SaveChangesAsync();

            var url = "/api/Ingredients/List?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<Ingredient>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotEmpty(response.Value.Results);
        }

        [Fact]
        public async Task Save_should_add_new_ingredient()
        {
            // Arrange
            var url = "/api/Ingredients/Save";
            var beerSort = new BeerSort { Name = "IPA" };
            await DbContext.AddAsync(beerSort);
            var batch = new BeerBatch { BeerSortId = beerSort.Id, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var command = new SaveIngredientCommand
            {
                Id = 0,
                Name = "Cascade Hops",
                Unit = "g",
                Quantity = 50,
                UnitPrice = 0.5m,
                BeerBatchId = batch.Id
            };

            // Act
            var response = await Client.PostAsJsonAsync(url, command);

            // Assert
            response.EnsureSuccessStatusCode();
            var saved = await DbContext.Ingredients.FirstOrDefaultAsync(x => x.Name == "Cascade Hops");
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task Save_should_update_existing_ingredient()
        {
            // Arrange
            var url = "/api/Ingredients/Save";
            var beerSort = new BeerSort { Name = "Stout" };
            await DbContext.AddAsync(beerSort);
            var batch = new BeerBatch { BeerSortId = beerSort.Id, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            var ingredient = new Ingredient { Name = "Old Name", BeerBatch = batch, Unit = "g", Quantity = 10 };
            await DbContext.AddAsync(ingredient);
            await DbContext.SaveChangesAsync();

            var command = new SaveIngredientCommand
            {
                Id = ingredient.Id,
                Name = "Updated Name",
                Unit = "g",
                Quantity = 20,
                BeerBatchId = batch.Id
            };

            // Act
            var response = await Client.PostAsJsonAsync(url, command);

            // Assert
            response.EnsureSuccessStatusCode();
            var updated = await DbContext.Ingredients.FindAsync(ingredient.Id);
            Assert.Equal("Updated Name", updated.Name);
            Assert.Equal(20, updated.Quantity);
        }

        [Fact]
        public async Task Delete_should_remove_existing_ingredient()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "Porter" };
            await DbContext.AddAsync(beerSort);
            var batch = new BeerBatch { BeerSort = beerSort, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            var ingredient = new Ingredient { Name = "Chocolate Malt", BeerBatch = batch, Unit = "kg", Quantity = 1 };
            await DbContext.AddAsync(ingredient);
            await DbContext.SaveChangesAsync();

            var url = "/api/Ingredients/Delete";
            var command = new DeleteIngredientCommand { Id = ingredient.Id };
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var exists = await DbContext.Ingredients.AnyAsync(x => x.Id == ingredient.Id);
            Assert.False(exists);
        }
    }
}
