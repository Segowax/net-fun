namespace Domain.DTOs
{
    public class SensorDataDto
    {
        public required string SensorId { get; set; }
        public required string Name { get; set; }
        public required string Value { get; set; }
        public DateTime MeasurementTime { get; set; }
        public DateTime EnqueuedTime { get; set; }
    }
}
