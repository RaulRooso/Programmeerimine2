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
        [Fact]
        public async Task Delete_should_throw_exception_if_request_is_null()
        {
            // Fixes: if (request == null)
            var handler = new DeletePhotoCommandHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Fact]
        public async Task Delete_should_return_early_if_id_is_invalid()
        {
            // Fixes: if (request.Id <= 0)
            var command = new DeletePhotoCommand { Id = 0 };
            var handler = new DeletePhotoCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_return_early_if_item_not_found()
        {
            // Fixes: if (item == null)
            var command = new DeletePhotoCommand { Id = 999 };
            var handler = new DeletePhotoCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.HasErrors);
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

        [Fact]
        public async Task Save_should_update_existing_photo()
        {
            // Arrange
            var batchId = await SetupParentBatch();
            var existingPhoto = new Photo
            {
                BeerBatchId = batchId,
                FilePath = "old.jpg",
                Description = "Old Photo"
            };
            await DbContext.Photos.AddAsync(existingPhoto);
            await DbContext.SaveChangesAsync();

            var command = new SavePhotoCommand
            {
                Id = existingPhoto.Id, // Triggers the 'else' block
                BeerBatchId = batchId,
                FilePath = "new.jpg",
                Description = "Updated Photo"
            };
            var handler = new SavePhotoCommandHandler(DbContext);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            DbContext.ChangeTracker.Clear();
            var updated = await DbContext.Photos.FindAsync(existingPhoto.Id);

            Assert.NotNull(updated);
            Assert.Equal("new.jpg", updated.FilePath);
            Assert.Equal("Updated Photo", updated.Description);
        }
    }
}