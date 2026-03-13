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

        [Fact]
        public async Task Get_should_throw_exception_if_request_is_null()
        {
            // Arrange
            var handler = new GetBatchLogQueryHandler(DbContext);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }

        [Fact]
        public async Task Get_should_return_empty_result_if_id_is_invalid()
        {
            // Arrange
            var query = new GetBatchLogQuery { Id = 0 }; // Triggers Id <= 0
            var handler = new GetBatchLogQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result.Value);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_null_value_if_item_does_not_exist()
        {
            // Arrange
            var query = new GetBatchLogQuery { Id = 999 }; // Positive ID but not in DB
            var handler = new GetBatchLogQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result.Value);
            Assert.False(result.HasErrors);
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

        [Fact]
        public async Task List_should_filter_by_description_when_search_term_is_provided()
        {
            // Arrange
            var relations = await SetupRelations(); // Use your log setup helper
            await DbContext.BatchLogs.AddAsync(new BatchLog { Description = "Mash started", BeerBatchId = relations.BatchId });
            await DbContext.BatchLogs.AddAsync(new BatchLog { Description = "Boil started", BeerBatchId = relations.BatchId });
            await DbContext.SaveChangesAsync();

            var query = new ListBatchLogsQuery { Page = 1, PageSize = 10, Description = "Mash" };
            var handler = new ListBatchLogsQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Contains("Mash", result.Value.Results.First().Description);
        }

        [Fact]
        public async Task List_should_throw_exception_if_request_is_null()
        {
            // Fixes the Red/Yellow on line 18
            var handler = new ListBatchLogsQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Fact]
        public async Task List_should_return_empty_result_if_pagination_is_invalid()
        {
            // Fixes line 23 & 24 (The "return result" branch)
            var query = new ListBatchLogsQuery { Page = 0, PageSize = 10 };
            var handler = new ListBatchLogsQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_not_filter_if_description_is_null_or_empty()
        {
            // Fixes line 27 (Tests the "False" path of the If statement)
            var (userId, batchId) = await SetupRelations();
            await DbContext.BatchLogs.AddAsync(new BatchLog { Description = "Log A", BeerBatchId = batchId });
            await DbContext.SaveChangesAsync();

            var query = new ListBatchLogsQuery { Page = 1, PageSize = 10, Description = "" };
            var handler = new ListBatchLogsQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            // Should return all logs because filter was skipped
            Assert.NotEmpty(result.Value.Results);
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

        [Fact]
        public async Task Delete_should_return_early_if_id_is_invalid()
        {
            // Arrange
            var command = new DeleteBatchLogCommand { Id = 0 };
            var handler = new DeleteBatchLogCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_return_early_if_item_does_not_exist()
        {
            // Arrange
            var command = new DeleteBatchLogCommand { Id = 99999 }; // Triggers item == null
            var handler = new DeleteBatchLogCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_throw_exception_if_request_is_null()
        {
            // Arrange
            var handler = new DeleteBatchLogCommandHandler(DbContext);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
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

        [Fact]
        public async Task Save_should_update_existing_batch_log()
        {
            // Arrange
            var (userId, batchId) = await SetupRelations();

            // Create an initial log to update
            var existingLog = new BatchLog
            {
                Date = DateTime.Now.AddDays(-1),
                Description = "Old Description",
                UserId = userId,
                BeerBatchId = batchId
            };
            await DbContext.BatchLogs.AddAsync(existingLog);
            await DbContext.SaveChangesAsync();

            var command = new SaveBatchLogCommand
            {
                Id = existingLog.Id, // Non-zero ID triggers the Update path
                Date = DateTime.Now,
                Description = "Updated Description",
                UserId = userId,
                BeerBatchId = batchId
            };
            var handler = new SaveBatchLogCommandHandler(DbContext);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Clear tracker to ensure we fetch fresh data from DB
            DbContext.ChangeTracker.Clear();
            var updated = await DbContext.BatchLogs.FindAsync(existingLog.Id);

            // Assert
            Assert.NotNull(updated);
            Assert.Equal("Updated Description", updated.Description);
        }
    }
}
