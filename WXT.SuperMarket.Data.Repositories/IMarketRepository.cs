namespace WXT.SuperMarket.Data.Repository
{
    using WXT.SuperMarket.Data.Entities;

    /// <summary>
    /// Defines the <see cref="IMarketRepository" />
    /// </summary>
    public interface IMarketRepository
    {
        /// <summary>
        /// The FindProduct
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <returns>The <see cref="Product"/></returns>
        Product FindProduct(int id);

        /// <summary>
        /// The FindProduct
        /// </summary>
        /// <param name="productName">The productName<see cref="string"/></param>
        /// <returns>The <see cref="Product"/></returns>
        Product FindProduct(string productName);

        /// <summary>
        /// The AddProduct
        /// </summary>
        /// <param name="product">The product<see cref="Product"/></param>
        /// <returns>The <see cref="Product"/></returns>
        Product AddProduct(Product product);

        /// <summary>
        /// The RemoveProduct
        /// </summary>
        /// <param name="product">The product<see cref="Product"/></param>
        void RemoveProduct(Product product);

        /// <summary>
        /// The GetStock
        /// </summary>
        /// <param name="id">The id<see cref="int"/></param>
        /// <returns>The <see cref="ProductItem"/></returns>
        ProductItem GetStock(int id);

        /// <summary>
        /// The AddToStock
        /// </summary>
        /// <param name="productId">The productId<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        void AddToStock(int productId, int count);

        /// <summary>
        /// The RemoveFromStock
        /// </summary>
        /// <param name="productItem">The productItem<see cref="ProductItem"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        void RemoveFromStock(ProductItem productItem, int count);

        /// <summary>
        /// The FindAllProduct
        /// </summary>
        /// <param name="isOnlyInStock">The isOnlyInStock<see cref="bool"/></param>
        /// <returns>The <see cref="string"/></returns>
        string FindAllProduct(bool isOnlyInStock);

        /// <summary>
        /// The RemoveFromStock
        /// </summary>
        /// <param name="productId">The productId<see cref="int"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        void RemoveFromStock(int productId, int count);
    }
}
