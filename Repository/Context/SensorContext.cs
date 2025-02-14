using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Context.Configurations;

namespace Repository.Context
{
    public class SensorContext : DbContext
    {
        public SensorContext(DbContextOptions<SensorContext> context)
            : base(context) { }

        public DbSet<SensorData> SensorData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SensorDataConfiguration());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
