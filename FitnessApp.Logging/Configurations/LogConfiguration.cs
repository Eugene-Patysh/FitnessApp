using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Logging.Models;

namespace FitnessApp.Logging.Configurations
{
    internal class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("logs").HasKey(t => t.Id); 
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();
            builder.Property(_ => _.Action).IsRequired();
            builder.Property(_ => _.Status).IsRequired();
        }
    }
}
