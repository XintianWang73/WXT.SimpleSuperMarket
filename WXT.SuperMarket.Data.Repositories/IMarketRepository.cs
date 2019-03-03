namespace WXT.SuperMarket.Data.Repository
{
    using WXT.SuperMarket.Data.Entities;

    public interface IMarketRepository
    {
        Product FindProduct(int id);

        Product FindProduct(string productName);

        Product AddProduct(Product product);

        void RemoveProduct(Product product);

        ProductItem GetStock(int id);

        void AddToStock(int productId, int count);

        void RemoveFromStock(ProductItem productItem, int count);

        string FindAllProduct(bool isOnlyInStock);

        void RemoveFromStock(int productId, int count);
    }
}
