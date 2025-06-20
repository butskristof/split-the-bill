namespace SplitTheBill.ServiceDefaults.Constants;

public static class HealthCheckConstants
{
    internal static class Endpoints
    {
        internal const string BasePath = "/health";
    }

    public static class Tags
    {
        public const string Live = "live";
        public const string Ready = "ready";
    }

    internal static class Names
    {
        internal const string Self = "self";
    }
}
