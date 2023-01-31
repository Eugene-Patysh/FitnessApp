using FitnessApp.Localization;
using FitnessApp.Logic.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FitnessApp.Logic.Validators
{
    public class NutrientCategoryValidator : AbstractValidator<NutrientCategoryDto>
    {
        public NutrientCategoryValidator(IStringLocalizer<SharedResource> sharedLocalizer)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o).NotNull().WithMessage(x => sharedLocalizer["ObjectIdCantBeNull"]);

            RuleFor(o => o.Title)
                .Must(t => !string.IsNullOrEmpty(t) && t.All(char.IsLetter)).WithMessage(x => sharedLocalizer["TitleNotNullOnlyLetters"])
                .MaximumLength(30).WithMessage(x => sharedLocalizer["LenghtNoMore30Symbols"]);

            RuleFor(o => o.Title)
                .Must(t => !string.IsNullOrEmpty(t) && t.All(char.IsLetter)).WithMessage("Nutrient category title can't be null and must contains only letters.")
                .MaximumLength(30).WithMessage("Length of nutrient category title can't be more than 30 symbols.");

            RuleSet("AddNutrientCategory", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage(x => sharedLocalizer["WhenCreatingIdMustBeNull"]);
            });

            RuleSet("UpdateNutrientCategory", () =>
            {
                RuleFor(o => o.Id).NotNull().GreaterThan(0).WithMessage(x => sharedLocalizer["WhenUpdatingIdNotNullGreatherZero"]);
            });
        }
    }
}
