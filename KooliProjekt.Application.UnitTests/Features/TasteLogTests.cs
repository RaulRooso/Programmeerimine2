using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.TasteLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class TasteLogTests : TestBase
    {
        [Fact]
        public void Constructor_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListTasteLogsQueryHandler(null);
            });
        }
        [Fact]
        public async Task Get_should_return_paginated_taste_logs()
        {
            // Arrange
            var query = new ListTasteLogsQuery { Page = 1, PageSize = 2 };
            var handler = new ListTasteLogsQueryHandler(DbContext);

            // 1. Create a parent user
            var user = new User { Username = "Tester" };
            await DbContext.Users.AddAsync(user);

            // 2. Create a parent batch
            var batch = new BeerBatch { Date = DateTime.Now };
            await DbContext.BeerBatches.AddAsync(batch);

            await DbContext.SaveChangesAsync();

            // 3. Add 3 taste logs
            for (int i = 0; i < 3; i++)
            {
                await DbContext.TasteLogs.AddAsync(new TasteLog
                {
                    Date = DateTime.Now.AddDays(i),
                    Rating = 5,
                    UserId = user.Id,
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
            var query = (ListTasteLogsQuery)null;
            var handler = new ListTasteLogsQueryHandler(DbContext);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await handler.Handle(query, CancellationToken.None);
            });
        }
    }
}
