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
    [Route("api/v1.0/nutrientCategory")]
    public class NutrientCategoryController : ControllerBase
    {
        private readonly INutrientCategoryService _nutrientCategoryService;
        private readonly ICustomValidator<NutrientCategoryDto> _validator;

        public NutrientCategoryController(INutrientCategoryService nutrientCategoryService, ICustomValidator<NutrientCategoryDto> validator)
        {
            _nutrientCategoryService = nutrientCategoryService;
            _validator = validator;
        }

        /// <summary> Gets all nutirent categories from DB. </summary>
        /// <returns> Returns collection of nutirent categories. </returns>
        /// <exception cref="Exception"></exception>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Not found collection of objects. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ICollection<NutrientCategoryDto>> GetAllAsync()
        {
            return await _nutrientCategoryService.GetAllAsync() ?? throw new Exception($"There are not objects of nutrient categories.");
        }

        /// <summary> Outputs paginated nutrient categories from DB, depending on the selected conditions.</summary>
        /// <param name="request"></param>
        /// <returns> Returns a PaginationResponse object containing a sorted collection of nutrient categories. </returns>
        /// <exception cref="Exception"></exception>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Not found collection of objects. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpPost("pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<PaginationResponse<NutrientCategoryDto>> GetPaginationAsync([FromBody] PaginationRequest request)
        {
            return await _nutrientCategoryService.GetPaginationAsync(request) ?? throw new Exception($"There are not objects of nutrient categories."); ;
        }

        /// <summary> Gets nutirent category from DB by Id. </summary>
        /// <param name="nutrientCategoryDtoId" example="666"> The nutirent category Id. </param>
        /// <returns> Returns object of nutirent category with Id: <paramref name="nutrientCategoryDtoId"/>. </returns>
        /// <remarks> Field "id" must be only positive number </remarks> 
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="Exception"></exception>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Object with this Id not found. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpGet("{nutrientCategoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<NutrientCategoryDto> GetByIdAsync(int? nutrientCategoryDtoId)
        {
            if (nutrientCategoryDtoId == null)
                throw new ValidationException($"Nutrient category Id can't be null");

            return await _nutrientCategoryService.GetByIdAsync(nutrientCategoryDtoId) ?? throw new Exception($"Object of nutrient category with this Id not exist.");
        }

        /// <summary> Creates new nutrient category. </summary>
        /// <param name="nutrientCategoryDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Required fields: "title" (Lenght:1-30 symbols; restriction: only letters). </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="400"> Incorrect request of object creation. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(NutrientCategoryDto), typeof(NutrientCategoryCreateExample))]
        public async Task CreateAsync([FromBody] NutrientCategoryDto nutrientCategoryDto)
        {
            _validator.Validate(nutrientCategoryDto, "AddNutrientCategory");

            await _nutrientCategoryService.CreateAsync(nutrientCategoryDto);
        }

        /// <summary> Updates nutrient category in DB. </summary>
        /// <param name="nutrientCategoryDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Required fields: "title" (Lenght:1-30 symbols; restriction: only letters); "id" (must be positive number). </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> That object not found.  </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(NutrientCategoryDto), typeof(NutrientCategoryUpdateExample))]
        public async Task UpdateAsync([FromBody] NutrientCategoryDto nutrientCategoryDto)
        {
            _validator.Validate(nutrientCategoryDto, "UpdateNutrientCategory");

            await _nutrientCategoryService.UpdateAsync(nutrientCategoryDto);
        }

        /// <summary> Deletes nutrient category from DB. </summary>
        /// <param name="nutrientCategoryId" example="666"> The nutrient category Id. </param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Field "id" must be only positive number. </remarks>
        /// <exception cref="ValidationException"></exception>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Object with this Id not found. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpDelete("{nutrientCategoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task DeleteAsync(int? nutrientCategoryId)
        {
            if (nutrientCategoryId == null)
                throw new ValidationException($"Nutrient category Id can't be null");

            await _nutrientCategoryService.DeleteAsync(nutrientCategoryId);
        }
    }
}

