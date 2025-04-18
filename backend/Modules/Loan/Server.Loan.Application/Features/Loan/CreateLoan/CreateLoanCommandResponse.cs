namespace Server.Loan.Application.Features.Products.CreateProduct;

internal record CreateLoanCommandResponse(Guid Id, string Name, string? Description, decimal Price, int StockQuantity, DateTime CreatedAt);
