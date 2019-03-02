namespace WXT.SuperMarket.Business.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using WXT.SuperMarket.Data.Entities;
    using WXT.SuperMarket.Data.Repository;

    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository = new JsonCustomerRepository();

        private readonly IMarketRepository _marketRepository = new JsonMarketRepository();

        private int ShoppingCartId { get; set; }

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

        public string RegisterNewCustomer(string userName, string passWord)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("userName", "UserName cannot be null, enmpty or only white spaces.");
            }
            if (string.IsNullOrWhiteSpace(passWord))
            {
                throw new ArgumentNullException("password", "Password cannot be null, enmpty or only white spaces.");
            }
            var locker = LockFile("customer.lk");
            if (_customerRepository.FindCustomer(userName) != null)
            {
                UnlockFile(locker);
                throw new InvalidOperationException($"UserName {userName} has been used.");
            }
            var customer = new Customer
            {
                UserName = userName,
                PassWord = passWord
            };

            var locker2 = LockFile("cart.lk");
            var result = _customerRepository.AddCustomer(customer).ToString();
            UnlockFile(locker2);
            UnlockFile(locker);
            return result;
        }

        public void DeleteCustomer()
        {
            CheckLoginStatus();
            var id = ShoppingCartId;
            Logout();
            var locker = LockFile("customer.lk");
            var locker2 = LockFile("cart.lk");
            _customerRepository.DeleteCustomer(id);
            UnlockFile(locker2);
            UnlockFile(locker);
        }

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
                ShoppingCartId = customer.Id;
            }
            else
            {
                throw new InvalidOperationException("Cannot login with this username and password.");
            }
        }

        public void Logout()
        {
            ShoppingCartId = 0;
        }

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

            var locker = LockFile("cart.lk");
            _customerRepository.AddToCart(ShoppingCartId, productId, count);
            UnlockFile(locker);
        }

        public int TakeFromCart(int productId, int count = 1)
        {
            CheckLoginStatus();

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count", "Count cannot less than 1.");
            }
            var locker = LockFile("cart.lk");
            int result = _customerRepository.RemoveFromCart(ShoppingCartId, productId, count);
            UnlockFile(locker);
            return result;
        }

        public void ClearCart()
        {
            CheckLoginStatus();
            var locker = LockFile("cart.lk");
            _customerRepository.ClearCart(ShoppingCartId);
            UnlockFile(locker);
        }

        private void CheckLoginStatus()
        {
            if (ShoppingCartId <= 0)
            {
                throw new InvalidOperationException($"Customer needs login before other operations.");
            }
        }

        public string CheckOut()
        {
            CheckLoginStatus();
            var locker = LockFile("cart.lk");
            var shoppingCart = _customerRepository.FindShoppingCart(ShoppingCartId);
            if (shoppingCart.ItemList.Count == 0)
            {
                UnlockFile(locker);
                throw new InvalidOperationException("There is no item in the shopping cart.");
            }
            var locker2 = LockFile("stock.lk");
            var item = shoppingCart.ItemList.FirstOrDefault(i => i.Count > (_marketRepository.GetStock(i.ProductId)?.Count ?? 0));
            if (item != null)
            {
                UnlockFile(locker2);
                UnlockFile(locker);
                throw new InvalidOperationException($"There is no enough product in stock.");
            }
            var locker3 = LockFile("receipt.lk");
            var result = _customerRepository.CheckOut(ShoppingCartId).ToString();
            UnlockFile(locker3);
            UnlockFile(locker2);
            UnlockFile(locker);
            return result;
        }

        public string FindAllProduct(bool isOnlyInStock)
        {
            return _marketRepository.FindAllProduct(isOnlyInStock);
        }
    }
}
