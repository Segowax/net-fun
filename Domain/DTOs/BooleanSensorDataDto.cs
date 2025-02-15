namespace Domain.DTOs
{
    public class BooleanSensorDataDto : BaseSensorDataDto
    {
        public override required object Value
        {
            get => (bool)base.Value;
            set => base.Value = (bool)value;
        }
    }
}
