using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Ingredients;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class IngredientsTests : TestBase
    {
        [Fact]
        public void Constructor_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListIngredientsQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_paginated_ingredients()
        {
            // Arrange
            var query = new ListIngredientsQuery { Page = 1, PageSize = 2 };
            var handler = new ListIngredientsQueryHandler(DbContext);

            // 1. Create a single batch for all ingredients to share
            var batch = new BeerBatch { Date = DateTime.Now };
            await DbContext.BeerBatches.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            // 2. Add 3 ingredients linked to that batch
            for (int i = 0; i < 3; i++)
            {
                await DbContext.Ingredients.AddAsync(new Ingredient
                {
                    Name = $"Ingredient {i}",
                    Unit = "g",
                    BeerBatchId = batch.Id // Link to the real ID
                });
            }
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Value.Results.Count);
            Assert.Equal(3, result.Value.RowCount);
        }

        [Fact]
        public async Task Get_should_throw_null_reference_exception_when_request_is_null()
        {
            // Arrange
            var query = (ListIngredientsQuery)null;
            var handler = new ListIngredientsQueryHandler(DbContext);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await handler.Handle(query, CancellationToken.None);
            });
        }
    }
}