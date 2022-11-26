using FitnessApp.Data.Models;
using FitnessApp.Logic.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Builders
{
    public static class ProductCategoryBuilder
    {
        public static ProductCategoryDto Build(ProductCategoryDb db)
        {
            
            return new ProductCategoryDto()
            {
                Id = db.Id,
                Title = db.Title,
                ProductSubCategories = ProductSubCategoryBuilder.Build(db.ProductSubCategories),
                Created = db.Created,
                Updated = db.Updated
            };
        }
        public static ProductCategoryDto[] Build(ProductCategoryDb[] dbs)
        {
            return dbs.Select(db => Build(db)).ToArray();
        }
    }
}
