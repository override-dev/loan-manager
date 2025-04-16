namespace Server.Products.Domain.Entities
{
    internal record Products(Guid Id, string Name, string Description, decimal Price, int StockQuantity, DateTime CreatedAt);
}
