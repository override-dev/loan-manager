namespace Server.Products.Domain.Interfaces
{
    internal interface IProductsService
    {
        Task<Entities.Products> CreateProductAsync(string name, string description, decimal price, int stockQuantity);
    }
}
