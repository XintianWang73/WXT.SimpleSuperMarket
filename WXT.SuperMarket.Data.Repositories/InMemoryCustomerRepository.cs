namespace WXT.SuperMarket.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using WXT.SuperMarket.Data.Entities;

    public class InMemoryCustomerRepository : ICustomerRepository
    {
        private static readonly List<ShoppingCart> _shoppingCarts = new List<ShoppingCart>();

        private static readonly List<Customer> _customers = new List<Customer>();

        private static readonly List<Receipt> _receipts = new List<Receipt>();

        private static readonly IMarketRepository _marketRepository = new InMemoryMarketRepository();

        public Customer AddCustomer(Customer customer)
        {
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

            ShoppingCart shoppingCart = new ShoppingCart
            {
                CustomerId = customer.Id
            };

            _shoppingCarts.Add(shoppingCart);

            return customer;
        }

        public Customer CustomerValidity(string userName, string password)
        {
            return _customers.FirstOrDefault(c => c.UserName == userName && c.PassWord == password);
        }

        public ShoppingCart FindShoppingCart(int id)
        {
            return _shoppingCarts.FirstOrDefault(s => s.CustomerId == id);
        }

        public Customer FindCustomer(string userName)
        {
            return _customers.FirstOrDefault(c => c.UserName == userName);
        }

        private Customer FindCustomer(int id)
        {
            return _customers.FirstOrDefault(c => c.Id == id);
        }

        public void DeleteCustomer(string userName)
        {
            var customer = FindCustomer(userName);
            if (customer != null)
            {
                _customers.Remove(customer);
                _shoppingCarts.Remove(_shoppingCarts.FirstOrDefault(s => s.CustomerId == customer.Id));
            }
        }

        public void DeleteCustomer(int id)
        {
            var customer = FindCustomer(id);
            if (customer != null)
            {
                _customers.Remove(customer);
                _shoppingCarts.Remove(_shoppingCarts.FirstOrDefault(s => s.CustomerId == customer.Id));
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
            }
            return realCount;
        }

        public void ClearCart(int shoppingCartId)
        {
            FindShoppingCart(shoppingCartId).ItemList.Clear();
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
            return AddNewReceipt(receipt);
        }

        public Receipt AddNewReceipt(Receipt receipt)
        {
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
            return receipt;
        }
    }
}
