namespace WXT.SuperMarket.Business.Services
{
    using WXT.SuperMarket.Data.Entities;

    /// <summary>
    /// Defines the <see cref="CustomerService" />
    /// </summary>
    public class CustomerService
    {
        /// <summary>
        /// The GetShoppingCart
        /// </summary>
        /// <returns>The <see cref="ShoppingCart"/></returns>
        public ShoppingCart GetShoppingCart()
        {
            return new ShoppingCart();
        }

        /// <summary>
        /// The AddtoCart
        /// </summary>
        public void AddtoCart()
        {
        }

        /// <summary>
        /// The TakeFromCart
        /// </summary>
        public void TakeFromCart()
        {
        }

        /// <summary>
        /// The CheckOut
        /// </summary>
        public void CheckOut()
        {
        }
    }
}
