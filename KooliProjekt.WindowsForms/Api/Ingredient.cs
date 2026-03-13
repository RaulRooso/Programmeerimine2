namespace KooliProjekt.WindowsForms.Api
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int BeerBatchId { get; set; }
    }
}