namespace FitnessApp.Logic.ApiModels
{
    public class PaginationRequest
    {
        public string Query { get; set; }
        public string SortBy { get; set; }
        public int? Skip { get; set; } = 0;
        public int? Take { get; set; } = 10;
        public bool Ascending { get; set; } = true;

    }
}
