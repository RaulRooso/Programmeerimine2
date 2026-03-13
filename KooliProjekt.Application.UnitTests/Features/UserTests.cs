using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.TasteLogs;
using KooliProjekt.Application.Features.Users;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class UserTests : TestBase
    {
        // === GET TESTS ===

        [Fact]
        public async Task Get_should_return_existing_user_dto()
        {
            // Arrange
            var user = new User { Username = "beer_master", Email = "pro@brew.com" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var query = new GetUserQuery { Id = user.Id };
            var handler = new GetUserQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal("beer_master", result.Value.Username);
            Assert.Equal("pro@brew.com", result.Value.Email);
        }

        // === LIST TESTS ===

        [Fact]
        public async Task List_should_return_paged_users()
        {
            // Arrange
            var query = new ListUsersQuery { Page = 1, PageSize = 2 };
            var handler = new ListUsersQueryHandler(DbContext);

            for (int i = 1; i <= 4; i++)
            {
                await DbContext.Users.AddAsync(new User { Username = $"User_{i}" });
            }
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Results.Count);
            Assert.Equal(4, result.Value.RowCount);
        }

        [Fact]
        public async Task List_should_filter_by_username_when_search_term_is_provided()
        {
            // Arrange
            await DbContext.Users.AddAsync(new User { Username = "AdminUser" });
            await DbContext.Users.AddAsync(new User { Username = "GuestUser" });
            await DbContext.SaveChangesAsync();

            var query = new ListUsersQuery { Page = 1, PageSize = 10, Username = "Admin" };
            var handler = new ListUsersQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("AdminUser", result.Value.Results.First().Username);
        }

        [Fact]
        public async Task List_should_return_empty_result_if_pagination_is_invalid()
        {
            // Fixes: if (request.Page <= 0 || request.PageSize <= 0)
            var query = new ListUsersQuery { Page = 0, PageSize = 10 };
            var handler = new ListUsersQueryHandler(DbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.Null(result.Value);
            Assert.False(result.HasErrors);
        }

        // === DELETE TESTS ===

        [Fact]
        public async Task Delete_should_remove_user_from_database()
        {
            // Arrange
            var user = new User { Username = "delete_me" };
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var command = new DeleteUserCommand { Id = user.Id };
            var handler = new DeleteUserCommandHandler(DbContext);

            // Act
            await handler.Handle(command, CancellationToken.None);
            var exists = await DbContext.Users.AnyAsync(x => x.Id == user.Id);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task Delete_should_throw_exception_if_request_is_null()
        {
            // Covers: if (request == null)
            var handler = new DeleteUserCommandHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Fact]
        public async Task Delete_should_return_early_if_id_is_invalid()
        {
            // Covers: if (request.Id <= 0)
            var command = new DeleteUserCommand { Id = 0 };
            var handler = new DeleteUserCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_return_early_if_item_not_found()
        {
            // Covers: if (item == null)
            var command = new DeleteUserCommand { Id = 999 };
            var handler = new DeleteUserCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.HasErrors);
        }

        // === SAVE TESTS ===

        [Fact]
        public async Task Save_should_add_new_user()
        {
            // Arrange
            var command = new SaveUserCommand
            {
                Id = 0,
                Username = "new_brewer",
                Email = "new@brew.com"
            };
            var handler = new SaveUserCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var saved = await DbContext.Users.FirstOrDefaultAsync(x => x.Username == "new_brewer");

            // Assert
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task Save_should_update_existing_user()
        {
            // Arrange: Create a user first so we have one to update
            var existingUser = new User { Username = "old_name", Email = "old@brew.com" };
            await DbContext.Users.AddAsync(existingUser);
            await DbContext.SaveChangesAsync();

            var command = new SaveUserCommand
            {
                Id = existingUser.Id, // This triggers the 'else' block
                Username = "updated_name",
                Email = "updated@brew.com"
            };
            var handler = new SaveUserCommandHandler(DbContext);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert: Clear the tracker to ensure we fetch fresh data from the DB
            DbContext.ChangeTracker.Clear();
            var updated = await DbContext.Users.FindAsync(existingUser.Id);

            Assert.NotNull(updated);
            Assert.Equal("updated_name", updated.Username);
            Assert.Equal("updated@brew.com", updated.Email);
        }
    }
}