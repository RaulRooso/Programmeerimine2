using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.BeerSorts;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class BeerSortTests : TestBase
    {
        [Fact]
        public void Constructor_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListBeerSortsQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_paginated_beer_sorts()
        {
            // Arrange
            var query = new ListBeerSortsQuery { Page = 1, PageSize = 2 };
            var handler = new ListBeerSortsQueryHandler(DbContext);

            // Add 3 sorts
            for (int i = 0; i < 3; i++)
            {
                await DbContext.BeerSorts.AddAsync(new BeerSort
                {
                    Name = $"Sort {i}",
                    Description = $"Description {i}"
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
            var query = (ListBeerSortsQuery)null;
            var handler = new ListBeerSortsQueryHandler(DbContext);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await handler.Handle(query, CancellationToken.None);
            });
        }
    }
}