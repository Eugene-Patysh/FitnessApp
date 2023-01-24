using FitnessApp.Logic.Models;
using FluentValidation;

namespace FitnessApp.Logic.Validators
{
    public class ProductCategoryValidator : AbstractValidator<ProductCategoryDto>
    {
        public ProductCategoryValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o).NotNull().WithMessage("Product category can't be null.");

            RuleFor(o => o.Title)
                .Must(t => !string.IsNullOrEmpty(t) && t.All(char.IsLetter)).WithMessage("Product category title can't be null and must contains only letters.")
                .MaximumLength(30).WithMessage("Length of product category title can't be more than 30 symbols.");

            RuleSet("AddProductCategory", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage("Product category Id must be null.");
            });

            RuleSet("UpdateProductCategory", () =>
            {
                RuleFor(o => o.Id).NotNull().GreaterThan(0).WithMessage("Product category Id can't be null and must be greather than zero.");
            });
        }
    }
}
