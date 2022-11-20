using FitnessApp.Data.Configurations;
using FitnessApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<ProductNutrientDb> ProductNutrients { get; set; }
        public DbSet<TreatingTypeDb> TreatingTypes { get; set; }
        public DbSet<ProductDb> Products { get; set; }
        public DbSet<ProductCategoryDb> ProductCategories { get; set; }
        public DbSet<ProductSubCategoryDb> ProductSubCategory { get; set; }
        public DbSet<NutrientDb> Nutrients { get; set; }
        public DbSet<NutrientCategoryDb> NutrientCategories { get; set; }

        // Override OnModelCreating of DbContext for Custom Model Configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductNutrientConfiguration());
            modelBuilder.ApplyConfiguration(new TreatingTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NutrientConfiguration());
            modelBuilder.ApplyConfiguration(new NutrientCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductSubCategoryConfiguration());
                  
            modelBuilder.SeedData(); // add default data
        }

        // MOVE TO PROGRAM
        // Connection to PostgreSql DataBase
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     // use postgreSQL
        //     optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=organization;Username=postgres;Password=123");
        // }
    }
}
