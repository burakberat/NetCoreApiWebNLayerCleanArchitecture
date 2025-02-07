using App.Application.Contracts.Persistence;
using FluentValidation;

namespace App.Application.Features.Products.Create
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        //private readonly IProductRepository _productRepository; //1.yol kullanılırsa açılacak.
        public CreateProductRequestValidator(IProductRepository productRepository)
        {
            //_productRepository = productRepository; 1. yol için

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(3, 10).WithMessage("Product name must be between 3 and 10 character.");
            //.Must(MustUniqueProductName).WithMessage("Product name already exist."); //1.yol (diğer yol service kısmında yapılıyor.)

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be bigger than 0.");

            RuleFor(x => x.Stock)
                .InclusiveBetween(1, 100).WithMessage("Stock must be between 1 and 100.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category id must be bigger than 0.");
        }

        //1. yol
        //private bool MustUniqueProductName(string name)
        //{
        //    return !_productRepository.Where(x => x.Name == name).Any();
        //}
    }
}