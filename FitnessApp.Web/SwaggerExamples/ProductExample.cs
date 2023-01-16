using FitnessApp.Logic.Models;
using Swashbuckle.AspNetCore.Filters;

namespace FitnessApp.Web.SwaggerExamples
{
    public class ProductCreateExample : IExamplesProvider<ProductDto>
    {
        public ProductDto GetExamples()
        {
            return new ProductDto()
            {
                Title = "Title",
                ProductSubCategoryId = 666
            };
        }
    }

    public class ProductUpdateExample : IExamplesProvider<ProductDto>
    {
        public ProductDto GetExamples()
        {
            return new ProductDto()
            {
                Id = 666,
                Title = "Title",
                ProductSubCategoryId = 666
            };
        }
    }
}