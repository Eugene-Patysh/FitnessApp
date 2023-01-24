using FitnessApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Data
{
    internal static class ModelBuilderExtension
    {
        // Extension methods for ModelBuilder
        internal static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategoryDb>()
               .HasData(
                   new ProductCategoryDb
                   {
                       Id = 1,
                       Title = "Fruits",
                       Created = DateTime.Now,
                       Updated = DateTime.Now
                   },
                   new ProductCategoryDb
                   {
                       Id = 2,
                       Title = "Vegetables",
                       Created = DateTime.Now,
                       Updated = DateTime.Now
                   }
               );

            modelBuilder.Entity<ProductSubCategoryDb>()
                .HasData(
                    new ProductSubCategoryDb
                    {
                        Id = 1,
                        Title = "Exotic",
                        ProductCategoryId = 1, // fruits
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    },
                    new ProductSubCategoryDb
                    {
                        Id = 2,
                        Title = "Tuberous",
                        ProductCategoryId = 2, // vegetables
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    }
                );

            modelBuilder.Entity<ProductDb>()
                .HasData(
                    new ProductDb
                    {
                        Id = 1,
                        Title = "Banana",
                        ProductSubCategoryId = 1, // exotic
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    },
                    new ProductDb
                    {
                        Id = 2,
                        Title = "Potato",
                        ProductSubCategoryId = 2, // tuberous (клубневый)
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    }
                );

            modelBuilder.Entity<NutrientCategoryDb>()
               .HasData(
                   new NutrientCategoryDb
                   {
                       Id = 1,
                       Title = "Macronutrients",
                       Created = DateTime.Now,
                       Updated = DateTime.Now
                   },
                   new NutrientCategoryDb
                   {
                       Id = 2,
                       Title = "Minerals",
                       Created = DateTime.Now,
                       Updated = DateTime.Now
                   }
               );

            modelBuilder.Entity<NutrientDb>()
                .HasData(
                    new NutrientDb
                    {
                        Id = 1,
                        Title = "Protein",
                        DailyDose = 0.75, // in gramms at kilogram of weight
                        NutrientCategoryId = 1, // Macronutrients
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    },
                    new NutrientDb
                    {
                        Id = 2,
                        Title = "Сalcium",
                        DailyDose = 0.9, //in gramms. Varies depending on age
                        NutrientCategoryId = 2, // Minerals
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    }
                );

            modelBuilder.Entity<TreatingTypeDb>()
                .HasData(
                    new TreatingTypeDb
                    {
                        Id = 1,
                        Title = "Fresh",
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    },
                    new TreatingTypeDb
                    {
                        Id = 2,
                        Title = "Fried",
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    }
                );

            modelBuilder.Entity<ProductNutrientDb>()
               .HasData(
                   new ProductNutrientDb
                   {
                       Id = 1,
                       ProductId = 1,
                       NutrientId = 1,
                       TreatingTypeId = 1,
                       Quality = 0.8,
                       Created = DateTime.Now,
                       Updated = DateTime.Now
                   },
                   new ProductNutrientDb
                   {
                       Id = 2,
                       ProductId = 2,
                       NutrientId = 2,
                       TreatingTypeId = 2,
                       Quality = 0.9,
                       Created = DateTime.Now,
                       Updated = DateTime.Now
                   }
               );
        }
    }
}
