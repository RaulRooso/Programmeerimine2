using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class PhotosControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_data_when_photos_exist()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "Stout", Description = "Dark" };
            await DbContext.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var batch = new BeerBatch { BeerSortId = beerSort.Id, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var photo = new Photo
            {
                BeerBatchId = batch.Id,
                Description = "Fermentation bubble view",
                FilePath = "uploads/test-photo.jpg" // Required field
            };
            await DbContext.AddAsync(photo);
            await DbContext.SaveChangesAsync();

            var url = "/api/Photos/List?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<Photo>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotEmpty(response.Value.Results);
        }

        [Fact]
        public async Task Delete_should_remove_photo_record_from_database()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "Porter" };
            await DbContext.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var batch = new BeerBatch { BeerSortId = beerSort.Id, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var photo = new Photo { BeerBatchId = batch.Id, FilePath = "temp.jpg" };
            await DbContext.AddAsync(photo);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Photos/Delete?Id={photo.Id}";

            // Act
            var response = await Client.DeleteAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var exists = await DbContext.Photos.AnyAsync(x => x.Id == photo.Id);
            Assert.False(exists);
        }
    }
}
