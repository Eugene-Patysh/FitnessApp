using FitnessApp.Logic.Models;
using FluentValidation;

namespace FitnessApp.Logic.Validators
{
    public class ProductSubCategoryValidator : AbstractValidator<ProductSubCategoryDto>
    {
        public ProductSubCategoryValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o).NotNull().WithMessage("Product subcategory can't be null.");

            RuleFor(o => o.Title)
                .Must(t => !string.IsNullOrEmpty(t) && t.All(char.IsLetter)).WithMessage("Product subcategory title can't be null and must contains only letters.")
                .MaximumLength(30).WithMessage("Length of product subcategory title can't be more than 30 symbols.");

            RuleFor(o => o.ProductCategoryId).NotNull().GreaterThan(0).WithMessage("Id of product category can't be null and must be greather than zero.");

            RuleSet("AddProductSubCategory", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage("Product subcategory Id must be null.");
            });

            RuleSet("UpdateProductSubCategory", () =>
            {
                RuleFor(o => o.Id).NotNull().GreaterThan(0).WithMessage("Product subcategory Id can't be null and must be greather than zero.");
            });
        }
    }
}
