using EventBus.Base.Standard;

namespace FitnessApp.Logging.Events
{
    public class LogEvent : IntegrationEvent
    {
        public string? Action { get; set; }
        public string? Status { get; set; }
        public string? TitleDto { get; set; }
    }
}
