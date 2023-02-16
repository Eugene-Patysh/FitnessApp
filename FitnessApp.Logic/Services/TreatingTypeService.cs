using EventBus.Base.Standard;
using FitnessApp.Data;
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
    public class TreatingTypeService : BaseService, ITreatingTypeService
    {
        private readonly ICustomValidator<TreatingTypeDto> _validator;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly IEventBus _eventBus;

        public TreatingTypeService(ProductContext context, ICustomValidator<TreatingTypeDto> validator, 
            IStringLocalizer<SharedResource> sharedLocalizer, IEventBus eventBus) : base(context)
        {
            _validator = validator;
            _sharedLocalizer = sharedLocalizer;
            _eventBus = eventBus;
        }

        /// <summary> Gets all treating types from DB. </summary>
        /// <returns> Returns collection of treating types. </returns>
        /// <exception cref="Exception"></exception>
        public async Task<ICollection<TreatingTypeDto>> GetAllAsync()
        {
            var treatingTypeDbs = await _context.TreatingTypes.ToListAsync().ConfigureAwait(false);

            return TreatingTypeBuilder.Build(treatingTypeDbs);
        }

        /// <summary> Outputs paginated treating types from DB, depending on the selected conditions.</summary>
        /// <param name="request"></param>
        /// <returns> Returns a PaginationResponse object containing a sorted collection of treating types. </returns>
        public async Task<PaginationResponse<TreatingTypeDto>> GetPaginationAsync(PaginationRequest request)
        {
            var query = _context.TreatingTypes.AsQueryable();

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
            var categoryDtos = TreatingTypeBuilder.Build(categoryDbs.Skip(request.Skip ?? 0).Take(request.Take ?? 10)?.ToList());

            return new PaginationResponse<TreatingTypeDto>
            {
                Total = total,
                Values = categoryDtos
            };
        }

        /// <summary> Gets treating type from DB by Id. </summary>
        /// <param name="treatingTypeId"></param>
        /// <returns> Returns object of treating type with Id: <paramref name="treatingTypeId"/>. </returns>
        /// <exception cref="ValidationException"></exception>
        public async Task<TreatingTypeDto> GetByIdAsync(int? treatingTypeId)
        {
            if (treatingTypeId == null)
            {
                throw new ValidationException(_sharedLocalizer["ObjectIdCantBeNull"]);
            }

            var treatingTypeDb = await _context.TreatingTypes.SingleOrDefaultAsync(_ => _.Id == treatingTypeId).ConfigureAwait(false);

            return TreatingTypeBuilder.Build(treatingTypeDb);
        }

        /// <summary> Creates new treating type. </summary>
        /// <param name="treatingTypeDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="Exception"></exception>
        public async Task CreateAsync(TreatingTypeDto treatingTypeDto)
        {
            _validator.Validate(treatingTypeDto, "AddTreatingType");

            treatingTypeDto.Created = DateTime.UtcNow;
            treatingTypeDto.Updated = DateTime.UtcNow;

            await _context.TreatingTypes.AddAsync(TreatingTypeBuilder.Build(treatingTypeDto)).ConfigureAwait(false);

            try
            {
                _eventBus.Publish(new LogEvent(Statuses.Fail, "Creation", treatingTypeDto.GetType().Name.Replace("Dto", ""), "Changes was not saved in data base"));
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch
            {
                throw new Exception(_sharedLocalizer["ObjectNotCreated"]);
            }
        }

        /// <summary> Updates treating type in DB. </summary>
        /// <param name="treatingTypeDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ValidationException"></exception>
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
                catch
                {
                    _eventBus.Publish(new LogEvent(Statuses.Fail, "Update", treatingTypeDto.GetType().Name.Replace("Dto", ""), "Changes was not saved in data base"));
                    throw new Exception(_sharedLocalizer["ObjectNotUpdated"]);
                }
            }
            else
            {
                throw new ValidationException(_sharedLocalizer["NotExistObjectForUpdating"]);
            }
        }

        /// <summary> Deletes treating type from DB. </summary>
        /// <param name="treatingTypeId"></param>
        /// <returns> Returns operation status code. </returns>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task DeleteAsync(int? treatingTypeId)
        {
            if (treatingTypeId == null)
            {
                throw new ValidationException(_sharedLocalizer["InvalidObjectId"]);
            }

            var treatingTypeDb = await _context.TreatingTypes.SingleOrDefaultAsync(_ => _.Id == treatingTypeId).ConfigureAwait(false);

            if (treatingTypeDb != null)
            {
                _context.TreatingTypes.Remove(treatingTypeDb);

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch
                {
                    _eventBus.Publish(new LogEvent(Statuses.Fail, "Deletion", GetType().Name.Replace("Service", ""), "Changes was not saved in data base"));
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
