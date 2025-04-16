

using Server.Products.Domain.Entities;
using Server.Products.Domain.Interfaces;

internal class ProductsService : IProductsService
{
    public async Task<Products> CreateProductAsync(string name, string description, decimal price, int stockQuantity)
    {
        var item = new Products(Guid.NewGuid(), name, description, price, stockQuantity, DateTime.UtcNow);

        await Task.Delay(100);

        return item;
    }
}
