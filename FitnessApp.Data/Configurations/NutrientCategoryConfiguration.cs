using FitnessApp.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Data.Configurations
{
    internal class NutrientCategoryConfiguration : IEntityTypeConfiguration<NutrientCategoryDb>
    {
        public void Configure(EntityTypeBuilder<NutrientCategoryDb> builder)
        {
            builder.ToTable("NutrientCategories").HasKey(t => t.Id); // configure table name and set primary key
            builder.Property(_ => _.Id).ValueGeneratedOnAdd(); // auto creating id when entity is added
            builder.Property(_ => _.Title).IsRequired().HasMaxLength(30); // field to require for fill. Set numbers of max symbols
        }
    }
}
