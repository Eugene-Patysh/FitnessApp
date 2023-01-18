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
    [Route("api/v1.0/productNutrient")]
    public class ProductNutrientController : ControllerBase
    {
        private readonly IProductNutrientService _productNutrientService;
        private readonly ICustomValidator<ProductNutrientDto> _validator;

        public ProductNutrientController(IProductNutrientService productNutrientService, ICustomValidator<ProductNutrientDto> validator)
        {
            _productNutrientService = productNutrientService;
            _validator = validator;
        }

        /// <summary> Gets all Product-Nutrients from DB. </summary>
        /// <returns> Returns collection of Product-Nutrients. </returns>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Not found collection of objects. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ICollection<ProductNutrientDto>> GetAllAsync()
        {
            return await _productNutrientService.GetAllAsync() ?? throw new Exception($"There are not objects of Product-Nutrients.");
        }

        /// <summary> Outputs paginated Product-Nutrients from DB, depending on the selected conditions.</summary>
        /// <param name="request"></param>
        /// <returns> Returns a PaginationResponse object containing a sorted collection of Product-Nutrients. </returns>
        /// <exception cref="Exception"></exception>
        ///  <response code="200"> Sucsess. </response>
        /// <response code="404"> Not found collection of objects. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpPost("pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<PaginationResponse<ProductNutrientDto>> GetPaginationAsync([FromBody] PaginationRequest request)
        {
            return await _productNutrientService.GetPaginationAsync(request) ?? throw new Exception($"There are not objects of Product-Nutrients."); ;
        }

        /// <summary> Gets Product-Nutrient from DB by Id. </summary>
        /// <param name="productNutrientDtoId" example="666">The Product-Nutrient Id. </param>
        /// <returns> Returns object of Product-Nutrient with Id: <paramref name="productNutrientDtoId"/>. </returns>
        /// <remarks> Field "id" must be only positive number </remarks>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="Exception"></exception>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Object with this Id not found. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpGet("{productNutrientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ProductNutrientDto> GetByIdAsync(int? productNutrientDtoId)
        {
            if (productNutrientDtoId == null)
                throw new ValidationException($"Product-Nutrient Id can't be null or equals zero and less.");

            return await _productNutrientService.GetByIdAsync(productNutrientDtoId) ?? throw new Exception($"Object Product-Nutrient with this Id not exist.");
        }

        /// <summary> Creates new Product-Nutrient. </summary>
        /// <param name="productNutrientDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Required fields: "productId", "nutrientId", "treatingTypeId" (must be positive number); "quality" (must be equals or more than zero). </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="400"> Incorrect request of object creation. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(ProductNutrientDto), typeof(ProductNutrientCreateExample))]
        public async Task CreateAsync([FromBody] ProductNutrientDto productNutrientDto)
        {
            _validator.Validate(productNutrientDto, "AddproductNutrient");

            await _productNutrientService.CreateAsync(productNutrientDto);
        }

        /// <summary> Updates Product-Nutrient in DB. </summary>
        /// <param name="productNutrientDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Required fields: "id", "productId", "nutrientId", "treatingTypeId" (must be positive number); "quality" (must be equals or more than zero). </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> That object not found.  </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(ProductNutrientDto), typeof(ProductNutrientUpdateExample))]
        public async Task UpdateAsync([FromBody] ProductNutrientDto productNutrientDto)
        {
            _validator.Validate(productNutrientDto, "UpdateproductNutrient");

            await _productNutrientService.UpdateAsync(productNutrientDto);
        }

        /// <summary> Deletes Product-Nutrient from DB. </summary>
        /// <param name="productNutrientDtoId" example="666"> The Product-Nutrient Id. </param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Field "id" must be only positive number. </remarks>
        /// <exception cref="ValidationException"></exception>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Object with this Id not found. </response>
        /// <response code="500"> Something wrong on the Server. </response> 
        [HttpDelete("{productNutrientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task DeleteAsync(int? productNutrientDtoId)
        {
            if (productNutrientDtoId == null)
                throw new ValidationException($"Product-Nutrient Id can't be null or equals zero and less.");

            await _productNutrientService.DeleteAsync(productNutrientDtoId);
        }
    }
}

