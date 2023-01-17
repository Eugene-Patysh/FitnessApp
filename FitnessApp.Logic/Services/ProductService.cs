using FitnessApp.Data;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Logic.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly ICustomValidator<ProductDto> _validator;

        public ProductService(ProductContext context, ICustomValidator<ProductDto> validator) : base(context)
        {
            _validator = validator;
        }

        public async Task<ICollection<ProductDto>> GetAllAsync()
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
            _validator.Validate(productDto,"AddProduct");

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
            _validator.Validate(productDto, "UpdateProduct");

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
