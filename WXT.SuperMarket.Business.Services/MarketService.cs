namespace WXT.SuperMarket.Business.Services
{
    using System;
    using WXT.SuperMarket.Data.Entities;
    using WXT.SuperMarket.Data.Repository;

    /// <summary>
    /// Defines the <see cref="MarketService" />
    /// </summary>
    public class MarketService
    {
        /// <summary>
        /// Defines the _marketRepository
        /// </summary>
        private readonly InMemoryMarketRepository _marketRepository = new InMemoryMarketRepository();

        /// <summary>
        /// The GetStock
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <returns>The <see cref="ProductItem"/></returns>
        public ProductItem GetStock(int id)
        {
            return _marketRepository.GetStock(id);
        }

        /// <summary>
        /// The AddProduct
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="price">The price<see cref="decimal"/></param>
        /// <returns>The <see cref="Product"/></returns>
        public Product AddProduct(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("userName", "UserName cannot be null, enmpty or only white spaces.");
            }
            if (price <= 0m)
            {
                throw new ArgumentNullException("price", "price cannot be less than 0.");
            }
            return _marketRepository.AddProduct(new Product { Name = name, Price = price });
        }

        /// <summary>
        /// The RemoveProduct
        /// </summary>
        /// <param name="productID">The productID<see cref="int"/></param>
        public void RemoveProduct(int productID)
        {
            var product = _marketRepository.FindProduct(productID);
            if (product == null)
            {
                throw new InvalidOperationException("This product does not exist.");
            }
            var stock = _marketRepository.GetStock(productID);
            if (stock != null)
            {
                throw new InvalidOperationException($"There are still some {product.Name}({product.Id}) in the stock.");
            }
            _marketRepository.RemoveProduct(product);
        }

        /// <summary>
        /// The AddToStock
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        public void AddToStock(int id, int count = 1)
        {
            if (_marketRepository.FindProduct(id) == null)
            {
                throw new InvalidOperationException("This product does not exist.");
            }
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count", "Count cannot less than 1.");
            }
            _marketRepository.AddToStock(id, count);
        }

        /// <summary>
        /// The RemoveFromStock
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        public void RemoveFromStock(int id, int count = 1)
        {
            var stock = _marketRepository.GetStock(id);
            if (stock == null)
            {
                throw new InvalidOperationException("This product is out of stock.");
            }
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count", "Count cannot less than 1.");
            }
            if (count > stock.Count)
            {
                throw new ArgumentOutOfRangeException("count", "There is no enough product in stock.");
            }
            _marketRepository.RemoveFromStock(stock, count);
        }
    }
}
