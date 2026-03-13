using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.BeerBatches;
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
    public class BeerBatchesControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_data_when_batches_exist()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "Amber Ale", Description = "Malty" };
            await DbContext.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var batch = new BeerBatch { BeerSortId = beerSort.Id, Date = DateTime.Now, Description = "Autumn Batch #1" };
            await DbContext.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var url = "/api/BeerBatches/List?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<BeerBatch>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotEmpty(response.Value.Results);
        }

        [Fact]
        public async Task Save_should_add_new_beer_batch()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "IPA" };
            await DbContext.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var url = "/api/BeerBatches/Save";
            var command = new SaveBeerBatchCommand
            {
                Id = 0,
                BeerSortId = beerSort.Id,
                Date = DateTime.Now,
                Description = "New Batch"
            };

            // Act
            var response = await Client.PostAsJsonAsync(url, command);

            // Assert
            response.EnsureSuccessStatusCode();
            var saved = await DbContext.BeerBatches.FirstOrDefaultAsync(x => x.Description == "New Batch");
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task Delete_should_remove_existing_beer_batch()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "Experimental IPA" };
            await DbContext.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var batch = new BeerBatch { BeerSortId = beerSort.Id, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var url = "/api/BeerBatches/Delete";
            var command = new DeleteBeerBatchCommand { Id = batch.Id };
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var exists = await DbContext.BeerBatches.AnyAsync(x => x.Id == batch.Id);
            Assert.False(exists);
        }
    }
}
