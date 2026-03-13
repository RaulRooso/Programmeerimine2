using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.BatchLogs;
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
    public class BatchLogsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_data_when_logs_exist()
        {
            // Arrange
            var user = new User { Username = "tester", Email = "test@test.com" };
            var beerSort = new BeerSort { Name = "Test Sort" };
            await DbContext.AddAsync(user);
            await DbContext.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var batch = new BeerBatch { BeerSortId = beerSort.Id, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var log = new BatchLog { BeerBatchId = batch.Id, UserId = user.Id, Date = DateTime.Now, Description = "Bubbling" };
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
        public async Task Save_should_add_new_batch_log()
        {
            // Arrange
            var user = new User { Username = "logger" };
            var beerSort = new BeerSort { Name = "Test Sort" };
            await DbContext.AddAsync(user);
            await DbContext.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();
            var batch = new BeerBatch { BeerSortId = beerSort.Id, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var url = "/api/BatchLogs/Save";
            var command = new SaveBatchLogCommand
            {
                Id = 0,
                BeerBatchId = batch.Id,
                UserId = user.Id,
                Date = DateTime.Now,
                Description = "New Log"
            };

            // Act
            var response = await Client.PostAsJsonAsync(url, command);

            // Assert
            response.EnsureSuccessStatusCode();
            var saved = await DbContext.BatchLogs.FirstOrDefaultAsync(x => x.Description == "New Log");
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task Delete_should_remove_existing_batch_log()
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

            var url = "/api/BatchLogs/Delete";
            var command = new DeleteBatchLogCommand { Id = log.Id };
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var exists = await DbContext.BatchLogs.AnyAsync(x => x.Id == log.Id);
            Assert.False(exists);
        }
    }
}
