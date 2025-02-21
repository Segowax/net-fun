namespace Domain.DTOs
{
    public class BaseSensorDataDto
    {
        public BaseSensorDataDto(string sensorId,
            string name,
            DateTime enqueuedTime,
            object value)
        {
            SensorId = sensorId;
            Name = name;
            EnqueuedTime = enqueuedTime;
            Value = value;
        }

        public string SensorId { get; set; }
        public string Name { get; set; }
        public DateTime EnqueuedTime { get; set; }
        public virtual object Value { get; set; }
    }
}
