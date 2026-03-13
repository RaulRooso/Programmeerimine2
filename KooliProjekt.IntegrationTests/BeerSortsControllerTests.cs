using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.BeerSorts;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http;
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
        public async Task Save_should_add_new_beer_sort()
        {
            // Arrange
            var url = "/api/BeerSorts/Save";
            var command = new SaveBeerSortCommand
            {
                Id = 0,
                Name = "Lager",
                Description = "Crisp"
            };

            // Act
            var response = await Client.PostAsJsonAsync(url, command);

            // Assert
            response.EnsureSuccessStatusCode();
            var saved = await DbContext.BeerSorts.FirstOrDefaultAsync(x => x.Name == "Lager");
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task Delete_should_remove_existing_item()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "Stout", Description = "Dark" };
            await DbContext.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var url = "/api/BeerSorts/Delete";
            var command = new DeleteBeerSortCommand { Id = beerSort.Id };
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var exists = await DbContext.BeerSorts.AnyAsync(x => x.Id == beerSort.Id);
            Assert.False(exists);
        }
    }
}
