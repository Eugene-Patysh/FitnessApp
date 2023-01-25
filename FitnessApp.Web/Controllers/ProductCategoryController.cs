using FitnessApp.Logic.Models;
using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Services;
using FitnessApp.Logic.Validators;
using FitnessApp.Web.SwaggerExamples;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using FluentValidation;

namespace FitnessApp.Web.Controllers
{
    [ApiController]
    [Route("api/v1.0/productCategory")]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly ICustomValidator<ProductCategoryDto> _validator;

        public ProductCategoryController (IProductCategoryService productCategoryService, ICustomValidator<ProductCategoryDto> validator)
        {
            _productCategoryService = productCategoryService;
            _validator = validator;
        }

        /// <summary> Gets all product categories from DB. </summary>
        /// <returns> Returns collection of product categories. </returns>
        /// <exception cref="Exception"></exception>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Not found collection of objects. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[SwaggerResponseExample(StatusCodes.Status200OK), typeof(ProductCategoryCommonExample))]
        public async Task<ICollection<ProductCategoryDto>> GetAllAsync()
        {
            return await _productCategoryService.GetAllAsync();
        }

        /// <summary> Outputs paginated product categories from DB, depending on the selected conditions.</summary>
        /// <param name="request"></param>
        /// <returns> Returns a PaginationResponse object containing a sorted collection of product categories. </returns>
        /// <exception cref="Exception"></exception>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Not found collection of objects. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpPost("pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<PaginationResponse<ProductCategoryDto>> GetPaginationAsync([FromBody] PaginationRequest request)
        {
            return await _productCategoryService.GetPaginationAsync(request) ?? throw new Exception($"There are not objects of product categories."); ;
        }

        /// <summary> Gets product category from DB by Id. </summary>
        /// <param name="productCategoryId" example="666">The product category Id. </param>
        /// <returns> Returns object of product category with Id: <paramref name="productCategoryId"/>. </returns>
        /// <remarks> Field "id" must be only positive number </remarks>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="Exception"></exception>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Object with this Id not found. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpGet("{productCategoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ProductCategoryDto> GetByIdAsync(int? productCategoryId)
        {
            if (productCategoryId == null)
                throw new ValidationException($"Product category Id can't be null");

            return await _productCategoryService.GetByIdAsync(productCategoryId) ?? throw new Exception($"Object of product category with this Id not exist.");
        }

        /// <summary> Creates new product category. </summary>
        /// <param name="productCategoryDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Required fields: "title" (Lenght:1-30 symbols; restriction: only letters). </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="400"> Incorrect request of object creation. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(ProductCategoryDto), typeof(ProductCategoryCreateExample))]
        public async Task CreateAsync ([FromBody] ProductCategoryDto productCategoryDto)
        {
            _validator.Validate(productCategoryDto, "AddProductCategory");

            await _productCategoryService.CreateAsync(productCategoryDto);
        }

        /// <summary> Updates product category in DB. </summary>
        /// <param name="productCategoryDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Required fields: "title" (Lenght:1-30 symbols; restriction: only letters); "id" (must be positive number). </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> That object not found.  </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(ProductCategoryDto), typeof(ProductCategoryUpdateExample))]
        public async Task UpdateAsync([FromBody] ProductCategoryDto productCategoryDto)
        {
            _validator.Validate(productCategoryDto, "UpdateProductCategory");

            await _productCategoryService.UpdateAsync(productCategoryDto);
        }


        /// <summary> Deletes product category from DB. </summary>
        /// <param name="productCategoryId" example="666"> The product category Id. </param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Field "id" must be only positive number. </remarks>
        /// <exception cref="ValidationException"></exception>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Object with this Id not found. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpDelete("{productCategoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task DeleteAsync (int? productCategoryId)
        {
            if (productCategoryId == null)
                throw new ValidationException($"Product category Id can't be null");

            await _productCategoryService.DeleteAsync(productCategoryId);
        }
    }
}
