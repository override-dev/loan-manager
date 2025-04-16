using Ardalis.Result;
using FastEndpoints;

namespace Server.Loan.Application.Features.Products.CreateProduct
{
    internal record CreateLoanCommand(
        string Name,
        string Description,
        decimal Price,
        int StockQuantity) : ICommand<Result<CreateLoanCommandResponse>>;
}