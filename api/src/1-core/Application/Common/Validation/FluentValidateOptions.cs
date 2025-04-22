using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SplitTheBill.Application.Common.Validation;

internal sealed class FluentValidateOptions<TOptions>
    : IValidateOptions<TOptions>
    where TOptions : class
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string? _name;

    public FluentValidateOptions(IServiceProvider serviceProvider, string? name)
    {
        _serviceProvider = serviceProvider;
        _name = name;
    }
    
    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        if (_name is not null && _name != name)
            return ValidateOptionsResult.Skip;

        ArgumentNullException.ThrowIfNull(options);

        var type = options.GetType().Name;

        using var scope = _serviceProvider.CreateScope();
        var validators = scope
            .ServiceProvider
            .GetServices<IValidator<TOptions>>()
            .ToList();
        if (validators.Count == 0)
            return ValidateOptionsResult.Fail($"No validator found for options of type {type}");

        var results = validators
            .Select(v => v.Validate(options))
            .ToList();
        if (results.All(r => r.IsValid))
            return ValidateOptionsResult.Success;

        var errors = results
            .SelectMany(r =>
                r.Errors
                    .Select(e =>
                        $"Validation failed for {type}.{e.PropertyName}: {e.ErrorMessage}"
                    )
            );
        return ValidateOptionsResult.Fail(errors);
    }
}