namespace SplitTheBill.Application.Common.Configuration;

public sealed class CorsSettings : ISettings
{
    public static string SectionName => "Cors";

    public required bool AllowCors { get; init; }
    public required string[] AllowedOrigins { get; init; }
}