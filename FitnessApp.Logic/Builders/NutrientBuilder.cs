using FitnessApp.Data.Models;
using FitnessApp.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Builders
{
    public static class NutrientBuilder
    {
        public static NutrientDto Build(NutrientDb db)
        {
            return db != null 
                ? new NutrientDto()
                {
                    Id = db.Id,
                    Title = db.Title,
                    DailyDose = db.DailyDose,
                    NutrientCategory = NutrientCategoryBuilder.Build(db.NutrientCategory),
                    NutrientCategoryId = db.NutrientCategoryId,
                    ProductNutrients = ProductNutrientBuilder.Build(db.ProductNutrients),
                    Created = db.Created,
                    Updated = db.Updated
                }
                : null;
        }

        public static NutrientDto[] Build(NutrientDb[] dbs)
        {
            return dbs?.Select(db => Build(db))?.ToArray();
        }

        public static ICollection<NutrientDto> Build(ICollection<NutrientDb> col)
        {
            return col?.Select(a => Build(a))?.ToArray();
        }

        public static NutrientDb Build(NutrientDto db)
        {

            return db != null 
                ? new NutrientDb()
                {
                    Id = db.Id,
                    Title = db.Title,
                    DailyDose = db.DailyDose,
                    NutrientCategory = NutrientCategoryBuilder.Build(db.NutrientCategory),
                    NutrientCategoryId = db.NutrientCategoryId,
                    ProductNutrients = ProductNutrientBuilder.Build(db.ProductNutrients),
                    Created = db.Created,
                    Updated = db.Updated
                }
                : null;
        }

        public static NutrientDb[] Build(NutrientDto[] dbs)
        {
            return dbs?.Select(db => Build(db))?.ToArray();
        }

        public static ICollection<NutrientDb> Build(ICollection<NutrientDto> col)
        {
            return col?.Select(a => Build(a))?.ToArray();
        }
    }
}
