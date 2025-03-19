using FluentValidation;
using SplitTheBill.Application.Common.Constants;

namespace SplitTheBill.Application.Common.Validation;

internal static class FluentValidationExtensions
{
    internal static IRuleBuilderOptions<T, TProperty?> NotNullWithErrorCode<T, TProperty>(
        this IRuleBuilder<T, TProperty?> ruleBuilder, string errorCode = ErrorCodes.Required)
        => ruleBuilder
            .NotNull()
            .WithMessage(errorCode);

    internal static IRuleBuilderOptions<T, TProperty> NotEmptyWithErrorCode<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder, string errorCode = ErrorCodes.Required)
        => ruleBuilder
            .NotEmpty()
            .WithMessage(errorCode);

    internal static IRuleBuilderOptions<T, Guid?> NotNullOrEmptyWithErrorCode<T>(
        this IRuleBuilder<T, Guid?> ruleBuilder, 
        string errorCodeWhenNull = ErrorCodes.Required,
        string errorCodeWhenEmpty = ErrorCodes.Invalid)
    {
        return ruleBuilder
            .NotNull()
            .WithMessage(errorCodeWhenNull)
            .Must(v => v.HasValue && v.Value != Guid.Empty)
            .WithMessage(errorCodeWhenEmpty);
    }

    internal static IRuleBuilderOptions<T, string?> ValidString<T>(this IRuleBuilder<T, string?> ruleBuilder,
        bool required,
        int maxLength = ApplicationConstants.DefaultMaxStringLength)
    {
        if (required)
            ruleBuilder = ruleBuilder.NotEmptyWithErrorCode();

        return ruleBuilder
            .MaximumLength(maxLength)
            .WithMessage(ErrorCodes.TooLong);
    }

    internal static IRuleBuilderOptions<T, int> PositiveInteger<T>(this IRuleBuilder<T, int> ruleBuilder,
        bool zeroInclusive)
        => (zeroInclusive
                ? ruleBuilder
                    .GreaterThanOrEqualTo(0)
                : ruleBuilder
                    .GreaterThan(0))
            .WithMessage(ErrorCodes.Invalid);

    internal static IRuleBuilderOptions<T, decimal?> PositiveDecimal<T>(this IRuleBuilder<T, decimal?> ruleBuilder,
        bool zeroInclusive)
        => (zeroInclusive
                ? ruleBuilder
                    .GreaterThanOrEqualTo(0)
                : ruleBuilder
                    .GreaterThan(0))
            .WithMessage(ErrorCodes.Invalid);

    internal static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilder<T, string> ruleBuilder)
        => ruleBuilder
            .Must(value => Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
                           (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            .WithMessage(ErrorCodes.Invalid);
}