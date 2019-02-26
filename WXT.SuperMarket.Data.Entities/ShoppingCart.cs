using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXT.SuperMarket.Data.Entities
{
    public class ShoppingCart
    {
        public int CustomerId { get; set; }
        public List<ProductItem> ItemList { get; set; } = new List<ProductItem>();
    }
}
