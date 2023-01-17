﻿using FitnessApp.Data;
using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Logic.Services
{
    public class NutrientCategoryService : BaseService, INutrientCategoryService
    {
        private readonly ICustomValidator<NutrientCategoryDto> _validator;

        public NutrientCategoryService(ProductContext context, ICustomValidator<NutrientCategoryDto> validator) : base(context)
        {
            _validator = validator;
        }

        public async Task<ICollection<NutrientCategoryDto>> GetAllAsync()
        {
            var nutrientCategoryDbs = await _context.NutrientCategories.ToListAsync().ConfigureAwait(false);

            return NutrientCategoryBuilder.Build(nutrientCategoryDbs);
        }

        /// <summary> Outputs paginated nutrient categories from DB, depending on the selected conditions.</summary>
        /// <param name="request"></param>
        /// <returns> Returns a PaginationResponse object containing a sorted collection of nutrient categories. </returns>
        public async Task<PaginationResponse<NutrientCategoryDto>> GetPaginationAsync(PaginationRequest request)
        {
            var query = _context.NutrientCategories.AsQueryable();

            if (!string.IsNullOrEmpty(request.Query))
            {
                query = query.Where(c => c.Title.Contains(request.Query, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.SortBy))
            {
                switch (request.SortBy)
                {
                    case "title": query = request.Ascending ? query.OrderBy(c => c.Title) : query.OrderByDescending(c => c.Title); break;
                }
            }

            var categoryDbs = await query.ToListAsync().ConfigureAwait(false);

            var total = categoryDbs.Count;
            var categoryDtos = NutrientCategoryBuilder.Build(categoryDbs.Skip(request.Skip ?? 0).Take(request.Take ?? 10)?.ToList());

            return new PaginationResponse<NutrientCategoryDto>
            {
                Total = total,
                Values = categoryDtos
            };
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
            _validator.Validate(nutrientCategoryDto, "AddNutrientCategory"); 

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
            _validator.Validate(nutrientCategoryDto, "UpdateNutrientCategory");

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
