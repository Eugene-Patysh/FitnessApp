using FitnessApp.Data;
using FitnessApp.Localization;
using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FitnessApp.Logic.Services
{
    public class NutrientCategoryService : BaseService, INutrientCategoryService
    {
        private readonly ICustomValidator<NutrientCategoryDto> _validator;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public NutrientCategoryService(ProductContext context, ICustomValidator<NutrientCategoryDto> validator, IStringLocalizer<SharedResource> sharedLocalizer) : base(context)
        {
            _validator = validator;
            _sharedLocalizer = sharedLocalizer;
        }

        /// <summary> Gets all nutirent categories from DB. </summary>
        /// <returns> Returns collection of nutirent categories. </returns>
        /// <exception cref="Exception"></exception>
        public async Task<ICollection<NutrientCategoryDto>> GetAllAsync()
        {
            var nutrientCategoryDbs = await _context.NutrientCategories.ToListAsync().ConfigureAwait(false);

            return NutrientCategoryBuilder.Build(nutrientCategoryDbs) ?? throw new Exception($"There are not objects of nutrient categories."); ;
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

        /// <summary> Gets nutirent category from DB by Id. </summary>
        /// <param name="nutrientCategoryId"></param>
        /// <returns> Returns object of nutirent category with Id: <paramref name="nutrientCategoryId"/>. </returns>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<NutrientCategoryDto> GetByIdAsync(int? nutrientCategoryId)
        {
            if (nutrientCategoryId == null)
            {
                throw new ValidationException(_sharedLocalizer["ObjectIdCantBeNull"]);
            }

            var nutrientCategoryDb = await _context.NutrientCategories.SingleOrDefaultAsync(_ => _.Id == nutrientCategoryId).ConfigureAwait(false);

            return NutrientCategoryBuilder.Build(nutrientCategoryDb);
        }

        /// <summary> Creates new nutrient category. </summary>
        /// <param name="nutrientCategoryDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="Exception"></exception>
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
            catch
            {
                throw new Exception(_sharedLocalizer["ObjectNotCreated"]);
            }
        }

        /// <summary> Updates nutrient category in DB. </summary>
        /// <param name="nutrientCategoryDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ValidationException"></exception>
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
                catch
                {
                    throw new Exception(_sharedLocalizer["ObjectNotUpdated"]);
                }
            }
            else
            {
                throw new ValidationException(_sharedLocalizer["NotExistObjectForUpdating"]);
            }
        }

        /// <summary> Deletes nutrient category from DB. </summary>
        /// <param name="nutrientCategoryId"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task DeleteAsync(int? nutrientCategoryId)
        {
            if (nutrientCategoryId == null)
            {
                throw new ValidationException(_sharedLocalizer["InvalidObjectId"]);
            }

            var nutrientCategoryDb = await _context.NutrientCategories.SingleOrDefaultAsync(_ => _.Id == nutrientCategoryId).ConfigureAwait(false);

            if (nutrientCategoryDb != null)
            {
                _context.NutrientCategories.Remove(nutrientCategoryDb);

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch
                {
                    throw new Exception(_sharedLocalizer["ObjectNotDeleted"]);
                }
            }
            else
            {
                throw new ValidationException(_sharedLocalizer["NotExistObjectForDeleting"]);
            }
        }
    }
}
