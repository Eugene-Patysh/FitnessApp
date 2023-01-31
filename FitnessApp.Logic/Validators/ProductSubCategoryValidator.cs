using FitnessApp.Localization;
using FitnessApp.Logic.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FitnessApp.Logic.Validators
{
    public class ProductSubCategoryValidator : AbstractValidator<ProductSubCategoryDto>
    {
        public ProductSubCategoryValidator(IStringLocalizer<SharedResource> sharedLocalizer)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o).NotNull().WithMessage(x => sharedLocalizer["ObjectIdCantBeNull"]);

            RuleFor(o => o.Title)
                .Must(t => !string.IsNullOrEmpty(t) && t.All(char.IsLetter)).WithMessage(x => sharedLocalizer["TitleNotNullOnlyLetters"])
                .MaximumLength(30).WithMessage(x => sharedLocalizer["LenghtNoMore30Symbols"]);

            RuleFor(o => o.ProductCategoryId).NotNull().GreaterThan(0).WithMessage(x => sharedLocalizer["IdDependsObjectNotNullGreatherZero"]);

            RuleSet("AddProductSubCategory", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage(x => sharedLocalizer["WhenCreatingIdMustBeNull"]); 
            });

            RuleSet("UpdateProductSubCategory", () =>
            {
                RuleFor(o => o.Id).NotNull().GreaterThan(0).WithMessage(x => sharedLocalizer["WhenUpdatingIdNotNullGreatherZero"]);
            });
        }
    }
}
