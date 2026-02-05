using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.BatchLogs;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class BatchLogTests : TestBase
    {
        // Helper method to setup required relations
        private async Task<(int UserId, int BatchId)> SetupRelations()
        {
            var user = new User { Username = "testuser" };
            var beerSort = new BeerSort { Name = "Test Sort" };
            await DbContext.Users.AddAsync(user);
            await DbContext.BeerSorts.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var batch = new BeerBatch { Date = DateTime.Now, BeerSortId = beerSort.Id };
            await DbContext.BeerBatches.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            return (user.Id, batch.Id);
        }

        // === GET TESTS ===

        [Fact]
        public async Task Get_should_return_existing_batch_log_dto()
        {
            // Arrange
            var (userId, batchId) = await SetupRelations();
            var log = new BatchLog
            {
                Date = DateTime.Now,
                Description = "Mashed at 65C",
                UserId = userId,
                BeerBatchId = batchId
            };
            await DbContext.BatchLogs.AddAsync(log);
            await DbContext.SaveChangesAsync();

            var query = new GetBatchLogQuery { Id = log.Id };
            var handler = new GetBatchLogQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal("Mashed at 65C", result.Value.Description);
        }

        // === LIST TESTS ===

        [Fact]
        public async Task List_should_return_paged_batch_logs()
        {
            // Arrange
            var (userId, batchId) = await SetupRelations();
            var query = new ListBatchLogsQuery { Page = 1, PageSize = 5 };
            var handler = new ListBatchLogsQueryHandler(DbContext);

            for (int i = 1; i <= 10; i++)
            {
                await DbContext.BatchLogs.AddAsync(new BatchLog
                {
                    Date = DateTime.Now,
                    Description = $"Log {i}",
                    UserId = userId,
                    BeerBatchId = batchId
                });
            }
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(5, result.Value.Results.Count);
        }

        // === DELETE TESTS ===

        [Fact]
        public async Task Delete_should_remove_batch_log_from_database()
        {
            // Arrange
            var (userId, batchId) = await SetupRelations();
            var log = new BatchLog { Date = DateTime.Now, UserId = userId, BeerBatchId = batchId };
            await DbContext.BatchLogs.AddAsync(log);
            await DbContext.SaveChangesAsync();

            var command = new DeleteBatchLogCommand { Id = log.Id };
            var handler = new DeleteBatchLogCommandHandler(DbContext);

            // Act
            await handler.Handle(command, CancellationToken.None);
            var exists = await DbContext.BatchLogs.AnyAsync(x => x.Id == log.Id);

            // Assert
            Assert.False(exists);
        }

        // === SAVE TESTS ===

        [Fact]
        public async Task Save_should_add_new_batch_log()
        {
            // Arrange
            var (userId, batchId) = await SetupRelations();
            var command = new SaveBatchLogCommand
            {
                Id = 0,
                Date = DateTime.Now,
                Description = "New Log",
                UserId = userId,
                BeerBatchId = batchId
            };
            var handler = new SaveBatchLogCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var saved = await DbContext.BatchLogs.FirstOrDefaultAsync(x => x.Description == "New Log");

            // Assert
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
        }
    }
}