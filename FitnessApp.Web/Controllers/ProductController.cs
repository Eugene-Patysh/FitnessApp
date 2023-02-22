using FitnessApp.Logic.ApiModels;
using FitnessApp.Logic.Models;
using FitnessApp.Logic.Services;
using FitnessApp.Logic.Validators;
using FitnessApp.Web.SwaggerExamples;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Filters;
using FitnessApp.Localization;
using EventBus.Base.Standard;
using FitnessApp.Logging.Events;

namespace FitnessApp.Web.Controllers
{
    [ApiController]
    [Route("api/v1.0/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICustomValidator<ProductDto> _validator;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly IEventBus _eventBus;

        public ProductController(IProductService productService, ICustomValidator<ProductDto> validator, 
            IStringLocalizer<SharedResource> sharedLocalizer, IEventBus eventBus)
        {
            _productService = productService;
            _validator = validator;
            _sharedLocalizer = sharedLocalizer;
            _eventBus = eventBus;
        }

        /// <summary> Gets all products from DB. </summary>
        /// <returns> Returns collection of products. </returns>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Not found collection of objects. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ICollection<ProductDto>> GetAllAsync()
        {
            return await _productService.GetAllAsync();
        }

        /// <summary> Outputs paginated products from DB, depending on the selected conditions.</summary>
        /// <param name="request"></param>
        /// <returns> Returns a PaginationResponse object containing a sorted collection of products. </returns>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Not found collection of objects. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        /// <exception cref="Exception"></exception>
        [HttpPost("pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<PaginationResponse<ProductDto>> GetPaginationAsync([FromBody] PaginationRequest request)
        {
            return await _productService.GetPaginationAsync(request);
        }

        /// <summary> Gets product from DB by Id. </summary>
        /// <param name="productId" example="666">The product Id. </param>
        /// <returns> Returns object of product with Id: <paramref name="productId"/>. </returns>
        /// <remarks> Field "id" must be only positive number </remarks>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="Exception"></exception>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Object with this Id not found. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ProductDto> GetByIdAsync(int? productId)
        {
            if (productId == null)
                throw new ValidationException(_sharedLocalizer["ObjectIdCantBeNull"]);

            return await _productService.GetByIdAsync(productId) ?? throw new Exception(_sharedLocalizer["NotExistObjectWithThisId"]);
        }

        /// <summary> Creates new product. </summary>
        /// <param name="productDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Required fields: "title" (Lenght:1-30 symbols; restriction: only letters); "productSubCategoryId" (must be positive number). </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="400"> Incorrect request of object creation. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(ProductDto), typeof(ProductCreateExample))]
        public async Task CreateAsync([FromBody] ProductDto productDto)
        {
            _validator.Validate(productDto, "AddProduct");

            await _productService.CreateAsync(productDto);
            _eventBus.Publish(new LogEvent(Statuses.Success, Actions.Creation, EntityTypes.Product, productDto));
        }

        /// <summary> Updates product in DB. </summary>
        /// <param name="productDto"></param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Required fields: "title" (Lenght:1-30 symbols; restriction: only letters); "id","productSubCategoryId" (must be positive number). </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> That object not found.  </response>
        /// <response code="500"> Something wrong on the Server. </response>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(ProductDto), typeof(ProductUpdateExample))]
        public async Task UpdateAsync([FromBody] ProductDto productDto)
        {
            _validator.Validate(productDto, "UpdateProduct");

            await _productService.UpdateAsync(productDto);
            _eventBus.Publish(new LogEvent(Statuses.Success, Actions.Update, EntityTypes.Product, productDto));
        }

        /// <summary> Deletes product from DB. </summary>
        /// <param name="productId" example="666"> The product Id. </param>
        /// <returns> Returns operation status code. </returns>
        /// <remarks> Field "id" must be only positive number. </remarks>
        /// <response code="200"> Sucsess. </response>
        /// <response code="404"> Object with this Id not found. </response>
        /// <response code="500"> Something wrong on the Server. </response>
        /// <exception cref="ValidationException"></exception>
        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task DeleteAsync(int? productId)
        {
            if (productId == null)
                throw new ValidationException(_sharedLocalizer["ObjectIdCantBeNull"]);

            await _productService.DeleteAsync(productId);
            _eventBus.Publish(new LogEvent(Statuses.Success, Actions.Deletion, EntityTypes.Product, $"with ID: {productId}"));
        }
    }
}


