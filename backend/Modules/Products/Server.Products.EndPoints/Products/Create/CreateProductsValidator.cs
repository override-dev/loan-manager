using FluentValidation;

namespace Server.Products.EndPoints.Products.Create
{
    internal class CreateProductsValidator : AbstractValidator<CreateProductsRequest>
    {
        public CreateProductsValidator()
        {
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.StockQuantity).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
