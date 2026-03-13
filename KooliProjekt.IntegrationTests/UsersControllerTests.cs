using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Users;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class UsersControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_data_when_users_exist()
        {
            // Arrange
            var user = new User { Username = "master_brewer", Email = "brewer@brewery.com" };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var url = "/api/Users/List?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<User>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotEmpty(response.Value.Results);
        }

        [Fact]
        public async Task Save_should_add_new_user()
        {
            // Arrange
            var url = "/api/Users/Save";
            var command = new SaveUserCommand
            {
                Id = 0,
                Username = "new_user",
                Email = "new@test.com"
            };

            // Act
            var response = await Client.PostAsJsonAsync(url, command);

            // Assert
            response.EnsureSuccessStatusCode();
            var saved = await DbContext.Users.FirstOrDefaultAsync(x => x.Username == "new_user");
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task Delete_should_remove_existing_user()
        {
            // Arrange
            var user = new User { Username = "temporary_user", Email = "temp@test.com" };
            await DbContext.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var url = "/api/Users/Delete";
            var command = new DeleteUserCommand { Id = user.Id };

            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(command)
            };

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var exists = await DbContext.Users.AnyAsync(x => x.Id == user.Id);
            Assert.False(exists);
        }
    }
}
