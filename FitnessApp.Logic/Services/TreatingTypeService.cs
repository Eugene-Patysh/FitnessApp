﻿using FitnessApp.Data;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Logic.Services
{
    public class TreatingTypeService : BaseService, ITreatingTypeService
    {
        private readonly ICustomValidator<TreatingTypeDto> _validator;

        public TreatingTypeService(ProductContext context, ICustomValidator<TreatingTypeDto> validator) : base(context)
        {
            _validator = validator;
        }

        public async Task<ICollection<TreatingTypeDto>> GetAllAsync()
        {
            var treatingTypeDbs = await _context.TreatingTypes.ToArrayAsync().ConfigureAwait(false);

            return TreatingTypeBuilder.Build(treatingTypeDbs);
        }

        public async Task<TreatingTypeDto> GetByIdAsync(int? treatingTypeDtoId)
        {
            if (treatingTypeDtoId == null)
            {
                throw new ValidationException("Treating type Id can't be null.");
            }

            var treatingTypeDb = await _context.TreatingTypes.SingleOrDefaultAsync(_ => _.Id == treatingTypeDtoId).ConfigureAwait(false);

            return TreatingTypeBuilder.Build(treatingTypeDb);
        }

        public async Task CreateAsync(TreatingTypeDto treatingTypeDto)
        {
            _validator.Validate(treatingTypeDto, "AddTreatingType");

            treatingTypeDto.Created = DateTime.UtcNow;
            treatingTypeDto.Updated = DateTime.UtcNow;

            await _context.TreatingTypes.AddAsync(TreatingTypeBuilder.Build(treatingTypeDto)).ConfigureAwait(false);

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Treating type has not been created. {ex.Message}.");
            }
        }

        public async Task UpdateAsync(TreatingTypeDto treatingTypeDto) 
        {
            _validator.Validate(treatingTypeDto, "UpdateTreatingType");

            var treatingTypeDb = await _context.TreatingTypes.SingleOrDefaultAsync(_ => _.Id == treatingTypeDto.Id).ConfigureAwait(false);

            if (treatingTypeDb != null)
            {
                treatingTypeDb.Title = treatingTypeDto.Title;
                treatingTypeDb.Updated = DateTime.UtcNow;

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Treating type has not been updated. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to update.");
            }
        }

        public async Task DeleteAsync(int? treatingTypeDtoId)
        {
            if (treatingTypeDtoId == null)
            {
                throw new ValidationException("Invalid Treating type Id.");
            }

            var treatingTypeDb = await _context.TreatingTypes.SingleOrDefaultAsync(_ => _.Id == treatingTypeDtoId).ConfigureAwait(false);

            if (treatingTypeDb != null)
            {
                _context.TreatingTypes.Remove(treatingTypeDb);

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Treating type has not been deleted. {ex.Message}.");
                }
            }
            else
            {
                throw new ValidationException($"There is not exist object, that you trying to delete.");
            }
        }
    }
}
