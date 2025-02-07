using App.Application.Contracts.Persistence;
using FluentValidation;

namespace App.Application.Features.Products.Update
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator(IProductRepository productRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(3, 10).WithMessage("Product name must be between 3 and 10 character.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be bigger than 0.");

            RuleFor(x => x.Stock)
                .InclusiveBetween(1, 100).WithMessage("Stock must be between 1 and 100.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category id must be bigger than 0.");
        }
    }
}