using Ardalis.Result;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Server.Loan.Application.Features.Products.CreateProduct;

namespace Server.Loan.EndPoints.Loan.Create
{
    public class CreateLoanEndPoint : Endpoint<CreateLoanRequest, CreateLoanResponse>
    {
        public override void Configure()
        {
            Post("/");
            Group<LoanGroup>();
            AllowAnonymous();
            Description(d => d
                .Produces<CreateLoanResponse>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status409Conflict));
        }

        public override async Task HandleAsync(CreateLoanRequest req, CancellationToken ct)
        {
            var command = new CreateLoanCommand(req.Name, req.Description ?? "No description", req.Price, req.StockQuantity);
            var result = await command.ExecuteAsync(ct);

            if (result.IsSuccess)
            {
                var response = new CreateLoanResponse(
                    result.Value.Id,
                    result.Value.Name,
                    result.Value.Description,
                    result.Value.Price,
                    result.Value.StockQuantity,
                    result.Value.CreatedAt);

                await SendAsync(response, StatusCodes.Status201Created, ct);
                return;
            }

            switch (result.Status)
            {
                case ResultStatus.Invalid:
                    foreach (var error in result.ValidationErrors)
                    {
                        AddError(error.Identifier, error.ErrorMessage);
                    }
                    await SendErrorsAsync(StatusCodes.Status400BadRequest, ct);
                    break;

                case ResultStatus.NotFound:
                    AddError("Product not found");
                    await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
                    break;

                case ResultStatus.Conflict:
                    AddError("Conflict creating the product");
                    await SendErrorsAsync(StatusCodes.Status409Conflict, ct);
                    break;

                default:
                    AddError("Error processing the request");
                    await SendErrorsAsync(StatusCodes.Status500InternalServerError, ct);
                    break;
            }
        }
    }
}
