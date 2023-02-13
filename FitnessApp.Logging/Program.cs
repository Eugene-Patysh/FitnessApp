using EventBus.Base.Standard;
using EventBus.RabbitMQ.Standard.Configuration;
using EventBus.RabbitMQ.Standard.Options;
using FitnessApp.Logging;
using FitnessApp.Logging.Events;
using FitnessApp.Logging.Services;
using Microsoft.EntityFrameworkCore;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// RabbitMQ
var rabbitMqOptions = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqOptions>();

var connectionString = builder.Configuration.GetValue<string>("ProductDbConnection");

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddDbContext<LoggingContext>(options => {
    options.UseNpgsql(connectionString);
});

builder.Services.AddTransient<LogService>();

builder.Services.AddRabbitMqConnection(rabbitMqOptions);
builder.Services.AddRabbitMqRegistration(rabbitMqOptions);

var app = builder.Build();

// RabbitMQ
var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<LogEvent, LogService>();

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/", () => "There is logging now");

app.Run();
