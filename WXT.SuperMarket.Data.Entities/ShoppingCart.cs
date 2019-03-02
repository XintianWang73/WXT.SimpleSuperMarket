namespace WXT.SuperMarket.Data.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="ShoppingCart" />
    /// </summary>
    public class ShoppingCart
    {
        public int CustomerId { get; set; }

        public List<ProductItem> ItemList { get; set; } = new List<ProductItem>();
    }
}
