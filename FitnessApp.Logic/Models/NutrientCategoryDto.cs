namespace FitnessApp.Logic.Models
{
    public class NutrientCategoryDto
    {

        public const string ENTITY_TYPE = "NutrientCategory";
        public int? Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<NutrientDto> Nutrients { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
