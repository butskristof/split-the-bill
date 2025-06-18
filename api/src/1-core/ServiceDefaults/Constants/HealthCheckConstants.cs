namespace SplitTheBill.ServiceDefaults.Constants;

internal static class HealthCheckConstants
{
    internal static class Endpoints
    {
        internal const string BasePath = "/health";
    }

    internal static class Tags
    {
        internal const string Live = "live";
        internal const string Ready = "ready";
    }

    internal static class Names
    {
        internal const string Self = "self";
    }
}
