using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Application.Data
{
    public class BeerBatch : Entity
    {
        [Required]
        public DateTime Date { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? Conclusion { get; set; }

        [ForeignKey(nameof(BeerSort))]
        public int BeerSortId { get; set; }
        public BeerSort BeerSort { get; set; } = null!;

        public List<Ingredient> Ingredients { get; set; } = new();
        public List<BatchLog> Logs { get; set; } = new();
        public List<TasteLog> TasteLogs { get; set; } = new();
        public List<Photo> Photos { get; set; } = new();
    }
}
