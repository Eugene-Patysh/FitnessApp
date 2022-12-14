using FitnessApp.Data;
using FitnessApp.Data.Models;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Services
{
    public class NutrientCategoryService : BaseService, INutrientCategoryService
    {
        private readonly IValidator<NutrientCategoryDto> _validator;

        public NutrientCategoryService(ProductContext context, IValidator<NutrientCategoryDto> validator) : base(context)
        {
            _validator = validator;
        }

        public async Task<NutrientCategoryDto[]> GetAllAsync()
        {
            var nutrientCategoryDbs = await _context.NutrientCategories.ToArrayAsync().ConfigureAwait(false);

            return NutrientCategoryBuilder.Build(nutrientCategoryDbs);
        }

        public async Task<NutrientCategoryDto> GetByIdAsync(int? nutrientCategoryDtoId)
        {
            if (nutrientCategoryDtoId == null)
            {
                throw new ValidationException("Nutrient category Id can't be null.");
            }

            var nutrientCategoryDb = await _context.NutrientCategories.SingleOrDefaultAsync(_ => _.Id == nutrientCategoryDtoId).ConfigureAwait(false);

            return NutrientCategoryBuilder.Build(nutrientCategoryDb);
        }

        public async Task CreateAsync(NutrientCategoryDto nutrientCategoryDto)
        {
            var validationResult = _validator.Validate(nutrientCategoryDto, v => v.IncludeRuleSets("AddNutrientCategory"));
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.ToString());

            nutrientCategoryDto.Created = DateTime.UtcNow;
            nutrientCategoryDto.Updated = DateTime.UtcNow;

            await _context.NutrientCategories.AddAsync(NutrientCategoryBuilder.Build(nutrientCategoryDto)).ConfigureAwait(false);

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Nutrient category has not been created. {ex.Message}.");
            }
        }

        public async Task UpdateAsync(NutrientCategoryDto nutrientCategoryDto)
        {
            var validationResult = _validator.Validate(nutrientCategoryDto, v => v.IncludeRuleSets("UpdateNutrientCategory"));
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.ToString());

            var nutrientCategoryDb = await _context.NutrientCategories.SingleOrDefaultAsync(_ => _.Id == nutrientCategoryDto.Id).ConfigureAwait(false);

            if (nutrientCategoryDb != null)
            {
                nutrientCategoryDb.Title = nutrientCategoryDto.Title;
                nutrientCategoryDb.Updated = DateTime.UtcNow;

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Nutrient category has not been updated. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to update.");
            }
        }

        public async Task DeleteAsync(int? nutrientCategoryDtoId)
        {
            if (nutrientCategoryDtoId == null)
            {
                throw new ValidationException("Invalid nutrient category Id.");
            }

            var nutrientCategoryDb = await _context.NutrientCategories.SingleOrDefaultAsync(_ => _.Id == nutrientCategoryDtoId).ConfigureAwait(false);

            if (nutrientCategoryDb != null)
            {
                _context.NutrientCategories.Remove(nutrientCategoryDb);

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Nutrient category has not been deleted. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to delete.");
            }
        }
    }
}
