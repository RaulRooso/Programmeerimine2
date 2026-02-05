using System;

namespace KooliProjekt.Application.Dto
{
    public class TasteLogDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public int Rating { get; set; }
        public int UserId { get; set; }
        public int BeerBatchId { get; set; }
    }
}