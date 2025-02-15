namespace Domain.DTOs
{
    public abstract class BaseSensorDataDto
    {
        public required string SensorId { get; set; }
        public required string Name { get; set; }
        public DateTime MeasurementTime { get; set; }
        public DateTime EnqueuedTime { get; set; }
        public virtual required object Value { get; set; }
    }
}
