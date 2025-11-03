using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class BeerBatch
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Conclusion { get; set; }

        public int BeerSortId { get; set; }
        public BeerSort BeerSort { get; set; } = null!;

        public List<Ingredient> Ingredients { get; set; } = new();
        public List<BatchLog> Logs { get; set; } = new();
        public List<TasteLog> TasteLogs { get; set; } = new();
        public List<Photo> Photos { get; set; } = new();
    }
}
