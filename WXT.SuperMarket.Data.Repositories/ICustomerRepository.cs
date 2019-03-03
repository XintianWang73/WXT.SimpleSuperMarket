namespace WXT.SuperMarket.Data.Repository
{
    using WXT.SuperMarket.Data.Entities;

    public interface ICustomerRepository
    {
        Customer AddCustomer(Customer customer);

        Customer CustomerValidity(string userName, string password);

        ShoppingCart FindShoppingCart(int id);

        Customer FindCustomer(string userName);

        void DeleteCustomer(string userName);

        void DeleteCustomer(int id);

        void AddToCart(int shoppingCartId, int productId, int count);

        int RemoveFromCart(int shoppingCartId, int productId, int count);

        void ClearCart(int shoppingCartId);

        Receipt CheckOut(int shoppingCartId);

        Receipt AddNewReceipt(Receipt receipt);
    }
}
