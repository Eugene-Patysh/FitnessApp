using FitnessApp.Data.Models;
using FitnessApp.Logic.Models;

namespace FitnessApp.Logic.Builders
{
    public static class TreatingTypeBuilder
    {
        public static TreatingTypeDto Build(TreatingTypeDb db)
        {
            return db!= null 
                ? new TreatingTypeDto()
                {
                    Id = db.Id,
                    Title = db.Title,
                    //ProductNutrients = ProductNutrientBuilder.Build(db.ProductNutrients),
                    Created = db.Created,
                    Updated = db.Updated
                }
                : null;
        }

        public static ICollection<TreatingTypeDto> Build(ICollection<TreatingTypeDb> dbs)
        {
            return dbs?.Select(db => Build(db))?.ToList();
        }

        public static TreatingTypeDb Build(TreatingTypeDto db)
        {
            return db!= null 
                ? new TreatingTypeDb()
                {
                    Id = db.Id,
                    Title = db.Title,
                    //ProductNutrients = ProductNutrientBuilder.Build(db.ProductNutrients),
                    Created = db.Created,
                    Updated = db.Updated
                }
                : null;
        }

        public static ICollection<TreatingTypeDb> Build(ICollection<TreatingTypeDto> dbs)
        {
            return dbs?.Select(db => Build(db))?.ToList();
        }
    }
}
