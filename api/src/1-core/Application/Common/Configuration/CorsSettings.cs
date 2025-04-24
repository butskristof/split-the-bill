using FluentValidation;
using SplitTheBill.Application.Common.Validation;

namespace SplitTheBill.Application.Common.Configuration;

public sealed record CorsSettings : ISettings
{
    public static string SectionName => "Cors";

    public required bool AllowCors { get; init; }
    public required string[] AllowedOrigins { get; init; }
}

internal sealed class CorsSettingsValidator : BaseValidator<CorsSettings>
{
    public CorsSettingsValidator()
    {
        RuleFor(s => s.AllowedOrigins)
            .NotEmpty()
            .When(s => s.AllowCors)
            .WithMessage("When CORS is enabled, at least one allowed origin must be specified");

        RuleForEach(s => s.AllowedOrigins)
            .NotEmpty()
            .WithMessage("Origin value cannot be empty")
            .Must(origin => Uri.TryCreate(origin, UriKind.Absolute, out _))
            .WithMessage("Origin value must be a valid absolute URI")
            // the BaseValidator should stop execution after the first failure, so it's safe
            // to assume we can construct a valid Uri after the check above
            .Must(origin => string.IsNullOrEmpty(new Uri(origin, UriKind.Absolute).Query))
            .WithMessage("Origin value must not contain a query string")
            .Must(origin => new Uri(origin).PathAndQuery == "/")
            .WithMessage("Origin value must not contain a path")
            .Must(origin => !origin.EndsWith('/'))
            .WithMessage("Origin value must not contain a trailing slash")
            .Must(origin => string.IsNullOrEmpty(new Uri(origin).UserInfo))
            .WithMessage("Origin value must not contain authentication information")
            .Must(origin => string.IsNullOrEmpty(new Uri(origin, UriKind.Absolute).Fragment))
            .WithMessage("Origin value must not contain a fragment")
            .Must(origin =>
                Uri.TryCreate(origin, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
            )
            .WithMessage("Origin value must use http or https scheme");
    }
}
