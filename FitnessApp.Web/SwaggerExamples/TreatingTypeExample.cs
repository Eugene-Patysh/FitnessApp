using FitnessApp.Logic.Models;
using Swashbuckle.AspNetCore.Filters;

namespace FitnessApp.Web.SwaggerExamples
{
    public class TreatingTypeCreateExample : IExamplesProvider<TreatingTypeDto>
    {
        public TreatingTypeDto GetExamples()
        {
            return new TreatingTypeDto()
            {
                Title = "Title"
            };
        }
    }

    public class TreatingTypeUpdateExample : IExamplesProvider<TreatingTypeDto>
    {
        public TreatingTypeDto GetExamples()
        {
            return new TreatingTypeDto()
            {
                Id = 666,
                Title = "Title"
            };
        }
    }
}
