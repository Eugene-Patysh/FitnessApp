using FitnessApp.Data;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Tests
{
    public static class DatabaseInMemory
    {
        public static ProductContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductContext>().UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
            var dbContext = new ProductContext(options);
            return dbContext;
        }
    }
}
