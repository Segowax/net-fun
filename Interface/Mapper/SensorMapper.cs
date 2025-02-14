using Domain.DTOs;
using Domain.Entities;

namespace Interface.Mapper
{
    public static class SensorMapper
    {
        public static SensorDataDto MapSensorDataToDto(this SensorData sensorData) =>
            new()
            {
                EnqueuedTime = sensorData.EnqueuedTime,
                MeasurementTime = sensorData.MeasurementTime,
                Name = sensorData.Name,
                SensorId = sensorData.SensorId,
                Value = sensorData.Value,
            };
    }
}
