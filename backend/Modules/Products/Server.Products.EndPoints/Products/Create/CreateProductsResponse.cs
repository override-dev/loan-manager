namespace Server.Products.EndPoints.Products.Create
{
    public record CreateProductsResponse(Guid Id, string Name, string? Description, decimal Price, int StockQuantity, DateTime CreatedAt);
}
