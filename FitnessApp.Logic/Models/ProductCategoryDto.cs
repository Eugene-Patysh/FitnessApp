﻿namespace FitnessApp.Logic.Models
{
    public class ProductCategoryDto
    {
        public const string ENTITY_TYPE = "ProductCategory";
        public int? Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<ProductSubCategoryDto> ProductSubCategories { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
