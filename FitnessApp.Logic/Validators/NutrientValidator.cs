using FitnessApp.Logic.Models;
using FluentValidation;

namespace FitnessApp.Logic.Validators
{
    public class NutrientValidator : AbstractValidator<NutrientDto>
    {
        public NutrientValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o).NotNull().WithMessage("Nutrient can't be null.");

            RuleFor(o => o.Title)
                .Must(p => !string.IsNullOrEmpty(p)).WithMessage("Nutrient title can't be null.")
                .Must(t => t.All(char.IsLetter)).WithMessage("Nutrient title must contains only letters.")
                .MaximumLength(30).WithMessage("Length of nutrient title can't be more than 30 symbols.");

            RuleFor(o => o.NutrientCategoryId).NotNull().GreaterThan(0).WithMessage("Id of nutrient category can't be null and must be greather than zero.");

            RuleFor(o => o.DailyDose).GreaterThan(0).WithMessage("DailyDose must be greather than zero.");

            RuleSet("AddNutrient", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage("Nutrient Id must be null.");  
            });

            RuleSet("UpdateNutrient", () =>
            {
                RuleFor(o => o.Id).NotNull().GreaterThan(0).WithMessage("Nutrient Id can't be null and must be greather than zero.");
            });
        }
    }
}
