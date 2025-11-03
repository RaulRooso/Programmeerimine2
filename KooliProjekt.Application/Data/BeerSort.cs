using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class BeerSort
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; }

        public List<BeerBatch> Batches { get; set; } = new();
    }
}
