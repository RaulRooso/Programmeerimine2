using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Photos;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class PhotoTests : TestBase
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
        public async Task Get_should_return_existing_photo_dto()
        {
            // Arrange
            var batchId = await SetupParentBatch();
            var photo = new Photo
            {
                Description = "Fermentation start",
                FilePath = "images/batch1.jpg",
                BeerBatchId = batchId
            };
            await DbContext.Photos.AddAsync(photo);
            await DbContext.SaveChangesAsync();

            var query = new GetPhotoQuery { Id = photo.Id };
            var handler = new GetPhotoQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal("images/batch1.jpg", result.Value.FilePath);
            Assert.Equal("Fermentation start", result.Value.Description);
        }

        // === LIST TESTS ===

        [Fact]
        public async Task List_should_return_paged_photos()
        {
            // Arrange
            var batchId = await SetupParentBatch();
            var query = new ListPhotosQuery { Page = 1, PageSize = 2 };
            var handler = new ListPhotosQueryHandler(DbContext);

            for (int i = 1; i <= 4; i++)
            {
                await DbContext.Photos.AddAsync(new Photo
                {
                    FilePath = $"file{i}.jpg",
                    BeerBatchId = batchId
                });
            }
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Results.Count);
            Assert.Equal(4, result.Value.RowCount);
        }

        // === DELETE TESTS ===

        [Fact]
        public async Task Delete_should_remove_photo_from_database()
        {
            // Arrange
            var batchId = await SetupParentBatch();
            var photo = new Photo { FilePath = "delete-me.jpg", BeerBatchId = batchId };
            await DbContext.Photos.AddAsync(photo);
            await DbContext.SaveChangesAsync();

            var command = new DeletePhotoCommand { Id = photo.Id };
            var handler = new DeletePhotoCommandHandler(DbContext);

            // Act
            await handler.Handle(command, CancellationToken.None);
            var exists = await DbContext.Photos.AnyAsync(x => x.Id == photo.Id);

            // Assert
            Assert.False(exists);
        }

        // === SAVE TESTS ===

        [Fact]
        public async Task Save_should_add_new_photo()
        {
            // Arrange
            var batchId = await SetupParentBatch();
            var command = new SavePhotoCommand
            {
                Id = 0,
                Description = "New Photo",
                FilePath = "new.png",
                BeerBatchId = batchId
            };
            var handler = new SavePhotoCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var saved = await DbContext.Photos.FirstOrDefaultAsync(x => x.FilePath == "new.png");

            // Assert
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
        }
    }
}