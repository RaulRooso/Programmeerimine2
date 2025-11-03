using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Photo
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string FilePath { get; set; } = "";
        
        public int BeerBatchId { get; set; }
        public BeerBatch BeerBatch { get; set; } = null!;
    }
}
