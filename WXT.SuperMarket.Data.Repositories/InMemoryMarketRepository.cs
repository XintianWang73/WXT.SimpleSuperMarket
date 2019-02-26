using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXT.SuperMarket.Data.Entities;

namespace WXT.SuperMarket.Data.Repository
{
    public class InMemoryMarketRepository
    {
        private static readonly List<Product> _products = new List<Product>();

        private static readonly List<ProductItem> _stock = new List<ProductItem>();

        public Product FindProduct(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public Product FindProduct(string productName)
        {
            return _products.FirstOrDefault(p => p.Name == productName);
        }

        public Product AddProduct(Product product)
        {
            int maxId = 0;
            try
            {
                maxId = _products.Max(p => p.Id);
            }
            catch
            {
            }
            product.Id = maxId + 1;
            product.Description = product.Name;

            _products.Add(product);

            return product;
        }

        public void RemoveProduct(Product product)
        {
            _products.Remove(product);
        }

        public ProductItem GetStock(int id)
        {
            return _stock.FirstOrDefault(s => s.ProductId == id);
        }

        public void AddToStock(int productId, int count)
        {
            var resultItem = GetStock(productId);
            if (resultItem == null)
            {
                _stock.Add(new ProductItem { ProductId = productId, Count = count});
            }
            else
            {
                resultItem.Count += count;
            }
        }

        public void RemoveFromStock(ProductItem productItem, int count)
        {
            productItem.Count -= count;
            if (productItem.Count == 0)
            {
                _stock.Remove(productItem);
            }
        }
    }
}
