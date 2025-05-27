using FluentValidation.TestHelper;
using Shouldly;
using SplitTheBill.Application.Common.Configuration;
using SplitTheBill.Application.Tests.Shared.TestData;

namespace SplitTheBill.Application.UnitTests.Common.Configuration;

internal sealed class AuthenticationSettingsValidatorTests
{
    private readonly AuthenticationSettingsValidator _sut = new();

    [Test]
    [MethodDataSource(typeof(TestValues), nameof(TestValues.EmptyStrings))]
    public void Authority_Empty_Fails(string value)
    {
        var settings = new AuthenticationSettings
        {
            Authority = value,
            Audiences = [],
        };

        var result = _sut.TestValidate(settings);
        result.ShouldHaveValidationErrorFor(r => r.Authority);
    }

    [Test]
    [Arguments("ftp://issuer.example.com/")] // Not HTTP/HTTPS
    [Arguments("ftp://issuer.example.com")] // Not HTTP/HTTPS
    [Arguments("issuer.example.com/")] // Not absolute URI
    [Arguments("issuer.example.com")] // Not absolute URI
    [Arguments("https:///issuer.example.com/")] // Malformed
    [Arguments("https:///issuer.example.com")] // Malformed
    [Arguments("://issuer.example.com/")] // Malformed
    [Arguments("://issuer.example.com")] // Malformed
    [Arguments("https://")] // Incomplete
    [Arguments("notauri")]
    [Arguments("not-a-uri")]
    [Arguments("notauri/path")]
    [Arguments("notauri/path/")]
    public void Authority_InvalidIssuer_Fails(string value)
    {
        var settings = new AuthenticationSettings
        {
            Authority = value,
            Audiences = []
        };

        var result = _sut.TestValidate(settings);
        result.ShouldHaveValidationErrorFor(r => r.Authority);
    }

    [Test]
    [Arguments("https://issuer.example.com/")]
    [Arguments("https://issuer.example.com")]
    [Arguments("https://sub.domain.com/path/")]
    [Arguments("https://sub.domain.com/path")]
    [Arguments("https://domain.com/path/")]
    [Arguments("https://domain.com/path")]
    [Arguments("http://issuer.example.com/")]
    [Arguments("http://issuer.example.com")]
    [Arguments("http://sub.domain.com/path/")]
    [Arguments("http://sub.domain.com/path")]
    [Arguments("http://domain.com/path/")]
    [Arguments("http://domain.com/path")]
    public void Authority_ValidIssuer_Passes(string value)
    {
        var settings = new AuthenticationSettings
        {
            Authority = value,
            Audiences = [],
        };

        var result = _sut.TestValidate(settings);
        result.ShouldNotHaveValidationErrorFor(r => r.Authority);
    }

    [Test]
    public void Audiences_Null_Fails()
    {
        var settings = new AuthenticationSettings
        {
            Authority = "https://issuer.example.com/",
            Audiences = null!,
        };

        var result = _sut.TestValidate(settings);
        result.IsValid.ShouldBeFalse();
        result.ShouldHaveValidationErrorFor(r => r.Audiences);
    }

    [Test]
    public void Audiences_Empty_Fails()
    {
        var settings = new AuthenticationSettings
        {
            Authority = "https://issuer.example.com/",
            Audiences = [],
        };

        var result = _sut.TestValidate(settings);
        result.IsValid.ShouldBeFalse();
        result.ShouldHaveValidationErrorFor(r => r.Audiences);
    }

    [Test]
    public void Audiences_WithEmptyValue_Fails()
    {
        var settings = new AuthenticationSettings { Audiences = [string.Empty, "valid"], Authority = string.Empty };
        var result = _sut.TestValidate(settings);
        result
            .ShouldHaveValidationErrorFor("Audiences[0]")
            .WithErrorMessage("Invalid");
    }

    [Test]
    [Arguments("aud1")]
    [Arguments("aud1", "aud2")]
    public void Audiences_WithValues_Passes(params string[] values)
    {
        var settings = new AuthenticationSettings { Audiences = values, Authority = string.Empty };
        var result = _sut.TestValidate(settings);
        result.ShouldNotHaveValidationErrorFor(r => r.Audiences);
    }

    [Test]
    public void ValidValues_Passes()
    {
        var settings = new AuthenticationSettings
        {
            Authority = "https://issuer.example.com/",
            Audiences = ["aud1", "aud2"]
        };

        var result = _sut.TestValidate(settings);
        result.IsValid.ShouldBeTrue();
    }
}