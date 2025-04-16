using Ardalis.Result;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Server.Products.Application.Features.Products.CreateProduct;

namespace Server.Products.EndPoints.Products.Create
{
    public class CreateProductsEndPoint : Endpoint<CreateProductsRequest, CreateProductsResponse>
    {
        public override void Configure()
        {
            Post("/");
            Group<ProductsGroup>();
            AllowAnonymous();
            Description(d => d
                .Produces<CreateProductsResponse>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status409Conflict));
        }

        public override async Task HandleAsync(CreateProductsRequest req, CancellationToken ct)
        {
            var command = new CreateProductsCommand(req.Name, req.Description ?? "No description", req.Price, req.StockQuantity);
            var result = await command.ExecuteAsync(ct);

            if (result.IsSuccess)
            {
                var response = new CreateProductsResponse(
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
