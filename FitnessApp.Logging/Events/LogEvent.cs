using EventBus.Base.Standard;
using System.Text.Json;

namespace FitnessApp.Logging.Events
{
    public class LogEvent : IntegrationEvent
    {
        public string Status { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public string Body { get; set; }

        public LogEvent(Statuses status, Actions action, EntityTypes entityType, object obj)
        {
            Status = status.ToString();
            Action = action.ToString();
            EntityType = entityType.ToString();
            Body =  JsonSerializer.Serialize(obj);
        }
    }
    public enum Statuses
    {
        None,
        Success,
        Fail,
        IsNotValid
    }
    public enum Actions
    {
        Creation,
        Update,
        Deletion
    }
    public enum EntityTypes
    {
        NutrientCategory,
        Nutrient,
        ProductCategory,
        Product,
        ProductNutrient,
        ProductSubCategory,
        TreatingType
    }   
}
