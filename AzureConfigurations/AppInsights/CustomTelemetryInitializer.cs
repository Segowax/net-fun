using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace AzureConfigurations.AppInsights
{
    /// <summary>
    /// A custom telemetry initializer that adds a cloud role name to Application Insights telemetry.
    /// This allows for easier identification and filtering of telemetry data based on the application's role.
    /// </summary>
    public class CustomTelemetryInitializer : ITelemetryInitializer
    {
        private readonly string _cloudRoleName;

        public CustomTelemetryInitializer(string cloudRoleName)
        {
            _cloudRoleName = cloudRoleName;
        }

        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = _cloudRoleName;
        }
    }
}
