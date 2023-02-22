using EventBus.Base.Standard;
using FitnessApp.Logging.Events;
using FitnessApp.Logging.Models;

namespace FitnessApp.Logging.Services
{
    public class LogService : IIntegrationEventHandler<LogEvent>
    {
        private readonly LoggingContext _context;

        public LogService(LoggingContext context)
        {
            _context = context;
        }

        public async Task Handle(LogEvent @event)
        {
                var log = new Log() 
                {
                    Action = @event.Action,
                    EntityType = @event.EntityType,
                    Body = @event.Body,
                    Status = @event.Status,
                    Date = DateTime.UtcNow
                };

            await _context.Logs.AddAsync(log).ConfigureAwait(false);
                
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
