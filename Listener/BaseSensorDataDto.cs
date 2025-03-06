namespace Listener
{
    class BaseSensorDataDto
    {
        public BaseSensorDataDto() { }

        public string SensorId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime EnqueuedTime { get; set; }
        public virtual object? Value { get; set; }
    }
}
