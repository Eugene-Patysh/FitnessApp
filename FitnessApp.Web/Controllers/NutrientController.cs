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
    [Route("api/v1.0/nutrient")]
    public class NutrientController : ControllerBase
    {
        private readonly INutrientService _nutrientService;
        private readonly ICustomValidator<NutrientDto> _validator;

        public NutrientController(INutrientService nutrientService, ICustomValidator<NutrientDto> validator)
        {
            _nutrientService = nutrientService;
            _validator = validator;
        }

        /// <summary> Gets all nutrients from DB. </summary>
        /// <returns> Returns collection of nutrients. </returns>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Not found collection of objects. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ICollection<NutrientDto>> GetAllAsync()
        {
            return await _nutrientService.GetAllAsync() ?? throw new Exception($"There are not objects of nutrients.");
        }

        /// <summary> Outputs paginated nutrients from DB, depending on the selected conditions.</summary>
        /// <param name="request"></param>
        /// <returns> Returns a PaginationResponse object containing a sorted collection of nutrients. </returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<PaginationResponse<NutrientDto>> GetPaginationAsync([FromBody] PaginationRequest request)
        {
            return await _nutrientService.GetPaginationAsync(request) ?? throw new Exception($"There are not objects of nutrients."); ;
        }

        /// <summary> Gets nutrient from DB by Id. </summary>
        /// <param name="nutrientId" example="666">The nutrient Id. </param>
        /// <returns> Returns object of nutrient with Id: <paramref name="nutrientId"/>. </returns>
        /// <remarks> Field "id" must be only positive number </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Object with this Id not found. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpGet("{nutrientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<NutrientDto> GetByIdAsync(int? nutrientId)
        {
            if (nutrientId == null)
                throw new ValidationException($"Nutrient Id can't be null or equals zero and less.");

            return await _nutrientService.GetByIdAsync(nutrientId) ?? throw new Exception($"Object of nutrient with this Id not exist.");
        }

        /// <summary> Creates new nutrient. </summary>
        /// <param name="NutrientDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Required fields: "title" (Lenght:1-30 symbols; restriction: only letters); "nutrientCategoryId", "dailyDose" (must be positive number). </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="400"> Incorrect request of object creation. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(NutrientDto), typeof(NutrientCreateExample))]
        public async Task CreateAsync([FromBody] NutrientDto NutrientDto)
        {
            _validator.Validate(NutrientDto, "AddNutrient");

            await _nutrientService.CreateAsync(NutrientDto);
        }

        /// <summary> Updates nutrient in DB. </summary>
        /// <param name="NutrientDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Required fields: "title" (Lenght:1-30 symbols; restriction: only letters); "id", "nutrientCategoryId", "dailyDose" (must be positive number). </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> That object not found.  </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(NutrientDto), typeof(NutrientUpdateExample))]
        public async Task UpdateAsync([FromBody] NutrientDto NutrientDto)
        {
            _validator.Validate(NutrientDto, "UpdateNutrient");

            await _nutrientService.UpdateAsync(NutrientDto);
        }

        /// <summary> Deletes nutrient from DB. </summary>
        /// <param name="nutrientId" example="666"> The nutrient Id. </param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Field "id" must be only positive number. </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Object with this Id not found. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpDelete("{nutrientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task DeleteAsync(int? nutrientId)
        {
            if (nutrientId == null)
                throw new ValidationException($"Nutrient Id can't be null or equals zero and less.");

            await _nutrientService.DeleteAsync(nutrientId);
        }
    }
}



