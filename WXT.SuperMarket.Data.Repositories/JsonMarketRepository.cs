using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXT.SuperMarket.Data.Entities;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace WXT.SuperMarket.Data.Repository
{
    public class JsonMarketRepository : IMarketRepository
    {
        private const string _productFile = "products.json";
        private const string _stockFile = "stocks.json";

        private static List<Product> _products; //e

        private static List<ProductItem> _stock; //d

        private void GetProducts()
        {
            try
            {
                _products = JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(_productFile));
            }
            catch
            {
                _products = new List<Product>();
            }
        }

        private void GetStocks()
        {
            try
            {
                _stock = JsonConvert.DeserializeObject<List<ProductItem>>(File.ReadAllText(_stockFile));
            }
            catch
            {
                _stock = new List<ProductItem>();
            }
        }

        /// <summary>
        /// The FindProduct
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <returns>The <see cref="Product"/></returns>
        public Product FindProduct(int id)
        {
            GetProducts();
            return _products.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// The FindProduct
        /// </summary>
        /// <param name="productName">The productName<see cref="string"/></param>
        /// <returns>The <see cref="Product"/></returns>
        public Product FindProduct(string productName)
        {
            GetProducts();
            return _products.FirstOrDefault(p => p.Name == productName);
        }

        /// <summary>
        /// The AddProduct
        /// </summary>
        /// <param name="product">The product<see cref="Product"/></param>
        /// <returns>The <see cref="Product"/></returns>
        public Product AddProduct(Product product)
        {
            GetProducts();
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
            SaveData(_products, _productFile);
            return product;
        }

        private void SaveData(Object o, string fileName)
        {
            try
            {
                var result = JsonConvert.SerializeObject(o);
                File.WriteAllText(fileName, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// The RemoveProduct
        /// </summary>
        /// <param name="product">The product<see cref="Product"/></param>
        public void RemoveProduct(Product product)
        {
            GetProducts();
            _products.Remove(product);
            SaveData(_products, _productFile);
        }

        /// <summary>
        /// The GetStock
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <returns>The <see cref="ProductItem"/></returns>
        public ProductItem GetStock(int id)
        {
            GetStocks();
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
            SaveData(_stock, _stockFile);
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
            SaveData(_stock, _stockFile);
        }

        public string FindAllProduct(bool isOnlyInStock)
        {
            GetProducts();
            GetStocks();
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
            GetStocks();
            var productItem = GetStock(productId);
            productItem.Count -= count;
            if (productItem.Count == 0)
            {
                _stock.Remove(productItem);
            }
            SaveData(_stock, _stockFile);
        }
    }
}
