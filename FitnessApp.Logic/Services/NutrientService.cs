﻿using FitnessApp.Data;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Logic.Services
{
    public class NutrientService : BaseService, INutrientService
    {
        private readonly ICustomValidator<NutrientDto> _validator;

        public NutrientService(ProductContext context, ICustomValidator<NutrientDto> validator) : base(context)
        {
            _validator = validator;
        }

        public async Task<ICollection<NutrientDto>> GetAllAsync()
        {
            var nutrientDbs = await _context.Nutrients.ToArrayAsync().ConfigureAwait(false);

            return NutrientBuilder.Build(nutrientDbs);
        }

        public async Task<NutrientDto> GetByIdAsync(int? nutrientDtoId)
        {
            if (nutrientDtoId == null)
            {
                throw new ValidationException("Nutrient Id can't be null.");
            }

            var nutrientDb = await _context.Nutrients.SingleOrDefaultAsync(_ => _.Id == nutrientDtoId).ConfigureAwait(false);

            return NutrientBuilder.Build(nutrientDb);
        }

        public async Task CreateAsync(NutrientDto nutrientDto)
        {
            _validator.Validate(nutrientDto, "AddNutrient");

            nutrientDto.Created = DateTime.UtcNow;
            nutrientDto.Updated = DateTime.UtcNow;

            await _context.Nutrients.AddAsync(NutrientBuilder.Build(nutrientDto)).ConfigureAwait(false);

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Nutrient has not been created. {ex.Message}.");
            }
        }

        public async Task UpdateAsync(NutrientDto nutrientDto)
        {
            _validator.Validate(nutrientDto, "UpdateNutrient");

            var nutrientDb = await _context.Nutrients.SingleOrDefaultAsync(_ => _.Id == nutrientDto.Id).ConfigureAwait(false);

            if (nutrientDb != null)
            {
                nutrientDb.Title = nutrientDto.Title;
                nutrientDb.NutrientCategoryId = nutrientDto.NutrientCategoryId;
                nutrientDb.DailyDose = nutrientDto.DailyDose;
                nutrientDb.Updated = DateTime.UtcNow;

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Nutrient has not been updated. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to update.");
            }
        }
        public async Task DeleteAsync(int? nutrientDtoId)
        {
            if (nutrientDtoId == null)
            {
                throw new ValidationException("Invalid nutrient Id.");
            }

            var nutrientDb = await _context.Nutrients.SingleOrDefaultAsync(_ => _.Id == nutrientDtoId).ConfigureAwait(false);

            if (nutrientDb != null)
            {
                _context.Nutrients.Remove(nutrientDb);

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Nutrient has not been deleted. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to delete.");
            }
        }
    }
}
