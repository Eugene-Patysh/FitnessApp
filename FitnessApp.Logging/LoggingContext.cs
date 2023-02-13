using FitnessApp.Logging.Configurations;
using FitnessApp.Logging.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Logging
{
    public class LoggingContext : DbContext
    {
        public LoggingContext(DbContextOptions<LoggingContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new LogConfiguration());
        }
    }
}
