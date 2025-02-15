namespace Domain.DTOs
{
    public class StringSensorDataDto : BaseSensorDataDto
    {
        public override required object Value
        {
            get => (string)base.Value;
            set => base.Value = (string)value;
        }
    }
}
