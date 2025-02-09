using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace AzureConfigurations.AppInsights
{
    public class CustomTelemetryProccessor : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor _telemetryProcessor;

        public CustomTelemetryProccessor(ITelemetryProcessor telemetryProcessor)
        {
            _telemetryProcessor = telemetryProcessor;
        }

        public void Process(ITelemetry item)
        {
            if (item is RequestTelemetry request)
            {
                if (request.Duration.Milliseconds < 500)
                {
                    return;
                }
            }

            _telemetryProcessor.Process(item);
        }
    }
}
