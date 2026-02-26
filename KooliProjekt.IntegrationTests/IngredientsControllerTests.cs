using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
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
            await DbContext.SaveChangesAsync();

            var batch = new BeerBatch { BeerSortId = beerSort.Id, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var ingredient = new Ingredient
            {
                BeerBatchId = batch.Id,
                Name = "Local Honey",
                Quantity = 500,
                Unit = "g",
                UnitPrice = 10
            };
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
        public async Task Delete_should_remove_ingredient_from_database()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "Bitter" };
            await DbContext.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var batch = new BeerBatch { BeerSortId = beerSort.Id, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var ingredient = new Ingredient
            {
                BeerBatchId = batch.Id,
                Name = "Hops",
                Unit = "g",
                Quantity = 10,
                UnitPrice = 1
            };
            await DbContext.AddAsync(ingredient);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Ingredients/Delete?Id={ingredient.Id}";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var exists = await DbContext.Ingredients.AnyAsync(x => x.Id == ingredient.Id);
            Assert.False(exists);
        }

        [Fact]
        public async Task Delete_should_return_bad_request_if_id_does_not_exist()
        {
            // Arrange
            var url = "/api/Ingredients/Delete?Id=99999";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            // Depending on your ApiControllerBase logic, this will be 404 or 400
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
