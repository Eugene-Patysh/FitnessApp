using FitnessApp.Logic.Models;
using FluentValidation;

namespace FitnessApp.Logic.Validators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o).NotNull().WithMessage("Product can't be null.");

            RuleFor(o => o.Title)
                .Must(t => !string.IsNullOrEmpty(t) && t.All(char.IsLetter)).WithMessage("Product title can't be null and must contains only letters.")
                .MaximumLength(30).WithMessage("Length of product title can't be more than 30 symbols.");

            RuleFor(o => o.ProductSubCategoryId).NotNull().GreaterThan(0).WithMessage("Id of product subcategory can't be null and must be greather than zero.");

            RuleSet("AddProduct", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage("Product Id must be null.");
            });

            RuleSet("UpdateProduct", () =>
            {
                RuleFor(o => o.Id).NotNull().GreaterThan(0).WithMessage("Product Id can't be null and must be greather than zero."); 
            });
        }
    }
}
