using Ardalis.Result;
using FastEndpoints;

namespace Server.Products.Application.Features.Products.CreateProduct
{
    internal record CreateProductsCommand(
        string Name,
        string Description,
        decimal Price,
        int StockQuantity) : ICommand<Result<CreateProductsCommandResponse>>;
}