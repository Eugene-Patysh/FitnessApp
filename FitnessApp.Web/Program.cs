using FitnessApp.Data;
using FitnessApp.Web.Configurations;
using Microsoft.EntityFrameworkCore;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

SwaggerConfiguration.Configure(builder);

// Get ConnectionString from appsettings.json
var connectionString = builder.Configuration.GetValue<string>("ProductDbConnection");

// Connection to PostgreSQL
builder.Services.AddDbContext<ProductContext>(options => {
    options.UseNpgsql(connectionString);
});

ServicesConfiguration.Configure(builder);

var app = builder.Build();

SwaggerConfiguration.Use(app);

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/", () => "I am coding like a boss");

app.Run();
