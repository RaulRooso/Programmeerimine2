using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.TasteLogs;
using KooliProjekt.Application.Features.Users;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class TasteLogTests : TestBase
    {
        // Helper to setup required relations (User and Batch)
        private async Task<(int UserId, int BatchId)> SetupRelations()
        {
            var user = new User { Username = "taster1" };
            var beerSort = new BeerSort { Name = "IPA" };
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
        public async Task Get_should_return_existing_taste_log_dto()
        {
            // Arrange
            var (userId, batchId) = await SetupRelations();
            var log = new TasteLog
            {
                Date = DateTime.Now,
                Description = "Tastes like pine",
                Rating = 8,
                UserId = userId,
                BeerBatchId = batchId
            };
            await DbContext.TasteLogs.AddAsync(log);
            await DbContext.SaveChangesAsync();

            var query = new GetTasteLogQuery { Id = log.Id };
            var handler = new GetTasteLogQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(8, result.Value.Rating);
            Assert.Equal("Tastes like pine", result.Value.Description);
        }

        // === LIST TESTS ===

        [Fact]
        public async Task List_should_return_paged_taste_logs()
        {
            // Arrange
            var (userId, batchId) = await SetupRelations();
            var query = new ListTasteLogsQuery { Page = 1, PageSize = 3 };
            var handler = new ListTasteLogsQueryHandler(DbContext);

            for (int i = 1; i <= 5; i++)
            {
                await DbContext.TasteLogs.AddAsync(new TasteLog
                {
                    Date = DateTime.Now,
                    Rating = i,
                    UserId = userId,
                    BeerBatchId = batchId
                });
            }
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(3, result.Value.Results.Count);
        }

        //[Fact]
        //public async Task List_should_throw_exception_if_request_is_null()
        //{
        //    // Fixes the ArgumentNullException line
        //    var handler = new ListTasteLogsQueryHandler(DbContext);
        //    await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        //}

        [Fact]
        public async Task List_should_return_empty_result_if_pagination_is_invalid()
        {
            // Fixes: if (request.Page <= 0 || request.PageSize <= 0)
            var query = new ListTasteLogsQuery { Page = 0, PageSize = 10 };
            var handler = new ListTasteLogsQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.Null(result.Value);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task List_should_filter_by_description_when_search_term_is_provided()
        {
            // Arrange
            var (userId, batchId) = await SetupRelations();
            await DbContext.TasteLogs.AddAsync(new TasteLog
            {
                Description = "Very hoppy",
                UserId = userId,
                BeerBatchId = batchId,
                Date = DateTime.Now
            });
            await DbContext.TasteLogs.AddAsync(new TasteLog
            {
                Description = "Sweet malty",
                UserId = userId,
                BeerBatchId = batchId,
                Date = DateTime.Now
            });
            await DbContext.SaveChangesAsync();

            var query = new ListTasteLogsQuery
            {
                Page = 1,
                PageSize = 10,
                Description = "hoppy" // This triggers the code inside the 'if'
            };
            var handler = new ListTasteLogsQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Contains("hoppy", result.Value.Results.First().Description);
        }

        // === DELETE TESTS ===

        [Fact]
        public async Task Delete_should_remove_taste_log_from_database()
        {
            // Arrange
            var (userId, batchId) = await SetupRelations();
            var log = new TasteLog { Date = DateTime.Now, Rating = 5, UserId = userId, BeerBatchId = batchId };
            await DbContext.TasteLogs.AddAsync(log);
            await DbContext.SaveChangesAsync();

            var command = new DeleteTasteLogCommand { Id = log.Id };
            var handler = new DeleteTasteLogCommandHandler(DbContext);

            // Act
            await handler.Handle(command, CancellationToken.None);
            var exists = await DbContext.TasteLogs.AnyAsync(x => x.Id == log.Id);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task Delete_should_throw_exception_if_request_is_null()
        {
            // Covers: if (request == null)
            var handler = new DeleteTasteLogCommandHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Fact]
        public async Task Delete_should_return_early_if_id_is_invalid()
        {
            // Covers: if (request.Id <= 0)
            var command = new DeleteTasteLogCommand { Id = 0 };
            var handler = new DeleteTasteLogCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_return_early_if_item_not_found()
        {
            // Covers: if (item == null)
            var command = new DeleteTasteLogCommand { Id = 999 };
            var handler = new DeleteTasteLogCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.HasErrors);
        }

        // === SAVE TESTS ===

        [Fact]
        public async Task Save_should_add_new_taste_log()
        {
            // Arrange
            var (userId, batchId) = await SetupRelations();
            var command = new SaveTasteLogCommand
            {
                Id = 0,
                Date = DateTime.Now,
                Description = "Excellent",
                Rating = 10,
                UserId = userId,
                BeerBatchId = batchId
            };
            var handler = new SaveTasteLogCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var saved = await DbContext.TasteLogs.FirstOrDefaultAsync(x => x.Rating == 10);

            // Assert
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task Save_should_update_existing_taste_log()
        {
            // Arrange: 1. Setup the basic relations
            var (userId, batchId) = await SetupRelations();

            // 2. Create an actual record in the DB to update
            var existingLog = new TasteLog
            {
                Description = "Old Description",
                Rating = 1,
                UserId = userId,
                BeerBatchId = batchId,
                Date = DateTime.Now.AddDays(-1)
            };
            await DbContext.TasteLogs.AddAsync(existingLog);
            await DbContext.SaveChangesAsync();

            // 3. Prepare the command with the EXISTING ID to trigger the update path
            var command = new SaveTasteLogCommand
            {
                Id = existingLog.Id, // <--- This is the key to hitting the 'else' branch!
                Date = DateTime.Now,
                Description = "Updated Description",
                Rating = 9,
                UserId = userId,
                BeerBatchId = batchId
            };
            var handler = new SaveTasteLogCommandHandler(DbContext);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            DbContext.ChangeTracker.Clear();
            var updated = await DbContext.TasteLogs.FindAsync(existingLog.Id);

            Assert.NotNull(updated);
            Assert.Equal("Updated Description", updated.Description);
            Assert.Equal(9, updated.Rating);
        }
    }
}