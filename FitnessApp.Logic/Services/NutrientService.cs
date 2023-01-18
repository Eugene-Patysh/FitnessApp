using FitnessApp.Data;
using FitnessApp.Logic.ApiModels;
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

        /// <summary> Gets all nutrients from DB. </summary>
        /// <returns> Returns collection of nutrients. </returns>
        /// <exception cref="Exception"></exception>
        public async Task<ICollection<NutrientDto>> GetAllAsync()
        {
            var nutrientDbs = await _context.Nutrients.ToListAsync().ConfigureAwait(false);

            return NutrientBuilder.Build(nutrientDbs) ?? throw new Exception($"There are not objects of nutrients.");
        }

        /// <summary> Outputs paginated nutrients from DB, depending on the selected conditions.</summary>
        /// <param name="request"></param>
        /// <returns> Returns a PaginationResponse object containing a sorted collection of nutrients. </returns>
        public async Task<PaginationResponse<NutrientDto>> GetPaginationAsync(PaginationRequest request)
        {
            var query = _context.Nutrients.AsQueryable();

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
            var categoryDtos = NutrientBuilder.Build(categoryDbs.Skip(request.Skip ?? 0).Take(request.Take ?? 10)?.ToList());

            return new PaginationResponse<NutrientDto>
            {
                Total = total,
                Values = categoryDtos
            };
        }

        /// <summary> Gets nutrient from DB by Id. </summary>
        /// <param name="nutrientDtoId"></param>
        /// <returns> Returns object of nutrient with Id: <paramref name="nutrientDtoId"/>. </returns>
        /// <exception cref="ValidationException"></exception>
        public async Task<NutrientDto> GetByIdAsync(int? nutrientDtoId)
        {
            if (nutrientDtoId == null)
            {
                throw new ValidationException("Nutrient Id can't be null.");
            }

            var nutrientDb = await _context.Nutrients.SingleOrDefaultAsync(_ => _.Id == nutrientDtoId).ConfigureAwait(false);

            return NutrientBuilder.Build(nutrientDb);
        }

        /// <summary> Creates new nutrient. </summary>
        /// <param name="nutrientDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary> Updates nutrient in DB. </summary>
        /// <param name="nutrientDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ValidationException"></exception>
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

        /// <summary> Deletes nutrient from DB. </summary>
        /// <param name="nutrientDtoId"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="Exception"></exception>
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
