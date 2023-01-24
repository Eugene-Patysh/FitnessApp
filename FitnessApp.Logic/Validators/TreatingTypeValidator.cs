using FitnessApp.Logic.Models;
using FluentValidation;

namespace FitnessApp.Logic.Validators
{
    public class TreatingTypeValidator : AbstractValidator<TreatingTypeDto>
    {
        public TreatingTypeValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o).NotNull().WithMessage("Treating type can't be null.");

            RuleFor(o => o.Title)
                .Must(t => !string.IsNullOrEmpty(t) && t.All(char.IsLetter)).WithMessage("Treating type title can't be null and must contains only letters.")
                .MaximumLength(30).WithMessage("Length of treating type title can't be more than 30 symbols.");

            RuleSet("AddTreatingType", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage("Treating type Id must be null.");
            });

            RuleSet("UpdateTreatingType", () =>
            {
                RuleFor(o => o.Id).NotNull().GreaterThan(0).WithMessage("Treating type Id can't be null and must be greather than zero.");
            });
        }
    }
}
