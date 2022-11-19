using FitnessApp.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Data.Configurations
{
    internal class ProductNutrientConfiguration : IEntityTypeConfiguration<ProductNutrientDb>
    {
        public void Configure(EntityTypeBuilder<ProductNutrientDb> builder)
        {
            builder.ToTable("ProductsNutrients").HasKey(t => t.Id); // configure table name and set primary key
            builder.Property(_ => _.Id).ValueGeneratedOnAdd(); // auto creating id when entity is added
        }
    }
}
