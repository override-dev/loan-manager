using Ardalis.Result;
using FastEndpoints;
using Server.Loan.Application.Features.Products.CreateProduct;
using Server.Loan.Domain.Interfaces;

namespace Server.Loan.Infrastructure.Integrations
{
    internal class CreateLoanCommandHandler(ILoanService productService) : CommandHandler<CreateLoanCommand, Result<CreateLoanCommandResponse>>
    {
        public override async Task<Result<CreateLoanCommandResponse>> ExecuteAsync(CreateLoanCommand command, CancellationToken ct = default)
        {
            var product = await productService.CreateLoanAsync(
                command.Name,
                command.Description,
                command.Price,
                command.StockQuantity);

            if (product == null)
            {
                return Result<CreateLoanCommandResponse>.Error("Product creation failed");
            }

            var response = new CreateLoanCommandResponse(
                 product.Id,
                 product.Name,
                 product.Description,
                 product.Price,
                 product.StockQuantity,
                 product.CreatedAt);

            return Result<CreateLoanCommandResponse>.Success(response);
        }
    }
}
