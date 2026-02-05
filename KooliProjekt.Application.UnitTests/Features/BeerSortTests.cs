using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.BeerSorts;
using KooliProjekt.Application.Dto;
using Microsoft.EntityFrameworkCore;
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