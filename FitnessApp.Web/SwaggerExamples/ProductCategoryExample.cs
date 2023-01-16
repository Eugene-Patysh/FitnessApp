using FitnessApp.Logic.Models;
using Swashbuckle.AspNetCore.Filters;

namespace FitnessApp.Web.SwaggerExamples
{
    public class ProductCategoryCreateExample : IExamplesProvider<ProductCategoryDto>
    {
        public ProductCategoryDto GetExamples()
        {
            return new ProductCategoryDto()
            {
                Title = "Title"
            };
        }
    }

    public class ProductCategoryUpdateExample : IExamplesProvider<ProductCategoryDto>
    {
        public ProductCategoryDto GetExamples()
        {
            return new ProductCategoryDto()
            {
                Id = 666,
                Title = "Title"
            };
        }
    }
}