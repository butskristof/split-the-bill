using FluentValidation.TestHelper;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.Modules.Members;

namespace SplitTheBill.Application.UnitTests.Modules.Members;
internal sealed class DeleteMemberValidatorTests
{
    private readonly DeleteMember.Validator _sut = new();

    [Test]
    public void EmptyId_Fails()
    {
        var request = new DeleteMember.Request(Guid.Empty);
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.Id)
            .WithErrorMessage(ErrorCodes.Invalid);
    }
    
    [Test]
    public void NonEmptyId_Passes()
    {
        var request = new DeleteMember.Request(Guid.NewGuid());
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.Id);
    }
}
