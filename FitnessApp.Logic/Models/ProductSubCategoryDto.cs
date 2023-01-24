namespace FitnessApp.Logic.Models
{
    public class ProductSubCategoryDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public int? ProductCategoryId { get; set; }
        public ProductCategoryDto ProductCategory { get; set; }
        public virtual ICollection<ProductDto> Products { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
