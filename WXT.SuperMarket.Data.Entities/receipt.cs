namespace WXT.SuperMarket.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Receipt
    {
        public int Id { get; set; }

        public DateTimeOffset TransactionTime { get; set; }

        public List<TransactionItem> ShoppingList { get; set; }

        public decimal TotalPrice => ShoppingList.Sum(s => s.TotalPrice);

        public override string ToString()
        {
            return $"Id = {Id} TransactionTime = {TransactionTime} ToalPrice = {TotalPrice}{Environment.NewLine}" +
                $"{string.Join(Environment.NewLine, ShoppingList.Select(s => s.ToString()))}";
        }
    }
}
