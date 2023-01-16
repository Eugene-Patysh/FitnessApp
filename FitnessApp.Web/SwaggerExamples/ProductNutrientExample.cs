using FitnessApp.Logic.Models;
using Swashbuckle.AspNetCore.Filters;

namespace FitnessApp.Web.SwaggerExamples
{
    public class ProductNutrientCreateExample : IExamplesProvider<ProductNutrientDto>
    {
        public ProductNutrientDto GetExamples()
        {
            return new ProductNutrientDto()
            {
                ProductId = 666,
                NutrientId = 666,
                TreatingTypeId = 666,
                Quality = 6.66
            };
        }
    }

    public class ProductNutrientUpdateExample : IExamplesProvider<ProductNutrientDto>
    {
        public ProductNutrientDto GetExamples()
        {
            return new ProductNutrientDto()
            {
                Id = 666,
                ProductId = 666,
                NutrientId = 666,
                TreatingTypeId = 666,
                Quality = 6.66
            };
        }
    }
}