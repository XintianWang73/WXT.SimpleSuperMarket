namespace WXT.SuperMarket.Data.Repository
{
    using System.Collections.Generic;
    using WXT.SuperMarket.Data.Entities;
    using System.Linq;
    using System;

    /// <summary>
    /// Defines the <see cref="InMemoryCustomerRepository" />
    /// </summary>
    public class InMemoryCustomerRepository
    {
        private static readonly List<ShoppingCart> _shoppingCarts = new List<ShoppingCart>();

        private static readonly List<Customer> _customers = new List<Customer>();

        public Customer AddCustomer (Customer customer)
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

        public ShoppingCart FindShoppingCart(Customer customer)
        {
            return _shoppingCarts.FirstOrDefault(s => s.CustomerId == customer.Id);
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
                _customers.Remove(customer);
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



        public void AddToCart(ShoppingCart shoppingCart, int productId, int count)
        {
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

        public int RemoveFromCart(ShoppingCart shoppingCart, int productId, int count)
        {
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

        public void ClearCart(ShoppingCart shoppingCart)
        {
            shoppingCart.ItemList.Clear();
        }
    }
}
