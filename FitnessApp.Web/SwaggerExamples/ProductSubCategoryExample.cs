using FitnessApp.Logic.Models;
using Swashbuckle.AspNetCore.Filters;

namespace FitnessApp.Web.SwaggerExamples
{
    public class ProductSubCategoryCreateExample : IExamplesProvider<ProductSubCategoryDto>
    {
        public ProductSubCategoryDto GetExamples()
        {
            return new ProductSubCategoryDto()
            {
                Title = "Title",
                ProductCategoryId = 666
            };
        }
    }

    public class ProductSubCategoryUpdateExample : IExamplesProvider<ProductSubCategoryDto>
    {
        public ProductSubCategoryDto GetExamples()
        {
            return new ProductSubCategoryDto()
            {
                Id = 666,
                Title = "Title",
                ProductCategoryId = 666
            };
        }
    }
}
