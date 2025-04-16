namespace Server.Products.Application.Features.Products.CreateProduct
{
    internal record CreateProductsCommandResponse(Guid Id, string Name, string? Description, decimal Price, int StockQuantity, DateTime CreatedAt);
}
