using FluentValidation.TestHelper;
using Shouldly;
using SplitTheBill.Application.Common.Configuration;

namespace SplitTheBill.Application.UnitTests.Common.Configuration;

internal sealed class CorsSettingsValidatorTests
{
    private readonly CorsSettingsValidator _sut = new();

    [Test]
    public void AllowCorsFalse_EmptyOrigins_Passes()
    {
        var settings = new CorsSettings
        {
            AllowCors = false,
            AllowedOrigins = [],
        };
        var result = _sut.TestValidate(settings);
        result.IsValid.ShouldBeTrue();
        result.ShouldNotHaveValidationErrorFor(r => r.AllowedOrigins);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void AllowCorsTrue_EmptyOrigins_Fails()
    {
        var settings = new CorsSettings
        {
            AllowCors = true,
            AllowedOrigins = [],
        };
        var result = _sut.TestValidate(settings);
        result.IsValid.ShouldBeFalse();
        result.ShouldHaveValidationErrorFor(r => r.AllowedOrigins);
    }

    [Test]
    public void AllowCorsTrue_NonEmptyOrigins_Passes()
    {
        var settings = new CorsSettings
        {
            AllowCors = true,
            AllowedOrigins = ["http://localhost:3000"],
        };
        var result = _sut.TestValidate(settings);
        result.IsValid.ShouldBeTrue();
        result.ShouldNotHaveValidationErrorFor(r => r.AllowedOrigins);
        result.Errors
            .ShouldNotContain(e => e.PropertyName.Contains(nameof(settings.AllowedOrigins)));
    }

    [Test]
    public void AllowCorsTrue_AnyInvalidOrigin_Fails()
    {
        var settings = new CorsSettings
        {
            AllowCors = true,
            AllowedOrigins = [
                "http://validorigin:3000",
                "invalidorigin"
            ],
        };
        var result = _sut.TestValidate(settings);
        result.IsValid.ShouldBeFalse();
        result.ShouldHaveValidationErrorFor($"{nameof(settings.AllowedOrigins)}[1]");
    }

    [Test]
    public void InvalidOrigin_MultipleFailures_OnlyReturnsOneErrorCode()
    {
        var settings = new CorsSettings
        {
            AllowCors = true,
            AllowedOrigins = [
                "invalidorigin?q=value",
            ],
        };
        var result = _sut.TestValidate(settings);
        result.Errors.Count.ShouldBe(1);
    }

    [Test]
    [Arguments("*")]
    [Arguments("invalid")]
    [Arguments("invalid/")]
    [Arguments("invalid:1234")]
    [Arguments("invalid:1234/")]
    [Arguments("localhost:1234")]
    [Arguments("localhost:1234/")]
    [Arguments("invalid:port")]
    [Arguments("invalid:port/")]
    [Arguments("http://invalid:port")]
    [Arguments("http://invalid:port/")]
    [Arguments("https://invalid:port")]
    [Arguments("https://invalid:port/")]
    [Arguments("http://localhost:123/somepath")]
    [Arguments("http://localhost:123/somepath/")]
    [Arguments("http://arealdomain.com/")]
    [Arguments("http://arealdomain.com/somepath")]
    [Arguments("https://arealdomain.com/")]
    [Arguments("https://arealdomain.com/somepath")]
    [Arguments("https://arealdomain.com/somepath/")]
    [Arguments("https://sub.arealdomain.com/")]
    [Arguments("https://sub.arealdomain.com/somepath")]
    [Arguments("https://sub.arealdomain.com/somepath/")]
    [Arguments("http://localhost:123/somepath/")]
    [Arguments("https://localhost:123/somepath")]
    [Arguments("https://localhost:123/somepath/")]
    [Arguments("http://*.somedomain.com")]
    [Arguments("https://*.somedomain.com")]
    [Arguments("ws://arealdomain.com")]
    [Arguments("ftp://arealdomain.com")]
    [Arguments("file://arealdomain.com")]
    [Arguments("ws://localhost:3000")]
    [Arguments("ftp://localhost:3000")]
    [Arguments("ftp://user:password@localhost:3000")]
    [Arguments("http://example.com?param=value")]
    [Arguments("http://example.com/?param=value")]
    [Arguments("http://localhost:3000?param=value")]
    [Arguments("http://localhost:3000/?param=value")]
    [Arguments("http://example.com#fragment")]
    [Arguments("http://example.com/#fragment")]
    [Arguments("http://localhost:3000#fragment")]
    [Arguments("http://localhost:3000/#fragment")]
    [Arguments("http://localhost:-1")]
    [Arguments("http://localhost:65536")]
    [Arguments("http://example.com/%20space")]
    [Arguments("relative/path")]
    [Arguments("/relative/path")]
    [Arguments("http://*")]
    public void InvalidOrigin_Fails(string value)
    {
        var settings = new CorsSettings
        {
            AllowCors = true,
            AllowedOrigins = [value],
        };
        var result = _sut.TestValidate(settings);
        result.IsValid.ShouldBeFalse();
        result.ShouldHaveValidationErrorFor($"{nameof(settings.AllowedOrigins)}[0]");
    }

    [Test]
    [Arguments("http://localhost")]
    [Arguments("https://localhost")]
    [Arguments("http://192.168.1.123")]
    [Arguments("https://192.168.1.123")]
    [Arguments("http://192.168.1.123:3000")]
    [Arguments("https://192.168.1.123:3000")]
    [Arguments("http://localhost:3000")]
    [Arguments("https://localhost:3000")]
    [Arguments("http://arealdomain.com")]
    [Arguments("https://arealdomain.com")]
    [Arguments("http://arealdomain.com:3000")]
    [Arguments("https://arealdomain.com:3000")]
    [Arguments("http://subdomain.ofarealdomain.com")]
    [Arguments("https://subdomain.ofarealdomain.com")]
    [Arguments("http://例子.测试")]
    [Arguments("http://xn--fsqu00a.xn--0zwm56d")]
    public void ValidOrigin_Passes(string value)
    {
        var settings = new CorsSettings
        {
            AllowCors = true,
            AllowedOrigins = [value],
        };
        var result = _sut.TestValidate(settings);
        result.IsValid.ShouldBeTrue();
        result.Errors
            .ShouldNotContain(e => e.PropertyName.Contains(nameof(settings.AllowedOrigins)));
        result.ShouldNotHaveAnyValidationErrors();
    }
}