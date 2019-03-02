namespace WXT.SuperMarket.Data.Repository
{
    using WXT.SuperMarket.Data.Entities;

    /// <summary>
    /// Defines the <see cref="ICustomerRepository" />
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// The AddCustomer
        /// </summary>
        /// <param name="customer">The customer<see cref="Customer"/></param>
        /// <returns>The <see cref="Customer"/></returns>
        Customer AddCustomer(Customer customer);

        /// <summary>
        /// The CustomerValidity
        /// </summary>
        /// <param name="userName">The userName<see cref="string"/></param>
        /// <param name="password">The password<see cref="string"/></param>
        /// <returns>The <see cref="Customer"/></returns>
        Customer CustomerValidity(string userName, string password);

        /// <summary>
        /// The FindShoppingCart
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <returns>The <see cref="ShoppingCart"/></returns>
        ShoppingCart FindShoppingCart(int id);

        /// <summary>
        /// The FindCustomer
        /// </summary>
        /// <param name="userName">The userName<see cref="string"/></param>
        /// <returns>The <see cref="Customer"/></returns>
        Customer FindCustomer(string userName);

        /// <summary>
        /// The DeleteCustomer
        /// </summary>
        /// <param name="userName">The userName<see cref="string"/></param>
        void DeleteCustomer(string userName);

        /// <summary>
        /// The DeleteCustomer
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        void DeleteCustomer(int id);

        /// <summary>
        /// The AddToCart
        /// </summary>
        /// <param name="shoppingCartId">The shoppingCartId<see cref="int"/></param>
        /// <param name="productId">The productId<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        void AddToCart(int shoppingCartId, int productId, int count);

        /// <summary>
        /// The RemoveFromCart
        /// </summary>
        /// <param name="shoppingCart">The shoppingCart<see cref="ShoppingCart"/></param>
        /// <param name="productId">The productId<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        /// <returns>The <see cref="int"/></returns>
        int RemoveFromCart(int shoppingCartId, int productId, int count);

        /// <summary>
        /// The ClearCart
        /// </summary>
        /// <param name="shoppingCart">The shoppingCart<see cref="ShoppingCart"/></param>
        void ClearCart(int shoppingCartId);

        /// <summary>
        /// The CheckOut
        /// </summary>
        /// <param name="shoppingCart">The shoppingCart<see cref="ShoppingCart"/></param>
        /// <returns>The <see cref="Receipt"/></returns>
        Receipt CheckOut(int shoppingCartId);

        /// <summary>
        /// The AddNewReceipt
        /// </summary>
        /// <param name="receipt">The receipt<see cref="Receipt"/></param>
        /// <returns>The <see cref="Receipt"/></returns>
        Receipt AddNewReceipt(Receipt receipt);
    }
}
