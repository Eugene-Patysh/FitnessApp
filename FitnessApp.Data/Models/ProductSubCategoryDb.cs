namespace FitnessApp.Data.Models
{
    public class ProductSubCategoryDb
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public int? ProductCategoryId { get; set; }
        public ProductCategoryDb ProductCategory { get; set; }
        public virtual ICollection<ProductDb> Products { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
