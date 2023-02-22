namespace FitnessApp.Logging.Models
{
    public class Log
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
    }   
}
