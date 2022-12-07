using FitnessApp.Logic.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Validators
{
    public class NutrientValidator : AbstractValidator<NutrientDto>
    {
        public NutrientValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o).NotNull().WithMessage("Nutrient can't be null.");

            RuleSet("AddNutrient", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage("Nutrient Id must be null.");  
            });

            RuleSet("UpdateNutrient", () =>
            {
                RuleFor(o => o.Id).NotNull().WithMessage("Nutrient Id can't be null.");
            });

            RuleFor(o => o.Title)
                .Must(p => !string.IsNullOrEmpty(p)).WithMessage("Nutrient title can't be null.")
                .MaximumLength(30).WithMessage("Length of nutrient title can't be more than 30 symbols.");

            RuleFor(o => o.NutrientCategoryId).NotNull().WithMessage("Id of nutrient category can't be null.");

            RuleFor(o => o.DailyDose).Must(p => p > 0).WithMessage("DailyDose must be more than zero");
        }
    }
}
