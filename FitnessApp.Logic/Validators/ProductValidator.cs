using FitnessApp.Logic.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Validators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o).NotNull().WithMessage("Product can't be null.");

            RuleSet("AddProduct", () =>
            {
                RuleFor(o => o.Id).Null().WithMessage("Product Id must be null.");
            });

            RuleSet("UpdateProduct", () =>
            {
                RuleFor(o => o.Id).NotNull().WithMessage("Product Id can't be null."); 
            });

            RuleFor(o => o.Title)
                .Must(t => !string.IsNullOrEmpty(t)).WithMessage("Product title can't be null.")
                .MaximumLength(30).WithMessage("Length of product title can't be more than 30 symbols.");

            RuleFor(o => o.ProductSubCategoryId).NotNull().WithMessage("Id of product subcategory can't be null.");
        }
    }
}
