using EventBus.Base.Standard;
using FitnessApp.Logging.Models;

namespace FitnessApp.Logging.Events
{
    public class LogEvent : IntegrationEvent
    {
        public Statuses Status { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public string Body { get; set; }

        public LogEvent(Statuses status, string action, string entityType, string body)
        {
            Status = status;
            Action = action;
            EntityType = entityType;
            Body = body;
        }
    }
}
