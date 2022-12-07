using FitnessApp.Logic.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Validators
{
    public class ProductSubCategoryValidator : AbstractValidator<ProductSubCategoryDto>
    {
        public ProductSubCategoryValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o).NotNull().WithMessage("Product subcategory can't be null.");

            RuleSet("AddProductSubCategory", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage("Product subcategory Id must be null.");

            });

            RuleSet("UpdateProductSubCategory", () =>
            {
                RuleFor(o => o.Id).NotNull().WithMessage("Product subcategory Id can't be null.");
            });

            RuleFor(o => o.Title)
                .Must(t => !string.IsNullOrEmpty(t)).WithMessage("Product subcategory title can't be null.")
                .MaximumLength(30).WithMessage("Length of product subcategory title can't be more than 30 symbols.");

            RuleFor(o => o.ProductCategoryId).NotNull().WithMessage("Id of product category can't be null.");
        }
    }
}
