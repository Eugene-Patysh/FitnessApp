using FitnessApp.Data;
using FitnessApp.Logic.Services;
using Microsoft.EntityFrameworkCore;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Get ConnectionString from appsettings.json
var connectionString = builder.Configuration.GetValue<string>("ProductDbConnection");

// Connection to PostgreSQL
builder.Services.AddDbContext<ProductContext>(options => {
    options.UseNpgsql(connectionString);
});

builder.Services.AddSingleton<IProductCategoryService, ProductCategoryService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
