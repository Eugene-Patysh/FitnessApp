namespace FitnessApp.Logic.Models
{
    public class ProductDto
    {
        public const string ENTITY_TYPE = "Product";
        public int? Id { get; set; }
        public string Title { get; set; }
        public ProductSubCategoryDto ProductSubCategory { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public virtual ICollection<ProductNutrientDto> ProductNutrients { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
