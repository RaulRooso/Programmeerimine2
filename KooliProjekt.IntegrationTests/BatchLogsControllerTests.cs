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
    public class BatchLogsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_data_when_logs_exist()
        {
            // Arrange: We need a User and a BeerBatch because BatchLog depends on them
            var user = new User { Username = "tester", Email = "test@test.com" };
            var beerSort = new BeerSort { Name = "Test Sort" };
            await DbContext.AddAsync(user);
            await DbContext.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var batch = new BeerBatch { BeerSortId = beerSort.Id, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var log = new BatchLog
            {
                BeerBatchId = batch.Id,
                UserId = user.Id,
                Date = DateTime.Now,
                Description = "Everything is bubbling nicely"
            };
            await DbContext.AddAsync(log);
            await DbContext.SaveChangesAsync();

            var url = "/api/BatchLogs/List?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<BatchLog>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotEmpty(response.Value.Results);
        }

        [Fact]
        public async Task Delete_should_return_ok_when_successful()
        {
            // Arrange
            var user = new User { Username = "deleter" };
            var beerSort = new BeerSort { Name = "Delete Sort" };
            await DbContext.AddAsync(user);
            await DbContext.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var batch = new BeerBatch { BeerSortId = beerSort.Id, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var log = new BatchLog { BeerBatchId = batch.Id, UserId = user.Id, Date = DateTime.Now };
            await DbContext.AddAsync(log);
            await DbContext.SaveChangesAsync();

            var url = $"/api/BatchLogs/Delete?Id={log.Id}";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var exists = await DbContext.BatchLogs.AnyAsync(x => x.Id == log.Id);
            Assert.False(exists);
        }
    }
}