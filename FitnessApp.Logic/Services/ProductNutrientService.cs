using FitnessApp.Data;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Logic.Services
{
    public class ProductNutrientService : BaseService, IProductNutrientService
    {
        private readonly ICustomValidator<ProductNutrientDto> _validator;

        public ProductNutrientService(ProductContext context, ICustomValidator<ProductNutrientDto> validator) : base(context)
        {
            _validator = validator;
        }

        public async Task<ICollection<ProductNutrientDto>> GetAllAsync()
        {
            var productNutrientDbs = await _context.ProductNutrients.ToArrayAsync().ConfigureAwait(false);

            return ProductNutrientBuilder.Build(productNutrientDbs);
        }

        public async Task<ProductNutrientDto> GetByIdAsync(int? productNutrientDtoId)
        {
            if (productNutrientDtoId == null)
            {
                throw new ValidationException("Product-Nutrient Id can't be null.");
            }

            var productNutrientDb = await _context.ProductNutrients.SingleOrDefaultAsync(_ => _.Id == productNutrientDtoId).ConfigureAwait(false);

            return ProductNutrientBuilder.Build(productNutrientDb);
        }

        public async Task CreateAsync(ProductNutrientDto productNutrientDto)
        {
            _validator.Validate(productNutrientDto, "AddProductNutrient");

            productNutrientDto.Created = DateTime.UtcNow;
            productNutrientDto.Updated = DateTime.UtcNow;

            await _context.ProductNutrients.AddAsync(ProductNutrientBuilder.Build(productNutrientDto)).ConfigureAwait(false);

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Product-Nutrient has not been created. {ex.Message}.");
            }
        }

        public async Task UpdateAsync(ProductNutrientDto productNutrientDto)
        {
            _validator.Validate(productNutrientDto, "UpdateProductNutrient");

            var productNutrientDb = await _context.ProductNutrients.SingleOrDefaultAsync(_ => _.Id == productNutrientDto.Id).ConfigureAwait(false);

            if (productNutrientDb != null)
            {
                productNutrientDb.Updated = DateTime.UtcNow;
                productNutrientDb.Quality = productNutrientDto.Quality;
                productNutrientDb.ProductId = productNutrientDto.ProductId;
                productNutrientDb.NutrientId = productNutrientDto.NutrientId;
                productNutrientDb.TreatingTypeId = productNutrientDto.TreatingTypeId;

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Product-Nutrient has not been updated. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to update.");
            }
        }

        public async Task DeleteAsync(int? productNutrientDtoId)
        {
            if (productNutrientDtoId == null)
            {
                throw new ValidationException("Invalid Product-Nutrient Id.");
            }

            var productNutrientDb = await _context.ProductNutrients.SingleOrDefaultAsync(_ => _.Id == productNutrientDtoId).ConfigureAwait(false);

            if (productNutrientDb != null)
            {
                _context.ProductNutrients.Remove(productNutrientDb);

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Product-Nutrient has not been deleted. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to delete.");
            }
        }
    }
}
