using FitnessApp.Data;
using Microsoft.EntityFrameworkCore;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Get ConnectionString from appsettings.json
var connectionString = builder.Configuration.GetValue<string>("ProductDbConnection");

// Connection to PostgreSQL
builder.Services.AddDbContext<ProductContext>(options => {
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}

app.UseCors("CorsAllowAll");

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1.0/swagger.json", "FithessApp API V1.0");
});

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => "I am coding like a boss");

app.Run();
