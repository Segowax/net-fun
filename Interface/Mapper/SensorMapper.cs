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
                return new DoubleSensorDataDto(
                    sensorId: sensorData.SensorId,
                    name: sensorData.Name,
                    enqueuedTime: sensorData.EnqueuedTime,
                    value: doubleResult);
            }
            else if (bool.TryParse(sensorData.Value, out bool boolResult))
            {
                return new BooleanSensorDataDto(
                    sensorId: sensorData.SensorId,
                    name: sensorData.Name,
                    enqueuedTime: sensorData.EnqueuedTime,
                    value: boolResult);
            }
            else
            {
                return new StringSensorDataDto(
                    sensorId: sensorData.SensorId,
                    name: sensorData.Name,
                    enqueuedTime: sensorData.EnqueuedTime,
                    value: sensorData.Value.ToString());
            }
        }

        public static DoubleSensorDataDto? MapToDoubleSensorDataDto(this SensorData sensorData)
        {
            if (double.TryParse(sensorData.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double doubleResult))
            {
                return new DoubleSensorDataDto(
                    sensorId: sensorData.SensorId,
                    name: sensorData.Name,
                    enqueuedTime: sensorData.EnqueuedTime,
                    value: doubleResult);
            }

            return null;
        }

        public static SensorData MapDtoToSensorDataEntity(this BaseSensorDataDto sensorDataDto) =>
            new SensorData()
            {
                EnqueuedTime = sensorDataDto.EnqueuedTime,
                Name = sensorDataDto.Name,
                SensorId = sensorDataDto.SensorId,
                Value = sensorDataDto.Value.ToString() ?? throw new Exception("ToDo")
            };
    }
}
