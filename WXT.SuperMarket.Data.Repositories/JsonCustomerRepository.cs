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

    public class JsonCustomerRepository : ICustomerRepository
    {
        private const string _shoppingCartFile = "shoppingCarts.json";

        private const string _customerFile = "customers.json";

        private const string _receiptFile = "receipts.json";

        private static List<ShoppingCart> _shoppingCarts;

        private static List<Customer> _customers;

        private static List<Receipt> _receipts;

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

        private static readonly IMarketRepository _marketRepository = new JsonMarketRepository();

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

        public Customer CustomerValidity(string userName, string password)
        {
            GetCustomers();
            password = Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
            return _customers.FirstOrDefault(c => c.UserName == userName && c.PassWord == password);
        }

        public ShoppingCart FindShoppingCart(int id)
        {
            GetShoppingCarts();
            return _shoppingCarts.FirstOrDefault(s => s.CustomerId == id);
        }

        public Customer FindCustomer(string userName)
        {
            GetCustomers();
            return _customers.FirstOrDefault(c => c.UserName == userName);
        }

        private Customer FindCustomer(int id)
        {
            GetCustomers();
            return _customers.FirstOrDefault(c => c.Id == id);
        }

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

        public void ClearCart(int shoppingCartId)
        {
            var shoppingCart = FindShoppingCart(shoppingCartId);
            shoppingCart.ItemList.Clear();
            SaveData(_shoppingCarts, _shoppingCartFile);
        }

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
