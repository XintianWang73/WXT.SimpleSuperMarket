namespace WXT.SuperMarket.Data.Entities
{
    /// <summary>
    /// Defines the <see cref="Product" />
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Price
        /// </summary>
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"Id = {Id} Name = {Name} Description = \"{Description}\" Price = {Price}"; 
        }
    }
}
