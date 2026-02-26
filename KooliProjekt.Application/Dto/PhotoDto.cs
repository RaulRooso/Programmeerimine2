using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class PhotoDto
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public int BeerBatchId { get; set; }
    }
}