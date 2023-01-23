using FitnessApp.Data.Models;
using FitnessApp.Logic.Models;

namespace FitnessApp.Logic.Builders
{
    public static class ProductCategoryBuilder
    {
        public static ProductCategoryDto Build(ProductCategoryDb db)
        {
            return db != null 
                ? new ProductCategoryDto()
                {
                    Id = db.Id,
                    Title = db.Title,
                    ProductSubCategories = ProductSubCategoryBuilder.Build(db.ProductSubCategories),
                    Created = db.Created,
                    Updated = db.Updated
                }
                : null;
        }

        public static ICollection<ProductCategoryDto> Build(ICollection<ProductCategoryDb> dbs)
        {
            return dbs?.Select(db => Build(db))?.ToList();
        }

        public static ProductCategoryDb Build(ProductCategoryDto db)
        {
            return db != null 
                ? new ProductCategoryDb()
                {
                    Id = db.Id,
                    Title = db.Title,
                    ProductSubCategories = ProductSubCategoryBuilder.Build(db.ProductSubCategories),
                    Created = db.Created,
                    Updated = db.Updated
                }
                : null;
        }
        public static ICollection<ProductCategoryDb> Build(ICollection<ProductCategoryDto> dbs)
        {
            return dbs?.Select(db => Build(db))?.ToList();
        }
    }
}
