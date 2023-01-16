namespace FitnessApp.Logic.ApiModels
{
    public class PaginationResponse<T>
    {
        public int? Total { get; set; }
        public ICollection<T> Values { get; set; }
    }
}
