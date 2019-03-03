namespace WXT.SuperMarket.Data.Entities
{
    /// <summary>
    /// Defines the <see cref="TransactionItem" />
    /// </summary>
    public class TransactionItem
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
        /// Gets or sets the Count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets the TotalPrice
        /// </summary>
        public decimal TotalPrice => Price * Count;

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override string ToString()
        {
            return $"{Id} {Name} {Count} @ {Price}   {TotalPrice}";
        }
    }
}
