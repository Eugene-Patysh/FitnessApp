using FitnessApp.Data.Models;
using FitnessApp.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Builders
{
    public static class ProductNutrientBuilder
    {
        public static ProductNutrientDto Build(ProductNutrientDb db)
        {

            return new ProductNutrientDto()
            {
                Id = db.Id,
                Quality = db.Quality,
                Product = ProductBuilder.Build(db.Product),
                ProductId = db.ProductId,
                Nutrient = NutrientBuilder.Build(db.Nutrient),
                NutrientId = db.NutrientId,
                TreatingType = TreatingTypeBuilder.Build(db.TreatingType),
                TreatingTypeId = db.TreatingTypeId,
                Created = db.Created,
                Updated = db.Updated
            };
        }
        public static ProductNutrientDto[] Build(ProductNutrientDb[] dbs)
        {
            return dbs.Select(db => Build(db)).ToArray();
        }
        public static ICollection<ProductNutrientDto> Build(ICollection<ProductNutrientDb> col)
        {
            return col.Select(a => Build(a)).ToArray();
        }
    }
}
