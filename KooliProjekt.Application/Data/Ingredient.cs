using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Application.Data
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(20)]
        public string Unit { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Quantity { get; set; }

        public int BeerBatchId { get; set; }
        public BeerBatch BeerBatch { get; set; } = null!;
    }
}
