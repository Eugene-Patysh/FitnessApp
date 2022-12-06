using FitnessApp.Logic.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Validators
{
    public class ProductNutrientValidator : AbstractValidator<ProductNutrientDto>
    {
        public ProductNutrientValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleSet("AddProductNutrient", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage("Product-Nutrient Id must be null.");
                RuleFor(o => o.ProductId).Null().WithMessage("Product Id must be null.");
                RuleFor(o => o.NutrientId).Null().WithMessage("Nutrient Id must be null.");
                RuleFor(o => o.TreatingTypeId).Null().WithMessage("Treating type Id must be null.");
            });

            RuleSet("UpdateProductNutrient", () =>
            {
                RuleFor(o => o.Id).NotNull().WithMessage("Product-Nutrient Id can't be null.");
                RuleFor(o => o.ProductId).NotNull().WithMessage("Product Id can't be null.");
                RuleFor(o => o.NutrientId).NotNull().WithMessage("Nutrient Id can't be null.");
                RuleFor(o => o.TreatingTypeId).NotNull().WithMessage("Treating type Id can't be null.");
            });

            RuleFor(o => o.Quality).Must(p => p >= 0).WithMessage("DailyDose must be equals or more than zero");
        }
    }
}
