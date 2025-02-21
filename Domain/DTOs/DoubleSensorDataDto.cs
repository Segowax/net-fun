
namespace Domain.DTOs
{
    public class DoubleSensorDataDto : BaseSensorDataDto
    {
        public DoubleSensorDataDto(string sensorId,
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
            get => (double)base.Value; 
            set => base.Value = (double)value;
        }
    }
}
