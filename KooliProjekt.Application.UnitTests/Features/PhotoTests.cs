using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Photos;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class PhotoTests : TestBase
    {
        [Fact]
        public void Constructor_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListPhotosQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_paginated_photos()
        {
            // Arrange
            var query = new ListPhotosQuery { Page = 1, PageSize = 2 };
            var handler = new ListPhotosQueryHandler(DbContext);

            // 1. Create a parent batch
            var batch = new BeerBatch { Date = DateTime.Now };
            await DbContext.BeerBatches.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            // 2. Add 3 photos linked to that batch
            for (int i = 0; i < 3; i++)
            {
                await DbContext.Photos.AddAsync(new Photo
                {
                    FilePath = $"path/to/photo{i}.jpg",
                    BeerBatchId = batch.Id
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
            var query = (ListPhotosQuery)null;
            var handler = new ListPhotosQueryHandler(DbContext);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await handler.Handle(query, CancellationToken.None);
            });
        }
    }
}