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
    public class TasteLogsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_data_when_taste_logs_exist()
        {
            // Arrange
            var user = new User { Username = "beer_taster", Email = "taste@test.com" };
            var beerSort = new BeerSort { Name = "Saison", Description = "Spicy and fruity" };
            await DbContext.AddAsync(user);
            await DbContext.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var batch = new BeerBatch { BeerSortId = beerSort.Id, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var tasteLog = new TasteLog
            {
                BeerBatchId = batch.Id,
                UserId = user.Id,
                Date = DateTime.Now,
                Description = "Notes of coriander and orange peel",
                Rating = 5
            };
            await DbContext.AddAsync(tasteLog);
            await DbContext.SaveChangesAsync();

            var url = "/api/TasteLogs/List?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<TasteLog>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotEmpty(response.Value.Results);
        }

        [Fact]
        public async Task Delete_should_remove_taste_log_from_database()
        {
            // Arrange
            var user = new User { Username = "critic" };
            var beerSort = new BeerSort { Name = "Sour Ale" };
            await DbContext.AddAsync(user);
            await DbContext.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var batch = new BeerBatch { BeerSortId = beerSort.Id, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var tasteLog = new TasteLog { BeerBatchId = batch.Id, UserId = user.Id, Date = DateTime.Now, Rating = 3 };
            await DbContext.AddAsync(tasteLog);
            await DbContext.SaveChangesAsync();

            var url = $"/api/TasteLogs/Delete?Id={tasteLog.Id}";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var exists = await DbContext.TasteLogs.AnyAsync(x => x.Id == tasteLog.Id);
            Assert.False(exists);
        }
    }
}
