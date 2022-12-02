using FitnessApp.Data.Models;
using FitnessApp.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Builders
{
    public static class NutrientCategoryBuilder
    {
        public static NutrientCategoryDto Build(NutrientCategoryDb db)
        {
            return new NutrientCategoryDto()
            {
                Id = db.Id,
                Title = db.Title,
                Nutrients = NutrientBuilder.Build(db.Nutrients),
                Created = db.Created,
                Updated = db.Updated
            };
        }

        public static NutrientCategoryDto[] Build(NutrientCategoryDb[] dbs)
        {
            return dbs.Select(db => Build(db)).ToArray();
        }

        public static NutrientCategoryDb Build(NutrientCategoryDto db)
        {
            return new NutrientCategoryDb()
            {
                Id = db.Id,
                Title = db.Title,
                Nutrients = NutrientBuilder.Build(db.Nutrients),
                Created = db.Created,
                Updated = db.Updated
            };
        }

        public static NutrientCategoryDb[] Build(NutrientCategoryDto[] dbs)
        {
            return dbs.Select(db => Build(db)).ToArray();
        }
    }
}
