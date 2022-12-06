using FitnessApp.Data.Models;
using FitnessApp.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Logic.Builders
{
    public static class TreatingTypeBuilder
    {
        public static TreatingTypeDto Build(TreatingTypeDb db)
        {
            return db!=null 
                ? new TreatingTypeDto()
                {
                    Id = db.Id,
                    Title = db.Title,
                    ProductNutrients = ProductNutrientBuilder.Build(db.ProductNutrients),
                    Created = db.Created,
                    Updated = db.Updated
                }
                : null;
        }

        public static TreatingTypeDto[] Build(TreatingTypeDb[] dbs)
        {
            return dbs?.Select(db => Build(db))?.ToArray();
        }

        public static TreatingTypeDb Build(TreatingTypeDto db)
        {
            return db!=null 
                ? new TreatingTypeDb()
                {
                    Id = db.Id,
                    Title = db.Title,
                    ProductNutrients = ProductNutrientBuilder.Build(db.ProductNutrients),
                    Created = db.Created,
                    Updated = db.Updated
                }
                : null;
        }

        public static TreatingTypeDb[] Build(TreatingTypeDto[] dbs)
        {
            return dbs?.Select(db => Build(db))?.ToArray();
        }
    }
}
