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
    }
}