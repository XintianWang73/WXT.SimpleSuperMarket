namespace WXT.SuperMarket.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WXT.SuperMarket.Data.Entities;

    /// <summary>
    /// Defines the <see cref="InMemoryMarketRepository" />
    /// </summary>
    public class InMemoryMarketRepository : IMarketRepository
    {
        /// <summary>
        /// Defines the _products
        /// </summary>
        private static readonly List<Product> _products = new List<Product>()
        {
            new Product {Id = 1 , Name = "banana", Price = 2.34m, Description = "banana"},
            new Product {Id = 2 , Name = "apple", Price = 1.34m, Description = "apple"},
            new Product {Id = 3 , Name = "TV", Price = 1500m, Description = "TV set"}
        };

        /// <summary>
        /// Defines the _stock
        /// </summary>
        private static readonly List<ProductItem> _stock = new List<ProductItem>()
        {
            new ProductItem {ProductId = 1 , Count = 10},
            new ProductItem {ProductId = 2 , Count = 2},
            new ProductItem {ProductId = 3 , Count = 4}
        };

        /// <summary>
        /// The FindProduct
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <returns>The <see cref="Product"/></returns>
        public Product FindProduct(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// The FindProduct
        /// </summary>
        /// <param name="productName">The productName<see cref="string"/></param>
        /// <returns>The <see cref="Product"/></returns>
        public Product FindProduct(string productName)
        {
            return _products.FirstOrDefault(p => p.Name == productName);
        }

        /// <summary>
        /// The AddProduct
        /// </summary>
        /// <param name="product">The product<see cref="Product"/></param>
        /// <returns>The <see cref="Product"/></returns>
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

        /// <summary>
        /// The RemoveProduct
        /// </summary>
        /// <param name="product">The product<see cref="Product"/></param>
        public void RemoveProduct(Product product)
        {
            _products.Remove(product);
        }

        /// <summary>
        /// The GetStock
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <returns>The <see cref="ProductItem"/></returns>
        public ProductItem GetStock(int id)
        {
            return _stock.FirstOrDefault(s => s.ProductId == id);
        }

        /// <summary>
        /// The AddToStock
        /// </summary>
        /// <param name="productId">The productId<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        public void AddToStock(int productId, int count)
        {
            var resultItem = GetStock(productId);
            if (resultItem == null)
            {
                _stock.Add(new ProductItem { ProductId = productId, Count = count });
            }
            else
            {
                resultItem.Count += count;
            }
        }

        /// <summary>
        /// The RemoveFromStock
        /// </summary>
        /// <param name="productItem">The productItem<see cref="ProductItem"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        public void RemoveFromStock(ProductItem productItem, int count)
        {
            productItem.Count -= count;
            if (productItem.Count == 0)
            {
                _stock.Remove(productItem);
            }
        }

        /// <summary>
        /// The FindAllProduct
        /// </summary>
        /// <param name="isOnlyInStock">The isOnlyInStock<see cref="bool"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string FindAllProduct(bool isOnlyInStock)
        {
            if (isOnlyInStock)
            {
                return
                    string.Join(Environment.NewLine, _stock.Join(_products,
                    item => item.ProductId,
                    product => product.Id,
                    (item, product) => product.ToString() + " Count = " + item.Count.ToString()));
            }
            else
            {
                return
                    string.Join(Environment.NewLine, _products.Join(_stock,
                    product => product.Id,
                    item => item.ProductId,
                    (product, item) => product.ToString() + " Count = " + (item?.Count ?? 0)));
            }
        }

        /// <summary>
        /// The RemoveFromStock
        /// </summary>
        /// <param name="productId">The productId<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        public void RemoveFromStock(int productId, int count)
        {
            var productItem = GetStock(productId);
            productItem.Count -= count;
            if (productItem.Count == 0)
            {
                _stock.Remove(productItem);
            }
        }
    }
}
