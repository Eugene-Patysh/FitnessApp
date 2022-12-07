using FitnessApp.Data;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace FitnessApp.Logic.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IValidator<ProductDto> _validator;

        public ProductService(ProductContext context, IValidator<ProductDto> validator) : base(context)
        {
            _validator = validator;
        }

        public async Task<ProductDto[]> GetAllAsync()
        {
            var productDbs = await _context.Products.ToArrayAsync().ConfigureAwait(false);

            return ProductBuilder.Build(productDbs);
        }

        public async Task<ProductDto> GetByIdAsync(int? productDtoId)
        {
            if (productDtoId == null)
            {
                throw new ValidationException("Product Id can't be null.");
            }

            var productDb = await _context.Products.SingleOrDefaultAsync(_ => _.Id == productDtoId).ConfigureAwait(false);

            return ProductBuilder.Build(productDb);
        }

        public async Task CreateAsync(ProductDto productDto)
        {
            var validationResult = _validator.Validate(productDto, v => v.IncludeRuleSets("AddProduct"));
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.ToString());

            productDto.Created = DateTime.UtcNow;
            productDto.Updated = DateTime.UtcNow;

            await _context.Products.AddAsync(ProductBuilder.Build(productDto)).ConfigureAwait(false);

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Product has not been created. {ex.Message}.");
            }
        }

        public async Task UpdateAsync(ProductDto productDto)
        {
            var validationResult = _validator.Validate(productDto, v => v.IncludeRuleSets("UpdateProduct"));
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.ToString());

            var productDb = await _context.Products.SingleOrDefaultAsync(_ => _.Id == productDto.Id).ConfigureAwait(false);

            if (productDb != null)
            {
                productDb.Title = productDto.Title;
                productDb.ProductSubCategoryId = productDto.ProductSubCategoryId;
                productDb.Updated = DateTime.UtcNow;

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Product has not been updated. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to update.");
            }
        }
        public async Task DeleteAsync(int? productDtoId)
        {
            if (productDtoId == null)
            {
                throw new ValidationException("Invalid product Id.");
            }

            var productDb = await _context.Products.SingleOrDefaultAsync(_ => _.Id == productDtoId).ConfigureAwait(false);

            if (productDb != null)
            {
                _context.Products.Remove(productDb);

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Product has not been deleted. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to delete.");
            }
        }
    }
}
