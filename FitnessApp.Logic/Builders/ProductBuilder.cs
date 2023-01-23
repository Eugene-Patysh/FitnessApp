using FitnessApp.Data.Models;
using FitnessApp.Logic.Models;

namespace FitnessApp.Logic.Builders
{
    public static class ProductBuilder
    {
        public static ProductDto Build(ProductDb db)
        {
            return db != null 
                ? new ProductDto()
                {
                    Id = db.Id,
                    Title = db.Title,
                    ProductSubCategory = ProductSubCategoryBuilder.Build(db.ProductSubCategory),
                    ProductSubCategoryId = db.ProductSubCategoryId,
                    //ProductNutrients = ProductNutrientBuilder.Build(db.ProductNutrients),
                    Created = db.Created,
                    Updated = db.Updated
                }
                : null;
        }

        public static ICollection<ProductDto> Build(ICollection<ProductDb> col)
        {
            return col?.Select(a => Build(a))?.ToList();
        }

        public static ProductDb Build(ProductDto db)
        {
            return db != null 
                ? new ProductDb()
                {
                    Id = db.Id,
                    Title = db.Title,
                    ProductSubCategory = ProductSubCategoryBuilder.Build(db.ProductSubCategory),
                    ProductSubCategoryId = db.ProductSubCategoryId,
                    //ProductNutrients = ProductNutrientBuilder.Build(db.ProductNutrients),
                    Created = db.Created,
                    Updated = db.Updated
                }
                :null;
        }

        public static ICollection<ProductDb> Build(ICollection<ProductDto> col)
        {
            return col?.Select(a => Build(a))?.ToList();
        }
    }
}
