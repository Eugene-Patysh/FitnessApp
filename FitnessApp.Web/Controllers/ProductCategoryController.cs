using FitnessApp.Logic.Models;
using FitnessApp.Logic.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Web.Controllers
{
    [ApiController]
    [Route("api/v1.0/productCategory")]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;
        public ProductCategoryController(IProductCategoryService productCategoryService) 
        {
            _productCategoryService = productCategoryService;
        }

        [HttpGet("{productCategoryId}")]
        public async Task<ActionResult<ProductCategoryDto>> GetByIdAsync(int? productCategoryId)
        {
            if (productCategoryId == null)
                BadRequest();

            var productCategory = await _productCategoryService.GetByIdAsync(productCategoryId);

            return productCategory == null ? NotFound() : Ok(productCategory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult> CreateAsync([FromBody, CustomizeValidator(RuleSet = "AddProductCategory")] ProductCategoryDto productCategory)
        {
            //if (productCategory == null || productCategory.Id != null)
            //    BadRequest();
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _productCategoryService.CreateAsync(productCategory);

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync([FromBody, CustomizeValidator(RuleSet = "UpdateProductCategory")] int? productCategoryId)
        {
            if (productCategoryId == null)
                BadRequest();

            await _productCategoryService.DeleteAsync(productCategoryId);

            return Ok();
        }
    }
}
