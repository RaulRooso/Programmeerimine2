using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.BatchLogs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class BatchLogTests : TestBase
    {
        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListBatchLogsQueryHandler(null);
            });
        }
        [Fact]
        public async Task Get_should_return_paginated_batch_logs()
        {
            // Arrange
            var query = new ListBatchLogsQuery { Page = 1, PageSize = 2 };
            var handler = new ListBatchLogsQueryHandler(DbContext);

            for (int i = 0; i < 3; i++)
            {
                await DbContext.BatchLogs.AddAsync(new BatchLog
                {
                    Date = DateTime.Now.AddMinutes(i),
                    UserId = 1,
                    BeerBatchId = 1
                });
            }
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Value.Results.Count); // Items on this page
            Assert.Equal(3, result.Value.RowCount);      // Total items in DB
        }
        [Fact]
        public async Task Get_should_throw_null_reference_exception_when_request_is_null()
        {
            // Arrange
            var query = (ListBatchLogsQuery)null;
            var handler = new ListBatchLogsQueryHandler(DbContext);

            // Act & Assert
            // We expect the handler to fail because it tries to read 'request.Page'
            await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await handler.Handle(query, CancellationToken.None);
            });
        }
        // Afte updating the ListBatchLogsQueryHandler.cs Handle method to prevent it from crashing when the request is null
        // then use the code below
        //[Fact]
        //public async Task Get_should_survive_null_request()
        //{
        //    // Arrange
        //    // We explicitly cast null to the query type so the compiler knows which Handle method to use
        //    var query = (ListBatchLogsQuery)null;
        //    var handler = new ListBatchLogsQueryHandler(DbContext);

        //    // Act
        //    // We call the handler with a null request
        //    var result = await handler.Handle(query, CancellationToken.None);

        //    // Assert
        //    // What do we expect to happen here? 
        //    // Usually, we want 'result' itself to not be null, even if 'result.Value' is.
        //    Assert.NotNull(result);
        //}

    }
}
