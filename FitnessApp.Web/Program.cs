using FitnessApp.Data;
using FitnessApp.Web.Configurations;
using Microsoft.EntityFrameworkCore;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Localization: Added localizaton service which will enable using IStringLocalizer in the Controllers/Services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
//builder.Services.AddControllersWithViews().AddMvcLocalization();

SwaggerConfiguration.Configure(builder);

// Get ConnectionString from appsettings.json
var connectionString = builder.Configuration.GetValue<string>("ProductDbConnection");

// Connection to PostgreSQL
builder.Services.AddDbContext<ProductContext>(options => {
    options.UseNpgsql(connectionString);
});

ServicesConfiguration.Configure(builder);

var app = builder.Build();

// Localization
var supportedCultures = new[] { "en-US", "ru-RU" };
var localizationOptions =
    new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

// Localization
app.UseRequestLocalization(localizationOptions);

SwaggerConfiguration.Use(app);

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/", () => "I am coding like a boss");

app.Run();
