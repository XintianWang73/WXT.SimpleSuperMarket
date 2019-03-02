namespace WXT.SuperMarket.Data.Entities
{
    /// <summary>
    /// Defines the <see cref="TransactionItem" />
    /// </summary>
    public class TransactionItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice => Price * Count;

        public override string ToString()
        {
            return $"{Id} {Name} {Count} @ {Price}   {TotalPrice}";
        }
    }
}
