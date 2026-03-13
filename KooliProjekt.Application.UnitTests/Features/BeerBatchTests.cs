using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.BeerBatches;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class BeerBatchTests : TestBase
    {
        // === GET TESTS ===

        [Fact]
        public async Task Get_should_return_existing_beer_batch_with_details()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "Stout" };
            await DbContext.BeerSorts.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var batch = new BeerBatch
            {
                Date = DateTime.Now,
                Description = "Batch with items",
                BeerSortId = beerSort.Id
            };

            // Add a child ingredient to test the "Include/Select" logic
            batch.Ingredients.Add(new Ingredient { Name = "Malt", Unit = "kg", Quantity = 5 });

            await DbContext.BeerBatches.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var query = new GetBeerBatchQuery { Id = batch.Id };
            var handler = new GetBeerBatchQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal("Batch with items", result.Value.Description);
            Assert.Single(result.Value.Ingredients);
            Assert.Equal("Malt", result.Value.Ingredients[0].Name);
        }

        // === LIST TESTS ===

        [Fact]
        public async Task List_should_return_paged_beer_batches()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "Test Sort" };
            await DbContext.BeerSorts.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var query = new ListBeerBatchesQuery { Page = 1, PageSize = 3 };
            var handler = new ListBeerBatchesQueryHandler(DbContext);

            for (int i = 1; i <= 5; i++)
            {
                await DbContext.BeerBatches.AddAsync(new BeerBatch
                {
                    Date = DateTime.Now,
                    BeerSortId = beerSort.Id
                });
            }
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(3, result.Value.Results.Count);
            Assert.Equal(5, result.Value.RowCount);
        }

        [Fact]
        public async Task List_should_filter_by_description_when_search_term_is_provided()
        {
            // Arrange
            var sort = new BeerSort { Name = "Test Sort" };
            await DbContext.BeerSorts.AddAsync(sort);
            await DbContext.BeerBatches.AddAsync(new BeerBatch { Description = "First Batch", BeerSort = sort, Date = DateTime.Now });
            await DbContext.BeerBatches.AddAsync(new BeerBatch { Description = "Target Batch", BeerSort = sort, Date = DateTime.Now });
            await DbContext.SaveChangesAsync();

            var query = new ListBeerBatchesQuery { Page = 1, PageSize = 10, Description = "Target" };
            var handler = new ListBeerBatchesQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("Target Batch", result.Value.Results.First().Description);
        }

        [Fact]
        public async Task List_should_return_empty_result_if_pagination_is_invalid()
        {
            // Fixes the early return branch for Page/PageSize <= 0
            var query = new ListBeerBatchesQuery { Page = 0, PageSize = 10 };
            var handler = new ListBeerBatchesQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.Null(result.Value);
            Assert.False(result.HasErrors);
        }

        // === DELETE TESTS ===

        [Fact]
        public async Task Delete_should_remove_batch_and_cascade_logic()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "Test Sort" };
            await DbContext.BeerSorts.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var batch = new BeerBatch { Date = DateTime.Now, BeerSortId = beerSort.Id };
            await DbContext.BeerBatches.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var command = new DeleteBeerBatchCommand { Id = batch.Id };
            var handler = new DeleteBeerBatchCommandHandler(DbContext);

            // Act
            await handler.Handle(command, CancellationToken.None);
            var exists = await DbContext.BeerBatches.AnyAsync(x => x.Id == batch.Id);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task Delete_should_return_early_if_batch_does_not_exist()
        {
            // Arrange
            var command = new DeleteBeerBatchCommand { Id = 9999 };
            var handler = new DeleteBeerBatchCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.HasErrors);
        }

        // === SAVE TESTS ===

        [Fact]
        public async Task Save_should_add_new_beer_batch()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "Test Sort" };
            await DbContext.BeerSorts.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var command = new SaveBeerBatchCommand
            {
                Id = 0,
                Date = DateTime.Now,
                Description = "New Batch",
                BeerSortId = beerSort.Id
            };
            var handler = new SaveBeerBatchCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var saved = await DbContext.BeerBatches.FirstOrDefaultAsync(x => x.Description == "New Batch");

            // Assert
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(beerSort.Id, saved.BeerSortId);
        }

        [Fact]
        public async Task Save_should_execute_update_logic_when_id_is_not_zero()
        {
            // 1. Arrange: Put a batch in the DB so we have something to find
            var beerSort = new BeerSort { Name = "Stout" };
            await DbContext.BeerSorts.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var existingBatch = new BeerBatch { BeerSortId = beerSort.Id, Date = DateTime.Now };
            await DbContext.BeerBatches.AddAsync(existingBatch);
            await DbContext.SaveChangesAsync();

            // 2. Setup the Command with the existing ID
            var command = new SaveBeerBatchCommand
            {
                Id = existingBatch.Id, // This triggers the 'else' block
                BeerSortId = beerSort.Id,
                Date = DateTime.Now,
                Description = "Updated Description"
            };
            var handler = new SaveBeerBatchCommandHandler(DbContext);

            // 3. Act
            await handler.Handle(command, CancellationToken.None);

            // 4. Assert
            var updated = await DbContext.BeerBatches.FindAsync(existingBatch.Id);
            Assert.Equal("Updated Description", updated.Description);
        }
    }
}