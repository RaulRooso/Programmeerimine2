using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Ingredients;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class IngredientTests : TestBase
    {
        // Helper to setup the parent Batch
        private async Task<int> SetupParentBatch()
        {
            var beerSort = new BeerSort { Name = "Test Sort" };
            await DbContext.BeerSorts.AddAsync(beerSort);

            var batch = new BeerBatch { Date = DateTime.Now, BeerSortId = beerSort.Id };
            await DbContext.BeerBatches.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            return batch.Id;
        }

        // === GET TESTS ===

        [Fact]
        public async Task Get_should_return_existing_ingredient_dto()
        {
            // Arrange
            var batchId = await SetupParentBatch();
            var ingredient = new Ingredient
            {
                Name = "Cascade Hops",
                Unit = "g",
                Quantity = 50,
                UnitPrice = 0.1m,
                BeerBatchId = batchId
            };
            await DbContext.Ingredients.AddAsync(ingredient);
            await DbContext.SaveChangesAsync();

            var query = new GetIngredientQuery { Id = ingredient.Id };
            var handler = new GetIngredientQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal("Cascade Hops", result.Value.Name);
            Assert.Equal(50, result.Value.Quantity);
        }

        // === LIST TESTS ===

        [Fact]
        public async Task List_should_return_paged_ingredients()
        {
            // Arrange
            var batchId = await SetupParentBatch();
            var query = new ListIngredientsQuery { Page = 1, PageSize = 5 };
            var handler = new ListIngredientsQueryHandler(DbContext);

            for (int i = 1; i <= 10; i++)
            {
                await DbContext.Ingredients.AddAsync(new Ingredient
                {
                    Name = $"Ingredient {i}",
                    Unit = "pcs",
                    Quantity = i,
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
        public async Task List_should_filter_by_name_when_search_term_is_provided()
        {
            // Arrange
            var batchId = await SetupParentBatch(); // Use existing helper
            await DbContext.Ingredients.AddAsync(new Ingredient { Name = "Hops", Unit = "g", BeerBatchId = batchId });
            await DbContext.Ingredients.AddAsync(new Ingredient { Name = "Malt", Unit = "kg", BeerBatchId = batchId });
            await DbContext.SaveChangesAsync();

            var query = new ListIngredientsQuery { Page = 1, PageSize = 10, Name = "Hops" };
            var handler = new ListIngredientsQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("Hops", result.Value.Results.First().Name);
        }


        [Fact]
        public async Task List_should_return_empty_result_if_pagination_is_invalid()
        {
            // Fixes: if (request.Page <= 0 || request.PageSize <= 0)
            var query = new ListIngredientsQuery { Page = 0, PageSize = 10 };
            var handler = new ListIngredientsQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.Null(result.Value);
            Assert.False(result.HasErrors);
        }

        //[Fact]
        //public async Task List_should_not_filter_if_name_is_null_or_empty()
        //{
        //    // Fixes the "False" branch of the string.IsNullOrEmpty(request.Name) check
        //    var batchId = await SetupParentBatch();
        //    await DbContext.Ingredients.AddAsync(new Ingredient { Name = "Water", Unit = "L", BeerBatchId = batchId });
        //    await DbContext.SaveChangesAsync();

        //    var query = new ListIngredientsQuery { Page = 1, PageSize = 10, Name = "" };
        //    var handler = new ListIngredientsQueryHandler(DbContext);

        //    var result = await handler.Handle(query, CancellationToken.None);

        //    Assert.NotNull(result.Value);
        //    Assert.NotEmpty(result.Value.Results);
        //}

        // === DELETE TESTS ===

        [Fact]
        public async Task Delete_should_remove_ingredient_from_database()
        {
            // Arrange
            var batchId = await SetupParentBatch();
            var ingredient = new Ingredient { Name = "Delete Me", Unit = "x", BeerBatchId = batchId };
            await DbContext.Ingredients.AddAsync(ingredient);
            await DbContext.SaveChangesAsync();

            var command = new DeleteIngredientCommand { Id = ingredient.Id };
            var handler = new DeleteIngredientCommandHandler(DbContext);

            // Act
            await handler.Handle(command, CancellationToken.None);
            var exists = await DbContext.Ingredients.AnyAsync(x => x.Id == ingredient.Id);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task Delete_should_throw_exception_if_request_is_null()
        {
            // Fixes: if (request == null)
            var handler = new DeleteIngredientCommandHandler(DbContext);

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }

        [Fact]
        public async Task Delete_should_return_early_if_id_is_invalid()
        {
            // Fixes: if (request.Id <= 0)
            var command = new DeleteIngredientCommand { Id = 0 };
            var handler = new DeleteIngredientCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_return_early_if_item_not_found()
        {
            // Fixes: if (item == null)
            var command = new DeleteIngredientCommand { Id = 9999 };
            var handler = new DeleteIngredientCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.HasErrors);
        }

        // === SAVE TESTS ===

        [Fact]
        public async Task Save_should_add_new_ingredient()
        {
            // Arrange
            var batchId = await SetupParentBatch();
            var command = new SaveIngredientCommand
            {
                Id = 0,
                Name = "Yeast",
                Unit = "pkg",
                Quantity = 1,
                BeerBatchId = batchId
            };
            var handler = new SaveIngredientCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var saved = await DbContext.Ingredients.FirstOrDefaultAsync(x => x.Name == "Yeast");

            // Assert
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(batchId, saved.BeerBatchId);
        }

        [Fact]
        public async Task Save_should_update_existing_ingredient()
        {
            // Arrange
            var batchId = await SetupParentBatch();

            // Create the initial ingredient to be updated
            var existing = new Ingredient
            {
                Name = "Old Malt",
                Unit = "kg",
                Quantity = 10,
                BeerBatchId = batchId
            };
            await DbContext.Ingredients.AddAsync(existing);
            await DbContext.SaveChangesAsync();

            var command = new SaveIngredientCommand
            {
                Id = existing.Id, // This triggers the 'else' block!
                Name = "New Malt",
                Unit = "kg",
                Quantity = 15,
                UnitPrice = 2.5m,
                BeerBatchId = batchId
            };
            var handler = new SaveIngredientCommandHandler(DbContext);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            DbContext.ChangeTracker.Clear(); // Clear cache to force a fresh DB read
            var updated = await DbContext.Ingredients.FindAsync(existing.Id);

            Assert.NotNull(updated);
            Assert.Equal("New Malt", updated.Name);
            Assert.Equal(15, updated.Quantity);
        }
    }
}