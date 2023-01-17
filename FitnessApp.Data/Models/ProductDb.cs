namespace FitnessApp.Data.Models
{
    public class ProductDb
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public ProductSubCategoryDb ProductSubCategory { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public virtual ICollection<ProductNutrientDb> ProductNutrients { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
