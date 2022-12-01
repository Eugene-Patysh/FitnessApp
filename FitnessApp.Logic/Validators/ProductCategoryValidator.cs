using FitnessApp.Logic.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Validators
{
    public class ProductCategoryValidator : AbstractValidator<ProductCategoryDto>
    {
        public ProductCategoryValidator()
        {
            RuleLevelCascadeMode = CascadeMode.StopOnFirstFailure; // TODO: investigate and use new behavior

            RuleSet("AddProductCategory", () => 
            {
                RuleFor(c => c.Id).Null().WithMessage("Product category id must be null.");
            });

            RuleSet("UpdateProductCategory", () =>
            {
                RuleFor(c => c.Id).NotNull().WithMessage("Product category id can't be null.");
            });

            RuleFor(c => c.Title)
                .Must(t => !string.IsNullOrEmpty(t)).WithMessage("Product category title can't be null.")
                .MaximumLength(30).WithMessage("Length of product category title can't be more than 30 symbols.");
        }
    }
}
