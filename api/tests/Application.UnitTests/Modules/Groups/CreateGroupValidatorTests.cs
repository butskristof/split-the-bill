using FluentValidation.TestHelper;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Tests.Shared.Builders;
using SplitTheBill.Application.Tests.Shared.TestData;

namespace SplitTheBill.Application.UnitTests.Modules.Groups;

internal sealed class CreateGroupValidatorTests
{
    private readonly CreateGroup.Validator _sut = new();

    #region Name

    [Test]
    [MethodDataSource(typeof(TestValues), nameof(TestValues.EmptyStrings))]
    public void NullOrEmptyName_Fails(string? name)
    {
        var request = new GroupRequestBuilder().WithName(name).BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Name).WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptyName_OnlyReturnsOneErrorCode()
    {
        var request = new GroupRequestBuilder().WithName(null).BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Name).Count().ShouldBe(1);
    }

    [Test]
    public void NameTooLong_Fails()
    {
        var name = TestUtilities.GenerateString(513);
        var request = new GroupRequestBuilder().WithName(name).BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Name).WithErrorMessage(ErrorCodes.TooLong);
    }

    [Test]
    [Arguments(1)]
    [Arguments(15)]
    [Arguments(512)]
    public void NameValidLength_Passes(int length)
    {
        var name = TestUtilities.GenerateString(length);
        var request = new GroupRequestBuilder().WithName(name).BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Name);
    }

    #endregion

    #region Request

    [Test]
    public void ValidRequest_Passes()
    {
        var request = new GroupRequestBuilder().WithName("group name").BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion
}
