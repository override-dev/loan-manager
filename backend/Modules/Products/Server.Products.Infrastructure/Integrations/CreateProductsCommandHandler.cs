using Ardalis.Result;
using FastEndpoints;
using Server.Products.Application.Features.Products.CreateProduct;
using Server.Products.Domain.Interfaces;

namespace Server.Products.Infrastructure.Integrations
{
    internal class CreateProductsCommandHandler(IProductsService productService) : CommandHandler<CreateProductsCommand, Result<CreateProductsCommandResponse>>
    {
        public override async Task<Result<CreateProductsCommandResponse>> ExecuteAsync(CreateProductsCommand command, CancellationToken ct = default)
        {
            var product = await productService.CreateProductAsync(
                command.Name,
                command.Description,
                command.Price,
                command.StockQuantity);

            if (product == null)
            {
                return Result<CreateProductsCommandResponse>.Error("Product creation failed");
            }

            var response = new CreateProductsCommandResponse(
                 product.Id,
                 product.Name,
                 product.Description,
                 product.Price,
                 product.StockQuantity,
                 product.CreatedAt);

            return Result<CreateProductsCommandResponse>.Success(response);
        }
    }
}
