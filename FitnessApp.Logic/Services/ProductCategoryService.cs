using FitnessApp.Data;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Logic.Services
{
    public class ProductCategoryService : BaseService, IProductCategoryService
    {
        private readonly ICustomValidator<ProductCategoryDto> _validator;

        public ProductCategoryService(ProductContext context, ICustomValidator<ProductCategoryDto> validator) : base(context)
        {
            _validator = validator;
        }

        public async Task<ICollection<ProductCategoryDto>> GetAllAsync()
        {
            var categoryDbs = await _context.ProductCategories.ToArrayAsync().ConfigureAwait(false);

            return ProductCategoryBuilder.Build(categoryDbs);
        }

        public async Task<ProductCategoryDto> GetByIdAsync(int? productCategoryDtoId)
        {
            if (productCategoryDtoId == null)
            {
                throw new ValidationException("Product Category Id can't be null.");
            }

            var categoryDb = await _context.ProductCategories.SingleOrDefaultAsync(_ => _.Id == productCategoryDtoId).ConfigureAwait(false);
            
            return ProductCategoryBuilder.Build(categoryDb);
        }

        public async Task CreateAsync(ProductCategoryDto productCategoryDto)
        {
            _validator.Validate(productCategoryDto, "AddProductCategory");

            productCategoryDto.Created = DateTime.UtcNow;
            productCategoryDto.Updated = DateTime.UtcNow;

            await _context.ProductCategories.AddAsync(ProductCategoryBuilder.Build(productCategoryDto)).ConfigureAwait(false);
            
            try 
            {
                await _context.SaveChangesAsync().ConfigureAwait(false); 
            }
            catch (Exception ex) 
            { 
                throw new Exception($"Product category has not been created. {ex.Message}.");
            }
        }

        public async Task UpdateAsync(ProductCategoryDto productCategoryDto)
        {
            _validator.Validate(productCategoryDto, "UpdateProductCategory");

            var categoryDb = await _context.ProductCategories.SingleOrDefaultAsync(_ => _.Id == productCategoryDto.Id).ConfigureAwait(false);
            
            if (categoryDb != null)
            {
                categoryDb.Title = productCategoryDto.Title;
                categoryDb.Updated = DateTime.UtcNow;

                try 
                { 
                    await _context.SaveChangesAsync().ConfigureAwait(false); 
                }
                catch (Exception ex) 
                {
                    throw new Exception($"Product category has not been updated. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to update.");
            }   
        }

        public async Task DeleteAsync(int? productCategoryDtoId)
        {
            if (productCategoryDtoId == null)
            {
                throw new ValidationException("Invalid product category Id.");
            }

            var categoryDb = await _context.ProductCategories.SingleOrDefaultAsync(_ => _.Id == productCategoryDtoId).ConfigureAwait(false);

            if (categoryDb != null)
            {
                _context.ProductCategories.Remove(categoryDb);

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Product category has not been deleted. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to delete.");
            }    
        }
    }
}
