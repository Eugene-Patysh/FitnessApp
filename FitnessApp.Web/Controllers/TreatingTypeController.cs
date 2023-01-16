using FitnessApp.Data.Models;
using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Services;
using FitnessApp.Logic.Validators;
using FitnessApp.Web.SwaggerExamples;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace FitnessApp.Web.Controllers
{
    [ApiController]
    [Route("api/v1.0/treatingType")]
    public class TreatingTypeController : ControllerBase
    {
        private readonly ITreatingTypeService _treatingTypeService;
        private readonly ICustomValidator<TreatingTypeDto> _validator;

        public TreatingTypeController(ITreatingTypeService treatingTypeService, ICustomValidator<TreatingTypeDto> validator)
        {
            _treatingTypeService = treatingTypeService;
            _validator = validator;
        }

        /// <summary> Gets all treating types from DB. </summary>
        /// <returns> Returns collection of treating types. </returns>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Not found collection of objects. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ICollection<TreatingTypeDto>> GetAllAsync()
        {
            return await _treatingTypeService.GetAllAsync() ?? throw new Exception($"There are not objects of treating types.");
        }

        /// <summary> Outputs paginated treating types from DB, depending on the selected conditions.</summary>
        /// <param name="request"></param>
        /// <returns> Returns a PaginationResponse object containing a sorted collection of treating types. </returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<PaginationResponse<TreatingTypeDto>> GetPaginationAsync([FromBody] PaginationRequest request)
        {
            return await _treatingTypeService.GetPaginationAsync(request) ?? throw new Exception($"There are not objects of  treating types."); ;
        }

        /// <summary> Gets treating type from DB by Id. </summary>
        /// <param name="treatingTypeId" example="666">The treating type Id. </param>
        /// <returns> Returns object of treating type with Id: <paramref name="treatingTypeId"/>. </returns>
        /// <remarks> Field "id" must be only positive number </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Object with this Id not found. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpGet("{treatingTypeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<TreatingTypeDto> GetByIdAsync(int? treatingTypeId)
        {
            if (treatingTypeId == null)
                throw new ValidationException($"Treating type Id can't be null or equals zero and less.");

            return await _treatingTypeService.GetByIdAsync(treatingTypeId) ?? throw new Exception($"Object treating type with this Id not exist.");
        }

        /// <summary> Creates new treating type. </summary>
        /// <param name="treatingTypeDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Required fields: "title" (Lenght:1-30 symbols; restriction: only letters). </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="400"> Incorrect request of object creation. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(TreatingTypeDto), typeof(TreatingTypeCreateExample))]
        public async Task CreateAsync([FromBody] TreatingTypeDto treatingTypeDto)
        {
            _validator.Validate(treatingTypeDto, "AddTreatingType");

            await _treatingTypeService.CreateAsync(treatingTypeDto);
        }

        /// <summary> Updates treating type in DB. </summary>
        /// <param name="treatingTypeDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Required fields: "title" (Lenght:1-30 symbols; restriction: only letters); "id" (must be positive number). </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> That object not found.  </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(TreatingTypeDto), typeof(TreatingTypeUpdateExample))]
        public async Task UpdateAsync([FromBody] TreatingTypeDto treatingTypeDto)
        {
            _validator.Validate(treatingTypeDto, "UpdateTreatingType");

            await _treatingTypeService.UpdateAsync(treatingTypeDto);
        }

        /// <summary> Deletes treating type from DB. </summary>
        /// <param name="treatingTypeId" example="666"> The treating type Id. </param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Field "id" must be only positive number. </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Object with this Id not found. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpDelete("{treatingTypeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task DeleteAsync(int? treatingTypeId)
        {
            if (treatingTypeId == null)
                throw new ValidationException($"Treating type Id can't be null or equals zero and less.");

            await _treatingTypeService.DeleteAsync(treatingTypeId);
        }
    }
}
