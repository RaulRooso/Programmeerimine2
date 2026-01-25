using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class UserTests : TestBase
    {
        [Fact]
        public void Constructor_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListUsersQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_return_paginated_users()
        {
            // Arrange
            var query = new ListUsersQuery { Page = 1, PageSize = 2 };
            var handler = new ListUsersQueryHandler(DbContext);

            for (int i = 0; i < 3; i++)
            {
                await DbContext.Users.AddAsync(new User
                {
                    Id = i,
                    Username = $"Name {i}"
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
            var query = (ListUsersQuery)null;
            var handler = new ListUsersQueryHandler(DbContext);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await handler.Handle(query, CancellationToken.None);
            });
        }
    }
}
