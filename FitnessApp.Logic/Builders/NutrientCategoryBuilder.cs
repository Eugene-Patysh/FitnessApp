using FitnessApp.Data.Models;
using FitnessApp.Logic.Models;

namespace FitnessApp.Logic.Builders
{
    public static class NutrientCategoryBuilder
    {
        public static NutrientCategoryDto Build(NutrientCategoryDb db)
        {
            return db != null 
                ? new NutrientCategoryDto()
                {
                    Id = db.Id,
                    Title = db.Title,
                    Nutrients = NutrientBuilder.Build(db.Nutrients),
                    Created = db.Created,
                    Updated = db.Updated
                }
                : null ;
        }

        public static ICollection<NutrientCategoryDto> Build(ICollection<NutrientCategoryDb> dbs)
        {
            return dbs?.Select(db => Build(db))?.ToList();
        }

        public static NutrientCategoryDb Build(NutrientCategoryDto db)
        {
            return db != null 
                ? new NutrientCategoryDb()
                {
                    Id = db.Id,
                    Title = db.Title,
                    Nutrients = NutrientBuilder.Build(db.Nutrients),
                    Created = db.Created,
                    Updated = db.Updated
                }
                : null;
        }

        public static ICollection<NutrientCategoryDb> Build(ICollection<NutrientCategoryDto> dbs)
        {
            return dbs?.Select(db => Build(db))?.ToList();
        }
    }
}
