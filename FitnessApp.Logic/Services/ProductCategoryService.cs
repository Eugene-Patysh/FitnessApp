using FitnessApp.Data;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Logic.Services
{
    public class ProductCategoryService : BaseService, IProductCategoryService
    {
        private readonly IValidator<ProductCategoryDto> _validator;

        public ProductCategoryService(ProductContext context, IValidator<ProductCategoryDto> validator) : base(context)
        {
            _validator = validator;
        }

        //public ProductCategoryDto[] GetWithCondition(int pageNumber, int objectsNumber)
        //{
        //    if (pageNumber <= 0 || objectsNumber <= 0)
        //    {
        //        return null;
        //    }
        //
        //    var categoryDbs = _context.ProductCategories.OrderBy(_ => _.Title).Skip((pageNumber - 1) * objectsNumber).Take(objectsNumber).ToArray();
        //
        //    return ProductCategoryBuilder.Build(categoryDbs);
        //}

        public async Task<ProductCategoryDto[]> GetAllAsync() //Если база пустая?
        {
            var categoryDbs = await _context.ProductCategories.ToArrayAsync().ConfigureAwait(false);

            return ProductCategoryBuilder.Build(categoryDbs);
        }

        public async Task<ProductCategoryDto> GetByIdAsync(int? productCategoryDtoId)
        {
            if (productCategoryDtoId == null) // Почему было не равно нулю?
            {
                throw new ValidationException("Product Category Id can't be null.");
            }

            var categoryDb = await _context.ProductCategories.SingleOrDefaultAsync(_ => _.Id == productCategoryDtoId).ConfigureAwait(false);
            
            return ProductCategoryBuilder.Build(categoryDb);
        }

        public async Task CreateAsync(ProductCategoryDto productCategoryDto) //Если сам объект null?
        {
            //if (productCategoryDto.Id != null || string.IsNullOrEmpty(productCategoryDto.Title))
            //{
            //    throw new ValidationException("Invalid model.");
            //}

            var validationResult = _validator.Validate(productCategoryDto, v => v.IncludeRuleSets("AddProductCategory"));
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.ToString());

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
            //if (productCategoryDto.Id == null || string.IsNullOrEmpty(productCategoryDto.Title))
            //{
            //    throw new ValidationException("Invalid model.");
            //}

            var validationResult = _validator.Validate(productCategoryDto, v => v.IncludeRuleSets("UpdateProductCategory"));
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.ToString());

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
            if (productCategoryDtoId == null) //Почему все таки проверяем только на null, но не на ноль?
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
