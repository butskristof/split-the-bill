using FluentValidation;
using SplitTheBill.Application.Common.Validation;

namespace SplitTheBill.Application.Common.Configuration;

public sealed record AuthenticationSettings : ISettings
{
    public static string SectionName => "Authentication";

    public required string Authority { get; init; }
    public required string[] Audiences { get; init; }
}

internal sealed class AuthenticationSettingsValidator : BaseValidator<AuthenticationSettings>
{
    public AuthenticationSettingsValidator()
    {
        RuleFor(s => s.Authority)
            .Cascade(CascadeMode.Stop)
            .NotEmptyWithErrorCode()
            .Must(origin => Uri.TryCreate(origin, UriKind.Absolute, out _))
            .WithMessage("Authority value must be a valid absolute URI")
            .Must(origin => new Uri(origin, UriKind.Absolute).Scheme is "http" or "https")
            .WithMessage("Authority value must be a valid HTTP or HTTPS URI");

        RuleFor(s => s.Audiences)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage(ErrorCodes.Required)
            .NotEmpty()
            .WithMessage(ErrorCodes.Required);

        RuleForEach(r => r.Audiences)
            .NotEmpty()
            .WithMessage(ErrorCodes.Invalid);
    }
}