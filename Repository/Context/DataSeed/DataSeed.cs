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
                    MeasurementTime = new DateTime(2025, 1, 1, 12, 10, 25, DateTimeKind.Utc),
                    Name = "Temperature Sensor 1",
                    SensorId = "nax_sensor_temperature__1",
                    Value = "15.1"
                },
                new()
                {
                    EnqueuedTime = new DateTime(2025, 1, 1, 12, 10, 15, DateTimeKind.Utc),
                    MeasurementTime = new DateTime(2025, 1, 1, 12, 10, 25, DateTimeKind.Utc),
                    Name = "Temperature Sensor 2",
                    SensorId = "nax_sensor_temperature__2",
                    Value = "16.0"
                },
                new()
                {
                    EnqueuedTime = new DateTime(2025, 1, 1, 12, 11, 15, DateTimeKind.Utc),
                    MeasurementTime = new DateTime(2025, 1, 1, 12, 11, 25, DateTimeKind.Utc),
                    Name = "Temperature Sensor 1",
                    SensorId = "nax_sensor_temperature__1",
                    Value = "15.5"
                },
                new()
                {
                    EnqueuedTime = new DateTime(2025, 1, 1, 12, 11, 15, DateTimeKind.Utc),
                    MeasurementTime = new DateTime(2025, 1, 1, 12, 11, 25, DateTimeKind.Utc),
                    Name = "Temperature Sensor 2",
                    SensorId = "nax_sensor_temperature__2",
                    Value = "15.9"
                },
                new()
                {
                    EnqueuedTime = new DateTime(2025, 2, 12, 10, 10, 0, DateTimeKind.Utc),
                    MeasurementTime = new DateTime(2025, 2, 12, 10, 10, 10, DateTimeKind.Utc),
                    Name = "Flag Sensor 1",
                    SensorId = "nax_sensor_flag__2",
                    Value = "True"
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
