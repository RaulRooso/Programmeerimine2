using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.TasteLogs;
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
    }
}