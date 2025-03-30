using Domain.Entities;

namespace Repository.Context.DataSeed
{
    public static class DataSeed
    {
        public static async Task SeedAsync(this SensorContext context)
        {
            try
            {
                if (context.SensorData.Any()) { return; }

                context.SensorData.AddRange(new List<SensorData>
                {
                    new()
                    {
                        EnqueuedTime = new DateTime(2025, 1, 1, 12, 10, 15, DateTimeKind.Utc),
                        Name = "Inside Temperature",
                        SensorId = "nax_temperature_sensor_1",
                        Value = "15.1"
                    },
                    new()
                    {
                        EnqueuedTime = new DateTime(2025, 1, 1, 12, 10, 15, DateTimeKind.Utc),
                        Name = "Outside Temperature",
                        SensorId = "nax_temperature_sensor_2",
                        Value = "14.0"
                    },
                    new()
                    {
                        EnqueuedTime = new DateTime(2025, 1, 1, 12, 11, 15, DateTimeKind.Utc),
                        Name = "Inside Temperature",
                        SensorId = "nax_temperature_sensor_1",
                        Value = "15.5"
                    },
                    new()
                    {
                        EnqueuedTime = new DateTime(2025, 1, 1, 12, 11, 15, DateTimeKind.Utc),
                        Name = "Outside Temperature",
                        SensorId = "nax_temperature_sensor_2",
                        Value = "14.9"
                    },
                    new()
                    {
                        EnqueuedTime = new DateTime(2025, 2, 12, 10, 10, 0, DateTimeKind.Utc),
                        Name = "Inside Lock",
                        SensorId = "nax_open_closed_sensor_1",
                        Value = "Open"
                    },
                    new()
                    {
                        EnqueuedTime = new DateTime(2025, 1, 1, 12, 11, 15, DateTimeKind.Utc),
                        Name = "Outside Temperature",
                        SensorId = "nax_temperature_sensor_2",
                        Value = "SOMETHING CANNOT BE PARSE TO DOUBLE - ERROR"
                    },
                });

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
