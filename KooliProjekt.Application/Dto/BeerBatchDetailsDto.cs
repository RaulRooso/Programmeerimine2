using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class BeerBatchDetailsDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string? Conclusion { get; set; }
        public int BeerSortId { get; set; }

        public List<IngredientDto> Ingredients { get; set; } = new();
        public List<BatchLogDto> Logs { get; set; } = new();
        public List<PhotoDto> Photos { get; set; } = new();
    }

}