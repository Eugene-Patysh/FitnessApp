namespace FitnessApp.Logic.Models
{
    public class TreatingTypeDto
    {
        public const string ENTITY_TYPE = "TreatingType";
        public int? Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<ProductNutrientDto> ProductNutrients { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
