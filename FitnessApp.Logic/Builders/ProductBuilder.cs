using FitnessApp.Data.Models;
using FitnessApp.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Builders
{
    public static class ProductBuilder
    {
        public static ProductDto Build(ProductDb db)
        {

            return new ProductDto()
            {
                Id = db.Id,
                Title = db.Title,
                ProductSubCategory = ProductSubCategoryBuilder.Build(db.ProductSubCategory),
                ProductSubCategoryId = db.ProductSubCategoryId,
                ProductNutrients = ProductNutrientBuilder.Build(db.ProductNutrients),
                Created = db.Created,
                Updated = db.Updated
            };
        }
        public static ProductDto[] Build(ProductDb[] dbs)
        {
            return dbs.Select(db => Build(db)).ToArray();
        }
        public static ICollection<ProductDto> Build(ICollection<ProductDb> col)
        {
            return col.Select(a => Build(a)).ToArray();
        }
    }
}
