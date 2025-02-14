namespace Domain
{
    public class Sensor
    {
        public int PropertyId { get; set; }
        public required string Name { get; set; }
        public required object Value { get; set; }
        public DateTime MeasurementTime { get; set; }
        public DateTime EnqueuedTime { get; set; }
    }
}
