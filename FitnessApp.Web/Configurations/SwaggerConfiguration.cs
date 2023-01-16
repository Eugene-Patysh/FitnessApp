using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace FitnessApp.Web.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void Configure(WebApplicationBuilder builder)
        {
            // Update builder to Add API information and description
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "FitnessApp API",
                    Description = "An ASP.NET Core Web API for managing ToDo items",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Pavel Kostyrko",
                        Email = "mr_pablo@mail.ru",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                options.ExampleFilters();

                // Add XML-documentation: o.IncludeXmlComments(xmlPath.Combine(AppContext.BaseDirectory, xmlFile)) and don`t forget change in <PropertyGroup> of .csproj-file
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            });

            builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
        }

        public static void Use(WebApplication app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            app.UseSwaggerUI(options =>
            {
                // Need for displaying SwaggerDoc from AddSwaggerGen
                options.SwaggerEndpoint("/swagger/v1.0/swagger.json", "FithessApp API V1.0");
                // If we don`t want see schema information
                options.DefaultModelsExpandDepth(-1);
            });
        }
    }
}
