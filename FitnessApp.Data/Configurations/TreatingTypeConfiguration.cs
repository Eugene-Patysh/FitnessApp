using FitnessApp.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Data.Configurations
{
    internal class TreatingTypeConfiguration : IEntityTypeConfiguration<TreatingTypeDb>
    {
        public void Configure(EntityTypeBuilder<TreatingTypeDb> builder)
        {
            builder.ToTable("TreatingTypes").HasKey(t => t.Id); // configure table name and set primary key
            builder.Property(_ => _.Id).ValueGeneratedOnAdd(); // auto creating id when entity is added
            builder.Property(_ => _.Title).IsRequired().HasMaxLength(20); // field to require for fill. Set numbers of max symbols
        }
    }
}
