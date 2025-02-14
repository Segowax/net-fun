using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Repository.Context.DataSeed;

namespace API
{
    public static class DatabaseMigrations
    {
        public static async Task Run(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SensorContext>();
                await context.Database.MigrateAsync();
                if (app.Environment.IsDevelopment())
                {
                    await context.SeedAsync();
                }
            }
        }
    }
}
