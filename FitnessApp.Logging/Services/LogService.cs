using EventBus.Base.Standard;
using FitnessApp.Localization;
using FitnessApp.Logging.Events;
using FitnessApp.Logging.Models;
using Microsoft.Extensions.Localization;

namespace FitnessApp.Logging.Services
{
    public class LogService : IIntegrationEventHandler<LogEvent>
    {
        private readonly LoggingContext _context;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public LogService(LoggingContext context, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _context = context;
            _sharedLocalizer = sharedLocalizer;
        }

        public async Task Handle(LogEvent @event)
        {
            if (!string.IsNullOrEmpty(@event.Action) && !string.IsNullOrEmpty(@event.Status))
            {
                var logGood = new Log() 
                { 
                    Action = $"{@event.Action} '{@event.TitleDto}'", 
                    Status = @event.Status, 
                    LoggingTime = DateTime.UtcNow 
                };

                await _context.Logs.AddAsync(logGood).ConfigureAwait(false);
            }
            
            else
            {
                var logBad = new Log()
                {
                    Action = DictionaryForEvent.ActionVariations["Logging"],
                    Status = DictionaryForEvent.StatusVariations["Fail"],
                    LoggingTime = DateTime.UtcNow
                };

                await _context.Logs.AddAsync(logBad).ConfigureAwait(false);
            }

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch
            {
                throw new Exception(_sharedLocalizer["LoggingError"]);
            }
        }
    }
}
