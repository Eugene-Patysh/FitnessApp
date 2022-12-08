using FitnessApp.Logic.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Validators
{
    public class NutrientCategoryValidator : AbstractValidator<NutrientCategoryDto>
    {
        public NutrientCategoryValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o).NotNull().WithMessage("Nutrient category can't be null.");

            RuleSet("AddNutrientCategory", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage("Nutrient category Id must be null.");
            });

            RuleSet("UpdateNutrientCategory", () =>
            {
                RuleFor(o => o.Id).NotNull().WithMessage("Nutrient category Id can't be null.");
            });

            RuleFor(o => o.Title)
                .Must(t => !string.IsNullOrEmpty(t)).WithMessage("Nutrient category title can't be null.")
                .MaximumLength(30).WithMessage("Length of nutrient category title can't be more than 30 symbols.");
        }
    }
}
