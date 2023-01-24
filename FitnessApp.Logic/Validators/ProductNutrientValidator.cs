using FitnessApp.Logic.Models;
using FluentValidation;

namespace FitnessApp.Logic.Validators
{
    public class ProductNutrientValidator : AbstractValidator<ProductNutrientDto>
    {
        public ProductNutrientValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o).NotNull().WithMessage("Product-Nutrient can't be null.");

            RuleFor(o => o.ProductId).NotNull().GreaterThan(0).WithMessage("Product Id can't be null and must be greather than zero.");

            RuleFor(o => o.NutrientId).NotNull().GreaterThan(0).WithMessage("Nutrient Id can't be null and must be greather than zero.");

            RuleFor(o => o.TreatingTypeId).NotNull().GreaterThan(0).WithMessage("Treating type Id can't be null and must be greather than zero.");

            RuleFor(o => o.Quality).Must(p => p >= 0).WithMessage("DailyDose must be equals or more than zero");

            RuleSet("AddProductNutrient", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage("Product-Nutrient Id must be null.");
            });

            RuleSet("UpdateProductNutrient", () =>
            {
                RuleFor(o => o.Id).NotNull().GreaterThan(0).WithMessage("Product-Nutrient Id can't be null and must be greather than zero.");
            });
        }
    }
}
