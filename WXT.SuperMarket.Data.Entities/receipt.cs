namespace WXT.SuperMarket.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="Receipt" />
    /// </summary>
    public class Receipt
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the TransactionTime
        /// </summary>
        public DateTimeOffset TransactionTime { get; set; }

        /// <summary>
        /// Gets or sets the ShoppingList
        /// </summary>
        public List<TransactionItem> ShoppingList { get; set; }

        /// <summary>
        /// Gets the TotalPrice
        /// </summary>
        public decimal TotalPrice => ShoppingList.Sum(s => s.TotalPrice);

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override string ToString()
        {
            return $"Id = {Id} TransactionTime = {TransactionTime} ToalPrice = {TotalPrice}{Environment.NewLine}{string.Join(Environment.NewLine, ShoppingList.Select(s => s.ToString()))}";
        }
    }
}
