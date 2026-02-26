using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.BeerSorts; // For your Query/Command classes
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class BeerSortsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_success_and_data()
        {
            // Arrange
            // We add a record directly to the DB so the list isn't empty
            var beerSort = new BeerSort { Name = "IPA", Description = "Hoppy" };
            await DbContext.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var url = "/api/BeerSorts/List?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<BeerSort>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotEmpty(response.Value.Results);
        }

        [Fact]
        public async Task Delete_should_remove_existing_item()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "Stout", Description = "Dark" };
            await DbContext.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            // The URL matches your [HttpDelete] [Route("Delete")]
            var url = $"/api/BeerSorts/Delete?Id={beerSort.Id}";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Verify it's actually gone from the database
            var exists = await DbContext.BeerSorts.AnyAsync(x => x.Id == beerSort.Id);
            Assert.False(exists);
        }
    }
}