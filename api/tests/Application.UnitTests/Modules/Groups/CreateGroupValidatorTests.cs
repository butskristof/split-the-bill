using FluentValidation.TestHelper;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.Modules.Groups;
using SplitTheBill.Application.Tests.Shared.Builders;
using SplitTheBill.Application.Tests.Shared.TestData;

namespace SplitTheBill.Application.UnitTests.Modules.Groups;

internal sealed class CreateGroupValidatorTests
{
    private readonly CreateGroup.Validator _sut = new();
    
    [Test]
    [MethodDataSource(typeof(TestValues), nameof(TestValues.EmptyStrings))]
    public void NullOrEmptyName_Fails(string? name)
    {
        var request = new CreateGroupRequestBuilder()
            .WithName(name)
            .Build();
        var result = _sut.TestValidate(request);
        
        result
            .ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage(ErrorCodes.Required);
    }
    
    [Test]
    public void ValidName_Passes()
    {
        var request = new CreateGroupRequestBuilder()
            .WithName("group name")
            .Build();
        var result = _sut.TestValidate(request);
        
        result
            .ShouldNotHaveValidationErrorFor(r => r.Name);
    }

    [Test]
    public void ValidRequest_Passes()
    {
        var request = new CreateGroupRequestBuilder()
            .WithName("group name")
            .Build();
        var result = _sut.TestValidate(request);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
}