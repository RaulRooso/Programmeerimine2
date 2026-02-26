using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class BeerSortDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}