using FitnessApp.Localization;
using FitnessApp.Logic.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FitnessApp.Logic.Validators
{
    public class ProductCategoryValidator : AbstractValidator<ProductCategoryDto>
    {
        public ProductCategoryValidator(IStringLocalizer<SharedResource> sharedLocalizer)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o).NotNull().WithMessage(x => String.Format(sharedLocalizer["ObjectIdCantBeNull"])); 

            RuleFor(o => o.Title)
                .Must(t => !string.IsNullOrEmpty(t) && t.All(char.IsLetter)).WithMessage(x => String.Format(sharedLocalizer["TitleNotNullOnlyLetters"]))
                .MaximumLength(30).WithMessage(x => String.Format(sharedLocalizer["LenghtNoMore30Symbols"]));

            RuleSet("AddProductCategory", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage(x => String.Format(sharedLocalizer["WhenCreatingIdMustBeNull"])); 
            });

            RuleSet("UpdateProductCategory", () =>
            {
                RuleFor(o => o.Id).NotNull().GreaterThan(0).WithMessage(x => String.Format(sharedLocalizer["WhenUpdatingIdNotNullGreatherZero"])); 
            });
        }
    }
}
