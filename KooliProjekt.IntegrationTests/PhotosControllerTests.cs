using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Photos;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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
            var batch = new BeerBatch { BeerSort = beerSort, Date = DateTime.Now };
            await DbContext.AddAsync(batch);

            var photo = new Photo
            {
                BeerBatch = batch,
                Description = "Fermentation bubble view",
                FilePath = "uploads/test-photo.jpg"
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
        public async Task Save_should_add_new_photo()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "IPA" };
            await DbContext.AddAsync(beerSort);
            var batch = new BeerBatch { BeerSort = beerSort, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            await DbContext.SaveChangesAsync();

            var url = "/api/Photos/Save";
            var command = new SavePhotoCommand
            {
                Id = 0,
                BeerBatchId = batch.Id,
                Description = "New Brew Day Photo",
                FilePath = "brewing.jpg"
            };

            // Act
            var response = await Client.PostAsJsonAsync(url, command);

            // Assert
            response.EnsureSuccessStatusCode();
            var saved = await DbContext.Photos.FirstOrDefaultAsync(x => x.Description == "New Brew Day Photo");
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task Delete_should_remove_existing_photo()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "Porter" };
            await DbContext.AddAsync(beerSort);
            var batch = new BeerBatch { BeerSort = beerSort, Date = DateTime.Now };
            await DbContext.AddAsync(batch);
            var photo = new Photo { Description = "Delete Me", BeerBatch = batch, FilePath = "temp.jpg" };
            await DbContext.AddAsync(photo);
            await DbContext.SaveChangesAsync();

            var url = "/api/Photos/Delete";
            var command = new DeletePhotoCommand { Id = photo.Id };

            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var exists = await DbContext.Photos.AnyAsync(x => x.Id == photo.Id);
            Assert.False(exists);
        }
    }
}
