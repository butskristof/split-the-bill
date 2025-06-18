using Microsoft.Extensions.Configuration;

namespace SplitTheBill.ServiceDefaults.Constants;

internal static class ConfigurationConstants
{
    internal const string OtelExporterEndpoint = "OTEL_EXPORTER_OTLP_ENDPOINT";
    internal const string ApplicationInsightsConnectionString =
        "APPLICATIONINSIGHTS_CONNECTION_STRING";

    internal static bool HasValue(this IConfiguration configuration, string key) =>
        !string.IsNullOrWhiteSpace(configuration[key]);
}
