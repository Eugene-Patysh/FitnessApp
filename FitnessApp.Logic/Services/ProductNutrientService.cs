using EventBus.Base.Standard;
using FitnessApp.Data;
using FitnessApp.Data.Models;
using FitnessApp.Localization;
using FitnessApp.Logging.Events;
using FitnessApp.Logging.Models;
using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Builders;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FitnessApp.Logic.Services
{
    public class ProductNutrientService : BaseService, IProductNutrientService
    {
        private readonly ICustomValidator<ProductNutrientDto> _validator;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly IEventBus _eventBus;

        public ProductNutrientService(ProductContext context, ICustomValidator<ProductNutrientDto> validator, 
            IStringLocalizer<SharedResource> sharedLocalizer, IEventBus eventBus) : base(context)
        {
            _validator = validator;
            _sharedLocalizer = sharedLocalizer;
            _eventBus = eventBus;
        }

        /// <summary> Gets all Product-Nutrients from DB. </summary>
        /// <returns> Returns collection of Product-Nutrients. </returns>
        /// <exception cref="Exception"></exception>
        public async Task<ICollection<ProductNutrientDto>> GetAllAsync()
        {
            var productNutrientDbs = await _context.ProductNutrients.ToListAsync().ConfigureAwait(false);
            
            return ProductNutrientBuilder.Build(productNutrientDbs);
        }

        /// <summary> Outputs paginated Product-Nutrients from DB, depending on the selected conditions.</summary>
        /// <param name="request"></param>
        /// <returns> Returns a PaginationResponse object containing a sorted collection of Product-Nutrients. </returns>
        public async Task<PaginationResponse<ProductNutrientDto>> GetPaginationAsync(PaginationRequest request)
        {
            var query = _context.ProductNutrients.AsQueryable();

            if (!string.IsNullOrEmpty(request.Query))
            {
                query = query.Where(c => c.Product.Title.Contains(request.Query, StringComparison.OrdinalIgnoreCase)
                || c.Nutrient.Title.Contains(request.Query, StringComparison.OrdinalIgnoreCase)
                || c.TreatingType.Title.Contains(request.Query, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.SortBy))
            {
                switch (request.SortBy)
                {
                    case "productTitle": query = request.Ascending ? query.OrderBy(c => c.Product.Title) : query.OrderByDescending(c => c.Product.Title); break;
                    case "nutrientTitle": query = request.Ascending ? query.OrderBy(c => c.Nutrient.Title) : query.OrderByDescending(c => c.Nutrient.Title); break;
                    case "treatingTypeTitle": query = request.Ascending ? query.OrderBy(c => c.TreatingType.Title) : query.OrderByDescending(c => c.TreatingType.Title); break;
                    case "quality": query = request.Ascending ? query.OrderBy(c => c.Quality) : query.OrderByDescending(c => c.Quality); break;
                }
            }

            var categoryDbs = await query.ToListAsync().ConfigureAwait(false);

            var total = categoryDbs.Count;
            var categoryDtos = ProductNutrientBuilder.Build(categoryDbs.Skip(request.Skip ?? 0).Take(request.Take ?? 10)?.ToList());

            return new PaginationResponse<ProductNutrientDto>
            {
                Total = total,
                Values = categoryDtos
            };
        }


        /// <summary> Gets Product-Nutrient from DB by Id. </summary>
        /// <param name="productNutrientId" example="666">The Product-Nutrient Id. </param>
        /// <returns> Returns object of Product-Nutrient with Id: <paramref name="productNutrientId"/>. </returns>
        /// <exception cref="ValidationException"></exception>
        public async Task<ProductNutrientDto> GetByIdAsync(int? productNutrientId)
        {
            if (productNutrientId == null)
            {
                throw new ValidationException(_sharedLocalizer["ObjectIdCantBeNull"]);
            }

            var productNutrientDb = await _context.ProductNutrients.SingleOrDefaultAsync(_ => _.Id == productNutrientId).ConfigureAwait(false);

            return ProductNutrientBuilder.Build(productNutrientDb);
        }

        /// <summary> Creates new Product-Nutrient. </summary>
        /// <param name="productNutrientDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="Exception"></exception>
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
                _eventBus.Publish(new LogEvent(Statuses.Fail, "Creation", ProductNutrientDto.ENTITY_TYPE,
                    $"Changes was not saved in data base: {ex.Message}"));
                throw new Exception(_sharedLocalizer["ObjectNotCreated"]);
            }
        }

        /// <summary> Updates Product-Nutrient in DB. </summary>
        /// <param name="productNutrientDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ValidationException"></exception>
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
                    _eventBus.Publish(new LogEvent(Statuses.Fail, "Update", ProductNutrientDto.ENTITY_TYPE, 
                        $"Changes was not saved in data base: {ex.Message}"));
                    throw new Exception(_sharedLocalizer["ObjectNotUpdated"]);
                }
            }
            else
            {
                throw new ValidationException(_sharedLocalizer["NotExistObjectForUpdating"]);
            }
        }

        /// <summary> Deletes Product-Nutrient from DB. </summary>
        /// <param name="productNutrientId"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task DeleteAsync(int? productNutrientId)
        {
            if (productNutrientId == null)
            {
                throw new ValidationException(_sharedLocalizer["InvalidObjectId"]);
            }

            var productNutrientDb = await _context.ProductNutrients.SingleOrDefaultAsync(_ => _.Id == productNutrientId).ConfigureAwait(false);

            if (productNutrientDb != null)
            {
                _context.ProductNutrients.Remove(productNutrientDb);

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _eventBus.Publish(new LogEvent(Statuses.Fail, "Deletion", ProductNutrientDto.ENTITY_TYPE, 
                        $"Changes was not saved in data base: {ex.Message}"));
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
