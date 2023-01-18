using FitnessApp.Logic.Models;
using FluentValidation;

namespace FitnessApp.Logic.Validators
{
    public class NutrientCategoryValidator : AbstractValidator<NutrientCategoryDto>
    {
        public NutrientCategoryValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o).NotNull().WithMessage("Nutrient category can't be null.");

            RuleFor(o => o.Title)
                .Must(t => !string.IsNullOrEmpty(t) && t.All(char.IsLetter)).WithMessage("Nutrient category title can't be null and must contains only letters.")
                .MaximumLength(30).WithMessage("Length of nutrient category title can't be more than 30 symbols.");

            RuleSet("AddNutrientCategory", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage("Nutrient category Id must be null.");
            });

            RuleSet("UpdateNutrientCategory", () =>
            {
                RuleFor(o => o.Id).NotNull().GreaterThan(0).WithMessage("Nutrient category Id can't be null and must be greather than zero.");
            });
        }
    }
}
