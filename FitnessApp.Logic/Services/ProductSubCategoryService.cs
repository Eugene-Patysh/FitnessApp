using FitnessApp.Data;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Logic.Services 
{
    public class ProductSubCategoryService : BaseService, IProductSubCategoryService
    {
        private readonly IValidator<ProductSubCategoryDto> _validator;

        public ProductSubCategoryService(ProductContext context, IValidator<ProductSubCategoryDto> validator) : base(context)
        {
            _validator = validator;
        }

        public async Task<ICollection<ProductSubCategoryDto>> GetAllAsync()
        {
            var subCategoryDbs = await _context.ProductSubCategories.ToArrayAsync().ConfigureAwait(false);

            return ProductSubCategoryBuilder.Build(subCategoryDbs);
        }

        public async Task<ProductSubCategoryDto> GetByIdAsync(int? productSubCategoryDtoId)
        {
            if (productSubCategoryDtoId == null)
            {
                throw new ValidationException("Product subcategory Id can't be null.");
            }

            var subCategoryDb = await _context.ProductSubCategories.SingleOrDefaultAsync(_ => _.Id == productSubCategoryDtoId).ConfigureAwait(false);

            return ProductSubCategoryBuilder.Build(subCategoryDb);
        }

        public async Task CreateAsync(ProductSubCategoryDto productSubCategoryDto)
        {
            var validationResult = _validator.Validate(productSubCategoryDto, v => v.IncludeRuleSets("AddProductSubCategory"));
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.ToString());

            productSubCategoryDto.Created = DateTime.UtcNow;
            productSubCategoryDto.Updated = DateTime.UtcNow;
           
            await _context.ProductSubCategories.AddAsync(ProductSubCategoryBuilder.Build(productSubCategoryDto)).ConfigureAwait(false);

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Product subcategory has not been created. {ex.Message}.");
            }
        }

        public async Task UpdateAsync(ProductSubCategoryDto productSubCategoryDto)
        {
            var validationResult = _validator.Validate(productSubCategoryDto, v => v.IncludeRuleSets("UpdateProductSubCategory"));
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.ToString());

            var subCategoryDb = await _context.ProductSubCategories.SingleOrDefaultAsync(_ => _.Id == productSubCategoryDto.Id).ConfigureAwait(false);

            if (subCategoryDb != null)
            {
                subCategoryDb.Title = productSubCategoryDto.Title;
                subCategoryDb.ProductCategoryId = productSubCategoryDto.ProductCategoryId;
                subCategoryDb.Updated = DateTime.UtcNow;

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Product subcategory has not been updated. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to update.");
            }
        }

        public async Task DeleteAsync(int? productSubCategoryDtoId)
        {
            if (productSubCategoryDtoId == null)
            {
                throw new ValidationException("Invalid product subcategory Id.");
            }

            var subCategoryDb = await _context.ProductSubCategories.SingleOrDefaultAsync(_ => _.Id == productSubCategoryDtoId).ConfigureAwait(false);

            if (subCategoryDb != null)
            {
                _context.ProductSubCategories.Remove(subCategoryDb);

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Product subcategory has not been deleted. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to delete.");
            }
        }
    }
}
