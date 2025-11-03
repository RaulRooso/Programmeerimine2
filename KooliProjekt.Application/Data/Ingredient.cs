using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit {  get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }

        public int BeerBatchId {  get; set; }
        public BeerBatch BeerBatch { get; set; } = null!;
    }
}
