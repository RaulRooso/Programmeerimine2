using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Application.Data
{
    public class Photo : Entity
    {
        [StringLength(500)]
        public string? Description { get; set; }

        [Required, StringLength(260)]
        public string FilePath { get; set; } = string.Empty;

        [ForeignKey(nameof(BeerBatch))]
        public int BeerBatchId { get; set; }
        public BeerBatch BeerBatch { get; set; } = null!;
    }
}
