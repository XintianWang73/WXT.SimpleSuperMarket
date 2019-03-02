namespace WXT.SuperMarket.Data.Repository
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using WXT.SuperMarket.Data.Entities;

    /// <summary>
    /// Defines the <see cref="JsonCustomerRepository" />
    /// </summary>
    public class JsonCustomerRepository : ICustomerRepository
    {
        /// <summary>
        /// Defines the _shoppingCartFile
        /// </summary>
        private const string _shoppingCartFile = "shoppingCarts.json";

        /// <summary>
        /// Defines the _customerFile
        /// </summary>
        private const string _customerFile = "customers.json";

        /// <summary>
        /// Defines the _receiptFile
        /// </summary>
        private const string _receiptFile = "receipts.json";

        /// <summary>
        /// Defines the _shoppingCarts
        /// </summary>
        private static List<ShoppingCart> _shoppingCarts;

        /// <summary>
        /// Defines the _customers
        /// </summary>
        private static List<Customer> _customers;

        /// <summary>
        /// Defines the _receipts
        /// </summary>
        private static List<Receipt> _receipts;

        /// <summary>
        /// Defines the _shoppingCarts
        /// </summary>
        private void GetShoppingCarts()
        {
            try
            {
                _shoppingCarts = JsonConvert.DeserializeObject<List<ShoppingCart>>(File.ReadAllText(_shoppingCartFile));
            }
            catch
            {
                _shoppingCarts = new List<ShoppingCart>();
            }
        }

        /// <summary>
        /// Defines the _customers
        /// </summary>
        private void GetCustomers()
        {
            try
            {
                _customers = JsonConvert.DeserializeObject<List<Customer>>(File.ReadAllText(_customerFile));
            }
            catch
            {
                _customers = new List<Customer>();
            }
        }

        /// <summary>
        /// Defines the _receipts
        /// </summary>
        private void GetReceipts()
        {
            try
            {
                _receipts = JsonConvert.DeserializeObject<List<Receipt>>(File.ReadAllText(_receiptFile));
            }
            catch
            {
                _receipts = new List<Receipt>();
            }
        }

        /// <summary>
        /// Defines the _marketRepository
        /// </summary>
        private static readonly IMarketRepository _marketRepository = new JsonMarketRepository();

        /// <summary>
        /// The AddCustomer
        /// </summary>
        /// <param name="customer">The customer<see cref="Customer"/></param>
        /// <returns>The <see cref="Customer"/></returns>
        public Customer AddCustomer(Customer customer)
        {
            GetCustomers();
            int maxId = 0;
            try
            {
                maxId = _customers.Max(c => c.Id);
            }
            catch
            {
            }
            customer.Id = maxId + 1;

            customer.PassWord = Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(customer.PassWord)));

            _customers.Add(customer);
            SaveData(_customers, _customerFile);

            ShoppingCart shoppingCart = new ShoppingCart
            {
                CustomerId = customer.Id
            };

            GetShoppingCarts();
            _shoppingCarts.Add(shoppingCart);
            SaveData(_shoppingCarts, _shoppingCartFile);
            return customer;
        }

        /// <summary>
        /// The SaveData
        /// </summary>
        /// <param name="o">The o<see cref="Object"/></param>
        /// <param name="fileName">The fileName<see cref="string"/></param>
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
        /// The CustomerValidity
        /// </summary>
        /// <param name="userName">The userName<see cref="string"/></param>
        /// <param name="password">The password<see cref="string"/></param>
        /// <returns>The <see cref="Customer"/></returns>
        public Customer CustomerValidity(string userName, string password)
        {
            GetCustomers();
            password = Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
            return _customers.FirstOrDefault(c => c.UserName == userName && c.PassWord == password);
        }

        /// <summary>
        /// The FindShoppingCart
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <returns>The <see cref="ShoppingCart"/></returns>
        public ShoppingCart FindShoppingCart(int id)
        {
            GetShoppingCarts();
            return _shoppingCarts.FirstOrDefault(s => s.CustomerId == id);
        }

        /// <summary>
        /// The FindCustomer
        /// </summary>
        /// <param name="userName">The userName<see cref="string"/></param>
        /// <returns>The <see cref="Customer"/></returns>
        public Customer FindCustomer(string userName)
        {
            GetCustomers();
            return _customers.FirstOrDefault(c => c.UserName == userName);
        }

        /// <summary>
        /// The FindCustomer
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <returns>The <see cref="Customer"/></returns>
        private Customer FindCustomer(int id)
        {
            GetCustomers();
            return _customers.FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// The DeleteCustomer
        /// </summary>
        /// <param name="userName">The userName<see cref="string"/></param>
        public void DeleteCustomer(string userName)
        {
            var customer = FindCustomer(userName);
            if (customer != null)
            {
                _customers.Remove(customer);
                SaveData(_customers, _customerFile);
                GetShoppingCarts();
                _shoppingCarts.Remove(_shoppingCarts.FirstOrDefault(s => s.CustomerId == customer.Id));
                SaveData(_shoppingCarts, _shoppingCartFile);
            }
        }

        /// <summary>
        /// The DeleteCustomer
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        public void DeleteCustomer(int id)
        {
            var customer = FindCustomer(id);
            if (customer != null)
            {
                _customers.Remove(customer);
                SaveData(_customers, _customerFile);
                GetShoppingCarts();
                _shoppingCarts.Remove(_shoppingCarts.FirstOrDefault(s => s.CustomerId == customer.Id));
                SaveData(_shoppingCarts, _shoppingCartFile);
            }
        }

        /// <summary>
        /// The AddToCart
        /// </summary>
        /// <param name="shoppingCartId">The shoppingCartId<see cref="int"/></param>
        /// <param name="productId">The productId<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        public void AddToCart(int shoppingCartId, int productId, int count)
        {
            var shoppingCart = FindShoppingCart(shoppingCartId);
            var resultItem = shoppingCart.ItemList.FirstOrDefault(i => i.ProductId == productId);
            if (resultItem == null)
            {
                shoppingCart.ItemList.Add(new ProductItem { ProductId = productId, Count = count });
            }
            else
            {
                resultItem.Count += count;
            }
            SaveData(_shoppingCarts, _shoppingCartFile);
        }

        /// <summary>
        /// The RemoveFromCart
        /// </summary>
        /// <param name="shoppingCartId">The shoppingCartId<see cref="int"/></param>
        /// <param name="productId">The productId<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        /// <returns>The <see cref="int"/></returns>
        public int RemoveFromCart(int shoppingCartId, int productId, int count)
        {
            var shoppingCart = FindShoppingCart(shoppingCartId);
            var resultItem = shoppingCart.ItemList.FirstOrDefault(i => i.ProductId == productId);
            int realCount = 0;
            if (resultItem != null)
            {
                realCount = Math.Min(resultItem.Count, count);
                resultItem.Count -= count;
                if (resultItem.Count <= 0)
                {
                    shoppingCart.ItemList.Remove(resultItem);
                }
                SaveData(_shoppingCarts, _shoppingCartFile);
            }
            return realCount;
        }

        /// <summary>
        /// The ClearCart
        /// </summary>
        /// <param name="shoppingCartId">The shoppingCartId<see cref="int"/></param>
        public void ClearCart(int shoppingCartId)
        {
            var shoppingCart = FindShoppingCart(shoppingCartId);
            shoppingCart.ItemList.Clear();
            SaveData(_shoppingCarts, _shoppingCartFile);
        }

        /// <summary>
        /// The CheckOut
        /// </summary>
        /// <param name="shoppingCartId">The shoppingCartId<see cref="int"/></param>
        /// <returns>The <see cref="Receipt"/></returns>
        public Receipt CheckOut(int shoppingCartId)
        {
            var shoppingCart = FindShoppingCart(shoppingCartId);
            Receipt receipt = new Receipt
            {
                TransactionTime = DateTimeOffset.UtcNow,
                ShoppingList = new List<TransactionItem>()
            };

            var items = receipt.ShoppingList;

            foreach (var productItem in shoppingCart.ItemList)
            {
                var product = _marketRepository.FindProduct(productItem.ProductId);
                items.Add(new TransactionItem
                {
                    Id = productItem.ProductId,
                    Name = product.Name,
                    Price = product.Price,
                    Count = productItem.Count
                });
                _marketRepository.RemoveFromStock(productItem.ProductId, productItem.Count);
            }
            shoppingCart.ItemList.Clear();
            SaveData(_shoppingCarts, _shoppingCartFile);
            return AddNewReceipt(receipt);
        }

        /// <summary>
        /// The AddNewReceipt
        /// </summary>
        /// <param name="receipt">The receipt<see cref="Receipt"/></param>
        /// <returns>The <see cref="Receipt"/></returns>
        public Receipt AddNewReceipt(Receipt receipt)
        {
            GetReceipts();
            int maxId = 0;
            try
            {
                maxId = _receipts.Max(c => c.Id);
            }
            catch
            {
            }
            receipt.Id = maxId + 1;

            _receipts.Add(receipt);
            SaveData(_receipts, _receiptFile);
            return receipt;
        }
    }
}
