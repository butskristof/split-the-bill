using FluentValidation.TestHelper;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.Modules.Groups;

namespace SplitTheBill.Application.UnitTests.Modules.Groups;

internal sealed class DeleteGroupValidatorTests
{
    private readonly DeleteGroup.Validator _sut = new();

    [Test]
    public void EmptyId_Fails()
    {
        var request = new DeleteGroup.Request(Guid.Empty);
        var result = _sut.TestValidate(request);
        result.ShouldHaveValidationErrorFor(r => r.Id).WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void ValidId_Passes()
    {
        var request = new DeleteGroup.Request(Guid.NewGuid());
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
