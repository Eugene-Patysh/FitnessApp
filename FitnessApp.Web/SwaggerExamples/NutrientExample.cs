using FitnessApp.Logic.Models;
using Swashbuckle.AspNetCore.Filters;

namespace FitnessApp.Web.SwaggerExamples
{
    public class NutrientCreateExample : IExamplesProvider<NutrientDto>
    {
        public NutrientDto GetExamples()
        {
            return new NutrientDto()
            {
                Title = "Title",
                NutrientCategoryId = 666,
                DailyDose = 6.66
            };
        }
    }

    public class NutrientUpdateExample : IExamplesProvider<NutrientDto>
    {
        public NutrientDto GetExamples()
        {
            return new NutrientDto()
            {
                Id = 666,
                Title = "Title",
                NutrientCategoryId = 666,
                DailyDose = 6.6
            };
        }
    }
}