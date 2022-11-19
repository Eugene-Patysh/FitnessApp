using FitnessApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Data
{
    internal static class ModelBuilderExtension
    {
        // Extension methods for ModelBuilder
        internal static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDb>()
                .HasData(
                    new ProductDb
                    {
                        Id = 1,
                        Title = "Banana",
                        ProductSubCategoryId = 1 // exotic
                    },
                    new ProductDb
                    {
                        Id = 2,
                        Title = "Potato",
                        ProductSubCategoryId = 2 // tuberous (клубневый)
                    }
                );

            modelBuilder.Entity<ProductSubCategoryDb>()
                .HasData(
                    new ProductSubCategoryDb
                    {
                        Id = 1,
                        Title = "Exotic",
                        ProductCategoryId = 1 // fruits
                    },
                    new ProductSubCategoryDb
                    {
                        Id = 2,
                        Title = "Tuberous",
                        ProductCategoryId = 2 // vegetables
                    }
                );

            modelBuilder.Entity<ProductCategoryDb>()
                .HasData(
                    new ProductCategoryDb
                    {
                        Id = 1,
                        Title = "Fruits"
                    },
                    new ProductCategoryDb
                    {
                        Id = 2,
                        Title = "Vegetables"
                    }
                );

            modelBuilder.Entity<NutrientDb>()
                .HasData(
                    new NutrientDb
                    {
                        Id = 1,
                        Title = "Protein",
                        DailyDose = 0.75 , // in gramms at kilogram of weight
                        NutrientCategoryId = 1 // Macronutrients
                    },
                    new NutrientDb
                    {
                        Id = 2,
                        Title = "Сalcium",
                        DailyDose = 0.9, //in gramms. Varies depending on age
                        NutrientCategoryId = 2 // Minerals
                    }
                );

            modelBuilder.Entity<NutrientCategoryDb>()
                .HasData(
                    new NutrientCategoryDb
                    {
                        Id = 1,
                        Title = "Macronutrients"
                    },
                    new NutrientCategoryDb
                    {
                        Id = 2,
                        Title = "Minerals"
                    }
                );

            modelBuilder.Entity<TreatingTypeDb>()
                .HasData(
                    new TreatingTypeDb
                    {
                        Id = 1,
                        Title = "Fresh",
                    },
                    new TreatingTypeDb
                    {
                        Id = 2,
                        Title = "Fried",
                    }
                );

            modelBuilder.Entity<ProductNutrientDb>()
               .HasData(
                   new ProductNutrientDb
                   {
                       Id = 1
                   },
                   new ProductNutrientDb
                   {
                       Id = 2
                   }
               );
        }
    }
}
