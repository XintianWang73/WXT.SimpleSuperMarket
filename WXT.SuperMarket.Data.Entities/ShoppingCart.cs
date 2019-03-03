namespace WXT.SuperMarket.Data.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="ShoppingCart" />
    /// </summary>
    public class ShoppingCart
    {
        /// <summary>
        /// Gets or sets the CustomerId
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the ItemList
        /// </summary>
        public List<ProductItem> ItemList { get; set; } = new List<ProductItem>();
    }
}
