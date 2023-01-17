using FitnessApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessApp.Data.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<ProductDb>
    {
        public void Configure(EntityTypeBuilder<ProductDb> builder)
        {
            builder.ToTable("Products").HasKey(t => t.Id); // configure table name and set primary key
            builder.Property(_ => _.Id).ValueGeneratedOnAdd(); // auto creating id when entity is added
            builder.Property(_ => _.Title).IsRequired().HasMaxLength(30); // field to require for fill. Set numbers of max symbols
            builder.HasOne(_ => _.ProductSubCategory).WithMany(_ => _.Products).HasForeignKey(_ => _.ProductSubCategoryId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
