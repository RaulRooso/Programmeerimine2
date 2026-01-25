using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.BeerBatches;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class BeerBatchTests : TestBase
    {
        [Fact]
        public void Constructor_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListBeerBatchesQueryHandler(null);
            });
        }
        [Fact]
        public async Task Get_should_return_paginated_beer_batches()
        {
            // Arrange
            var query = new ListBeerBatchesQuery { Page = 1, PageSize = 2 };
            var handler = new ListBeerBatchesQueryHandler(DbContext);

            for (int i = 0; i < 3; i++)
            {
                await DbContext.BeerBatches.AddAsync(new BeerBatch
                {
                    Date = DateTime.Now.AddDays(i),
                    BeerSortId = 1,
                    Description = $"Batch {i}"
                });
            }
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(2, result.Value.Results.Count);
            Assert.Equal(3, result.Value.RowCount);
        }
        [Fact]
        public async Task Get_should_throw_null_reference_exception_when_request_is_null()
        {
            // Arrange
            var query = (ListBeerBatchesQuery)null;
            var handler = new ListBeerBatchesQueryHandler(DbContext);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await handler.Handle(query, CancellationToken.None);
            });
        }
    }
}