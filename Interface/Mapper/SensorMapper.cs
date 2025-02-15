using Domain.DTOs;
using Domain.Entities;
using System.Globalization;

namespace Interface.Mapper
{
    public static class SensorMapper
    {
        public static BaseSensorDataDto MapSensorDataToDto(this SensorData sensorData)
        {
            if (double.TryParse(sensorData.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double doubleResult))
            {
                return new DoubleSensorDataDto()
                {
                    EnqueuedTime = sensorData.EnqueuedTime,
                    MeasurementTime = sensorData.MeasurementTime,
                    Name = sensorData.Name,
                    SensorId = sensorData.SensorId,
                    Value = doubleResult
                };
            }
            else if (bool.TryParse(sensorData.Value, out bool boolResult))
            {
                return new BooleanSensorDataDto()
                {
                    EnqueuedTime = sensorData.EnqueuedTime,
                    MeasurementTime = sensorData.MeasurementTime,
                    Name = sensorData.Name,
                    SensorId = sensorData.SensorId,
                    Value = boolResult
                };
            }
            else
            {
                return new StringSensorDataDto()
                {
                    EnqueuedTime = sensorData.EnqueuedTime,
                    MeasurementTime = sensorData.MeasurementTime,
                    Name = sensorData.Name,
                    SensorId = sensorData.SensorId,
                    Value = sensorData.Value
                };
            }
        }
    }
}
