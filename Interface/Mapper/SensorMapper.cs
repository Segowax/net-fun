using Domain.DTOs;
using Domain.Entities;

namespace Interface.Mapper
{
    public static class SensorMapper
    {
        public static SensorDataDTO MapSensorDataToDto(this SensorData sensorData) =>
            new()
            {
                EnqueuedTime = sensorData.EnqueuedTime,
                MeasurementTime = sensorData.MeasurementTime,
                Name = sensorData.Name,
                PropertyId = sensorData.PropertyId,
                Value = sensorData.Value,
            };
    }
}
