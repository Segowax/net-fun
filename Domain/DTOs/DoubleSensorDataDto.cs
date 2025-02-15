namespace Domain.DTOs
{
    public class DoubleSensorDataDto : BaseSensorDataDto
    {
        public override required object Value
        {
            get => (double)base.Value; 
            set => base.Value = (double)value;
        }
    }
}
