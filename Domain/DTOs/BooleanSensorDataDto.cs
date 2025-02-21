
namespace Domain.DTOs
{
    public class BooleanSensorDataDto : BaseSensorDataDto
    {
        public BooleanSensorDataDto(string sensorId,
            string name,
            DateTime enqueuedTime,
            object value) : base(sensorId, name, enqueuedTime, value)
        {
            SensorId = sensorId;
            Name = name;
            EnqueuedTime = enqueuedTime;
            Value = value;
        }

        public override object Value
        {
            get => (bool)base.Value;
            set => base.Value = (bool)value;
        }
    }
}
