namespace WXT.SuperMarket.Business.Services
{
    using System;
    using System.IO;
    using System.Threading;
    using WXT.SuperMarket.Data.Entities;
    using WXT.SuperMarket.Data.Repository;

    /// <summary>
    /// Defines the <see cref="MarketService" />
    /// </summary>
    public class MarketService
    {
        //private readonly IMarketRepository _marketRepository = new InMemoryMarketRepository();
        /// <summary>
        /// Defines the _marketRepository
        /// </summary>
        private readonly IMarketRepository _marketRepository = new JsonMarketRepository();

        /// <summary>
        /// The LockFile
        /// </summary>
        /// <param name="fileName">The fileName<see cref="string"/></param>
        /// <returns>The <see cref="FileStream"/></returns>
        private FileStream LockFile(string fileName)
        {
            while (true)
            {
                try
                {
                    return File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// The UnlockFile
        /// </summary>
        /// <param name="fileStream">The fileStream<see cref="FileStream"/></param>
        private void UnlockFile(FileStream fileStream)
        {
            while (true)
            {
                try
                {
                    fileStream.Close();
                    break;
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// The AddProduct
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="price">The price<see cref="decimal"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string AddProduct(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("userName", "UserName cannot be null, enmpty or only white spaces.");
            }
            if (price <= 0m)
            {
                throw new ArgumentNullException("price", "price cannot be less than 0.");
            }
            var locker = LockFile("product.lk");
            var result = _marketRepository.AddProduct(new Product { Name = name, Price = price }).ToString();
            UnlockFile(locker);
            return result;
        }

        /// <summary>
        /// The RemoveProduct
        /// </summary>
        /// <param name="productID">The productID<see cref="int"/></param>
        public void RemoveProduct(int productID)
        {
            var locker = LockFile("product.lk");
            var product = _marketRepository.FindProduct(productID);
            if (product == null)
            {
                UnlockFile(locker);
                throw new InvalidOperationException("This product does not exist.");
            }
            var locker2 = LockFile("stock.lk");
            var stock = _marketRepository.GetStock(productID);
            if (stock != null)
            {
                UnlockFile(locker2);
                UnlockFile(locker);
                throw new InvalidOperationException($"There are still some {product.Name}({stock.Count}) in the stock.");
            }
            _marketRepository.RemoveProduct(product);
            UnlockFile(locker2);
            UnlockFile(locker);
        }

        /// <summary>
        /// The FindAllProduct
        /// </summary>
        /// <param name="isOnlyInStock">The isOnlyInStock<see cref="bool"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string FindAllProduct(bool isOnlyInStock)
        {
            return _marketRepository.FindAllProduct(isOnlyInStock);
        }

        /// <summary>
        /// The AddToStock
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        public void AddToStock(int id, int count = 1)
        {
            var locker = LockFile("product.lk");
            if (_marketRepository.FindProduct(id) == null)
            {
                UnlockFile(locker);
                throw new InvalidOperationException("This product does not exist.");
            }
            if (count <= 0)
            {
                UnlockFile(locker);
                throw new ArgumentOutOfRangeException("count", "Count cannot less than 1.");
            }
            var locker2 = LockFile("stock.lk");
            _marketRepository.AddToStock(id, count);
            UnlockFile(locker2);
            UnlockFile(locker);
        }

        /// <summary>
        /// The RemoveFromStock
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        public void RemoveFromStock(int id, int count = 1)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count", "Count cannot less than 1.");
            }
            var locker = LockFile("stock.lk");
            var stock = _marketRepository.GetStock(id);
            if (stock == null)
            {
                UnlockFile(locker);
                throw new InvalidOperationException("This product is out of stock.");
            }

            if (count > stock.Count)
            {
                UnlockFile(locker);
                throw new ArgumentOutOfRangeException("count", "There is no enough product in stock.");
            }
            _marketRepository.RemoveFromStock(stock, count);
            UnlockFile(locker);
        }
    }
}
