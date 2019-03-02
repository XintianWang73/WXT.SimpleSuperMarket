namespace WXT.SuperMarket.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"Id = {Id} Name = {Name} Description = \"{Description}\" Price = {Price}"; 
        }
    }
}
