using FitnessApp.Localization;
using FitnessApp.Logic.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FitnessApp.Logic.Validators
{
    public class ProductNutrientValidator : AbstractValidator<ProductNutrientDto>
    {
        public ProductNutrientValidator(IStringLocalizer<SharedResource> sharedLocalizer)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o).NotNull().WithMessage(x => sharedLocalizer["ObjectIdCantBeNull"]);

            RuleFor(o => o.ProductId).NotNull().GreaterThan(0).WithMessage(x => sharedLocalizer["IdDependsObjectNotNullGreatherZero"]);

            RuleFor(o => o.NutrientId).NotNull().GreaterThan(0).WithMessage(x => sharedLocalizer["IdDependsObjectNotNullGreatherZero"]);

            RuleFor(o => o.TreatingTypeId).NotNull().GreaterThan(0).WithMessage(x => sharedLocalizer["IdDependsObjectNotNullGreatherZero"]);

            RuleFor(o => o.Quality).Must(p => p >= 0).WithMessage(x => sharedLocalizer["ValueEqualsOrGreatherZero"]);

            RuleSet("AddProductNutrient", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage(x => sharedLocalizer["WhenCreatingIdMustBeNull"]);
            });

            RuleSet("UpdateProductNutrient", () =>
            {
                RuleFor(o => o.Id).NotNull().GreaterThan(0).WithMessage(x => sharedLocalizer["WhenUpdatingIdNotNullGreatherZero"]);
            });
        }
    }
}
