using FitnessApp.Logic.Models;
using Swashbuckle.AspNetCore.Filters;

namespace FitnessApp.Web.SwaggerExamples
{
    public class NutrientCategoryCreateExample : IExamplesProvider<NutrientCategoryDto>
    {
        public NutrientCategoryDto GetExamples()
        {
            return new NutrientCategoryDto()
            {
                Title = "Title"
            };
        }
    }

    public class NutrientCategoryUpdateExample : IExamplesProvider<NutrientCategoryDto>
    {
        public NutrientCategoryDto GetExamples()
        {
            return new NutrientCategoryDto()
            {
                Id = 666,
                Title = "Title"
            };
        }
    }
}
