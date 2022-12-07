using FitnessApp.Logic.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Validators
{
    public class TreatingTypeValidator : AbstractValidator<TreatingTypeDto>
    {
        public TreatingTypeValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o).NotNull().WithMessage("Treating type can't be null.");

            RuleSet("AddTreatingType", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage("Treating type Id must be null.");
            });

            RuleSet("UpdateTreatingType", () =>
            {
                RuleFor(o => o.Id).NotNull().WithMessage("Treating type Id can't be null.");
            });

            RuleFor(o => o.Title)
                .Must(t => !string.IsNullOrEmpty(t)).WithMessage("Treating type title can't be null.")
                .MaximumLength(30).WithMessage("Length of treating type title can't be more than 30 symbols.");
        }
    }
}
