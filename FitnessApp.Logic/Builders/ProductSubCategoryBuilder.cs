using FitnessApp.Data.Models;
using FitnessApp.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Builders
{
    public static class ProductSubCategoryBuilder
    {
        public static ProductSubCategoryDto Build(ProductSubCategoryDb db)
        {
            return new ProductSubCategoryDto()
            {
                Id = db.Id,
                Title = db.Title,
                ProductCategoryId = db.Id,
                ProductCategory = ProductCategoryBuilder.Build(db.ProductCategory),
                Products = ProductBuilder.Build(db.Products),
                Created = db.Created,
                Updated = db.Updated
            };
        }

        public static ProductSubCategoryDto[] Build(ProductSubCategoryDb[] dbs)
        {
            return dbs.Select(db => Build(db)).ToArray();
        }

        public static ICollection<ProductSubCategoryDto> Build(ICollection<ProductSubCategoryDb> col)
        {
            return col.Select(a => Build(a)).ToArray();
        }

        public static ProductSubCategoryDb Build(ProductSubCategoryDto db)
        {
            return new ProductSubCategoryDb()
            {
                Id = db.Id,
                Title = db.Title,
                ProductCategoryId = db.Id,
                ProductCategory = ProductCategoryBuilder.Build(db.ProductCategory),
                Products = ProductBuilder.Build(db.Products),
                Created = db.Created,
                Updated = db.Updated
            };
        }

        public static ProductSubCategoryDb[] Build(ProductSubCategoryDto[] dbs)
        {
            return dbs.Select(db => Build(db)).ToArray();
        }

        public static ICollection<ProductSubCategoryDb> Build(ICollection<ProductSubCategoryDto> col)
        {
            return col.Select(a => Build(a)).ToArray();
        }
    }
}
