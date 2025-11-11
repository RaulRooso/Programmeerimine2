using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Application.Data
{
    public class BatchLog
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [ForeignKey(nameof(BeerBatch))]
        public int BeerBatchId { get; set; }
        public BeerBatch BeerBatch { get; set; } = null!;
    }
}
