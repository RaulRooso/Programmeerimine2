using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.BeerSorts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class BeerSortTests : TestBase
    {
        // === GET TESTS ===

        [Fact]
        public async Task Get_should_return_existing_beer_sort_dto()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "IPA" };
            await DbContext.BeerSorts.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var query = new GetBeerSortQuery { Id = beerSort.Id };
            var handler = new GetBeerSortQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.IsType<BeerSortDto>(result.Value);
            Assert.Equal("IPA", result.Value.Name);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_return_null_when_id_is_invalid(int id)
        {
            // Arrange
            var query = new GetBeerSortQuery { Id = id };
            var handler = new GetBeerSortQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result.Value);
        }

        // === LIST TESTS ===

        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListBeerSortsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListBeerSortsQuery)null;
            var handler = new ListBeerSortsQueryHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-1, 5)]
        public async Task List_should_return_empty_operation_result_when_page_or_size_is_invalid(int page, int pageSize)
        {
            // Arrange
            var query = new ListBeerSortsQuery { Page = page, PageSize = pageSize };
            var handler = new ListBeerSortsQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_paged_beer_sorts()
        {
            // Arrange
            var query = new ListBeerSortsQuery { Page = 1, PageSize = 5 };
            var handler = new ListBeerSortsQueryHandler(DbContext);

            for (int i = 1; i <= 10; i++)
            {
                await DbContext.BeerSorts.AddAsync(new BeerSort { Name = $"Sort {i}" });
            }
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(5, result.Value.Results.Count);
            Assert.Equal(10, result.Value.RowCount);
        }

        [Fact]
        public async Task List_should_filter_by_name_when_search_term_is_provided()
        {
            // Arrange
            await DbContext.BeerSorts.AddAsync(new BeerSort { Name = "IPA" });
            await DbContext.BeerSorts.AddAsync(new BeerSort { Name = "Stout" });
            await DbContext.BeerSorts.AddAsync(new BeerSort { Name = "Lager" });
            await DbContext.SaveChangesAsync();

            var query = new ListBeerSortsQuery { Page = 1, PageSize = 10, Name = "Stout" };
            var handler = new ListBeerSortsQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("Stout", result.Value.Results.First().Name);
        }

        // === DELETE TESTS ===

        [Fact]
        public async Task Delete_should_remove_beer_sort_from_database()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "To be deleted" };
            await DbContext.BeerSorts.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var command = new DeleteBeerSortCommand { Id = beerSort.Id };
            var handler = new DeleteBeerSortCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var exists = await DbContext.BeerSorts.AnyAsync(x => x.Id == beerSort.Id);

            // Assert
            Assert.False(result.HasErrors);
            Assert.False(exists);
        }

        [Fact]
        public async Task Delete_should_throw_exception_if_request_is_null()
        {
            // Fixes: if (request == null)
            var handler = new DeleteBeerSortCommandHandler(DbContext);

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }

        [Fact]
        public async Task Delete_should_return_early_if_id_is_invalid()
        {
            // Fixes: if (request.Id <= 0)
            var command = new DeleteBeerSortCommand { Id = 0 };
            var handler = new DeleteBeerSortCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_return_early_if_item_not_found()
        {
            // Fixes: if (item == null)
            var command = new DeleteBeerSortCommand { Id = 999 };
            var handler = new DeleteBeerSortCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.HasErrors);
        }

        // === SAVE TESTS ===

        [Fact]
        public async Task Save_should_add_new_beer_sort_when_id_is_zero()
        {
            // Arrange
            var command = new SaveBeerSortCommand { Id = 0, Name = "Stout" };
            var handler = new SaveBeerSortCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var savedItem = await DbContext.BeerSorts.FirstOrDefaultAsync(x => x.Name == "Stout");

            // Assert
            Assert.False(result.HasErrors);
            Assert.NotNull(savedItem);
            Assert.Equal("Stout", savedItem.Name);
        }

        [Fact]
        public async Task Save_should_update_existing_beer_sort()
        {
            // Arrange
            var beerSort = new BeerSort { Name = "Old Name" };
            await DbContext.BeerSorts.AddAsync(beerSort);
            await DbContext.SaveChangesAsync();

            var command = new SaveBeerSortCommand { Id = beerSort.Id, Name = "Updated Name" };
            var handler = new SaveBeerSortCommandHandler(DbContext);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Re-fetch from DB to verify
            var updatedItem = await DbContext.BeerSorts.FindAsync(beerSort.Id);

            // Assert
            Assert.Equal("Updated Name", updatedItem.Name);
        }
    }
}