namespace FitnessApp.Logging.Events
{
    public static class DictionaryForEvent
    {
        public static Dictionary<string, string> ActionVariations { get; set; } = new()
        {
            ["Creating"] = "Object creating with title",
            ["Updating"] = "Object updating with new title",
            ["Deleting"] = "Object deleting with title",
            ["Logging"] = "Logging"
        };
        public static Dictionary<string, string> StatusVariations { get; set; } = new()
        {
            ["Success"] = " was successful.",
            ["Fail"] = " was failed."
        };
    }
}
