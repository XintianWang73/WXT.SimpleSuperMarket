namespace WXT.SuperMarket.Business.Services
{
    using System;
    using WXT.SuperMarket.Data.Entities;
    using WXT.SuperMarket.Data.Repository;

    /// <summary>
    /// Defines the <see cref="CustomerService" />
    /// </summary>
    public class CustomerService
    {
        /// <summary>
        /// Defines the _customerRepository
        /// </summary>
        private readonly InMemoryCustomerRepository _customerRepository = new InMemoryCustomerRepository();

        /// <summary>
        /// Defines the _marketRepository
        /// </summary>
        private readonly InMemoryMarketRepository _marketRepository = new InMemoryMarketRepository();

        /// <summary>
        /// Defines the _myShoppingCart
        /// </summary>
        private ShoppingCart _myShoppingCart = null;

        /// <summary>
        /// The RegisterNewCustomer
        /// </summary>
        /// <param name="userName">The userName<see cref="string"/></param>
        /// <param name="passWord">The passWord<see cref="string"/></param>
        /// <returns>The <see cref="Customer"/></returns>
        public Customer RegisterNewCustomer(string userName, string passWord)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("userName", "UserName cannot be null, enmpty or only white spaces.");
            }
            if (string.IsNullOrWhiteSpace(passWord))
            {
                throw new ArgumentNullException("password", "Password cannot be null, enmpty or only white spaces.");
            }
            if (_customerRepository.FindCustomer(userName) != null)
            {
                throw new InvalidOperationException($"UserName {userName} has been used.");
            }
            var customer = new Customer
            {
                UserName = userName,
                PassWord = passWord
            };
            return _customerRepository.AddCustomer(customer);
        }

        /// <summary>
        /// The DeleteCustomer
        /// </summary>
        /// <param name="userName">The userName<see cref="string"/></param>
        /// <param name="passWord">The passWord<see cref="string"/></param>
        public void DeleteCustomer(string userName, string passWord)
        {
            if (_myShoppingCart != null)
            {
                _customerRepository.DeleteCustomer(userName);
                Logout();
            }
        }

        /// <summary>
        /// The Login
        /// </summary>
        /// <param name="userName">The userName<see cref="string"/></param>
        /// <param name="passWord">The passWord<see cref="string"/></param>
        public void Login(string userName, string passWord)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("userName", "UserName cannot be null, enmpty or only white spaces.");
            }
            if (string.IsNullOrWhiteSpace(passWord))
            {
                throw new ArgumentNullException("password", "Password cannot be null, enmpty or only white spaces.");
            }
            var customer = _customerRepository.CustomerValidity(userName, passWord);

            if (customer != null)
            {
                _myShoppingCart = _customerRepository.FindShoppingCart(customer);
            }
        }

        /// <summary>
        /// The Logout
        /// </summary>
        public void Logout()
        {
            _myShoppingCart = null;
        }

        /// <summary>
        /// The AddtoCart
        /// </summary>
        /// <param name="productId">The productId<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        public void AddtoCart(int productId, int count = 1)
        {
            CheckLoginStatus();

            ProductItem stock = _marketRepository.GetStock(productId);
            if (stock == null)
            {
                throw new InvalidOperationException($"Product {productId} is out of stock.");
            }
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count", "Count cannot less than 1.");
            }

            if (stock.Count < count)
            {
                throw new ArgumentOutOfRangeException("count", $"There is no enough product in stock. left {stock.Count}, need {count}");
            }
            _customerRepository.AddToCart(_myShoppingCart, productId, count);
        }

        /// <summary>
        /// The TakeFromCart
        /// </summary>
        /// <param name="productId">The productId<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        public void TakeFromCart(int productId, int count = 1)
        {
            CheckLoginStatus();

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count", "Count cannot less than 1.");
            }
            _customerRepository.RemoveFromCart(_myShoppingCart, productId, count);
        }

        /// <summary>
        /// The ClearCart
        /// </summary>
        public void ClearCart()
        {
            CheckLoginStatus();

            _customerRepository.ClearCart(_myShoppingCart);
        }

        /// <summary>
        /// The CheckLoginStatus
        /// </summary>
        private void CheckLoginStatus()
        {
            if (_myShoppingCart == null)
            {
                throw new InvalidOperationException($"Customer needs login before other operations.");
            }
        }

        /// <summary>
        /// The CheckOut
        /// </summary>
        public void CheckOut()
        {
            CheckLoginStatus();
        }
    }
}
