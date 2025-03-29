using FluentValidation.TestHelper;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.Modules.Groups.Expenses;
using SplitTheBill.Application.Tests.Shared.Builders;
using SplitTheBill.Application.Tests.Shared.TestData;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.UnitTests.Modules.Groups.Expenses;

internal sealed class UpdateExpenseValidatorTests
{
    private readonly UpdateExpense.Validator _sut = new();

    #region GroupId

    [Test]
    public void NullOrEmptyGroupId_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithGroupId(null)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.GroupId)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptyGroupId_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithGroupId(Guid.Empty)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.GroupId)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void NonEmptyGroupId_Passes()
    {
        var request = new ExpenseRequestBuilder()
            .WithGroupId(Guid.NewGuid())
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.GroupId);
    }

    #endregion

    #region ExpenseId

    [Test]
    public void NullExpenseId_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithExpenseId(null)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.ExpenseId)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptyExpenseId_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithExpenseId(Guid.Empty)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.ExpenseId)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void NonEmptyExpenseId_Passes()
    {
        var request = new ExpenseRequestBuilder()
            .WithExpenseId(Guid.NewGuid())
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.ExpenseId);
    }

    #endregion

    #region Description

    [Test]
    [MethodDataSource(typeof(TestValues), nameof(TestValues.EmptyStrings))]
    public void NullOrEmptyDescription_Fails(string? name)
    {
        var request = new ExpenseRequestBuilder()
            .WithDescription(name)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.Description)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptyDescription_OnlyReturnsOneErrorCode()
    {
        var request = new ExpenseRequestBuilder()
            .WithDescription(null)
            .BuildUpdateRequest();

        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.Description)
            .Count().ShouldBe(1);
    }

    [Test]
    public void DescriptionTooLong_Fails()
    {
        var description = TestUtilities.GenerateString(513);
        var request = new ExpenseRequestBuilder()
            .WithDescription(description)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.Description)
            .WithErrorMessage(ErrorCodes.TooLong);
    }

    [Test]
    [Arguments(1)]
    [Arguments(15)]
    [Arguments(512)]
    public void DescriptionValidLength_Passes(int length)
    {
        var description = TestUtilities.GenerateString(length);
        var request = new ExpenseRequestBuilder()
            .WithDescription(description)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Description);
    }

    #endregion

    #region PaidByMemberId

    [Test]
    public void NullPaidByMemberId_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithPaidByMemberId(null)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.PaidByMemberId)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptyPaidByMemberId_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithPaidByMemberId(Guid.Empty)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.PaidByMemberId)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void NonEmptyPaidByMemberId_Passes()
    {
        var request = new ExpenseRequestBuilder()
            .WithPaidByMemberId(Guid.NewGuid())
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.PaidByMemberId);
    }

    #endregion

    #region Amount

    [Test]
    public void NullAmount_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithAmount(null)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.Amount)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public void NegativeOrZeroAmount_Fails(decimal amount)
    {
        var request = new ExpenseRequestBuilder()
            .WithAmount(amount)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.Amount)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void PositiveAmount_Passes()
    {
        var request = new ExpenseRequestBuilder()
            .WithAmount(1.0m)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.Amount);
    }

    #endregion

    #region SplitType

    [Test]
    public void NullSplitType_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType(null)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.SplitType)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void InvalidSplitType_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType((ExpenseSplitType)100) // Invalid enum value
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.SplitType)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    [Arguments(ExpenseSplitType.Evenly)]
    [Arguments(ExpenseSplitType.Percentual)]
    [Arguments(ExpenseSplitType.ExactAmount)]
    public void ValidSplitType_Passes(ExpenseSplitType splitType)
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType(splitType)
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.SplitType);
    }

    #endregion

    #region Participants

    [Test]
    public void EmptyParticipants_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithParticipants([])
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.Participants)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void NullParticipant_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithParticipants([null!])
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor($"{nameof(request.Participants)}[0]")
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    #region MemberId

    [Test]
    public void ParticipantWithNullMemberId_Fails()
    {
        var participant = new ExpenseRequestBuilder.ParticipantBuilder()
            .WithMemberId(null)
            .Build();

        var request = new ExpenseRequestBuilder()
            .WithParticipants([participant])
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(UpdateExpense.Request.Participant.MemberId)}"
            )
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void ParticipantWithEmptyMemberId_Fails()
    {
        var participant = new ExpenseRequestBuilder.ParticipantBuilder()
            .WithMemberId(Guid.Empty)
            .Build();

        var request = new ExpenseRequestBuilder()
            .WithParticipants([participant])
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(UpdateExpense.Request.Participant.MemberId)}"
            )
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void ParticipantWithValidMemberId_Passes()
    {
        var participant = new ExpenseRequestBuilder.ParticipantBuilder()
            .WithMemberId(Guid.NewGuid())
            .Build();

        var request = new ExpenseRequestBuilder()
            .WithParticipants([participant])
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(UpdateExpense.Request.Participant.MemberId)}"
            );
    }

    #endregion

    #region SplitTypeEvenly

    [Test]
    public void EvenlySplitWithoutShareValues_Passes()
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.Evenly)
            .WithParticipants([
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithPercentualShare(null)
                    .WithExactShare(null)
            ])
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(UpdateExpense.Request.Participant.PercentualShare)}"
            );
        result
            .ShouldNotHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(UpdateExpense.Request.Participant.ExactShare)}"
            );
    }

    #endregion

    #region SplitTypePercentual

    [Test]
    public void PercentualSplitWithNoPercentualShare_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants([
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithPercentualShare(null)
            ])
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(UpdateExpense.Request.Participant.PercentualShare)}"
            )
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void PercentualSplitWithNegativePercentualShare_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants([
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithPercentualShare(-1)
            ])
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(UpdateExpense.Request.Participant.PercentualShare)}"
            )
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    public static IEnumerable<Func<int[]>> GetPercentageShares()
    {
        yield return () => [0, 100];
        yield return () => [50, 50];
        yield return () => [25, 25, 50];
        yield return () => [10, 20, 30, 40];
    }

    [Test]
    [MethodDataSource(nameof(GetPercentageShares))]
    public void PercentualSplitWithValidPercentualShare_Passes(int[] percentageShares)
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants(percentageShares
                .Select(p => new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithPercentualShare(p)
                    .Build()
                )
                .ToList()
            )
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(nameof(request.Participants));
    }

    [Test]
    [Arguments(ExpenseSplitType.Evenly)]
    [Arguments(ExpenseSplitType.ExactAmount)]
    public void NotPercentualSplitWithPercentualShare_Fails(ExpenseSplitType splitType)
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType(splitType)
            .WithParticipants([
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithPercentualShare(10)
            ])
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(UpdateExpense.Request.Participant.PercentualShare)}"
            )
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    [Arguments(50, 49)]
    [Arguments(50, 51)]
    public void PercentualSplitWithPercentualShareSumNotEqualTo100_Fails(int percentage1, int percentage2)
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants([
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithPercentualShare(percentage1),
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithPercentualShare(percentage2)
            ])
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(nameof(request.Participants))
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    #endregion

    #region SplitTypeExactAmount

    [Test]
    public void ExactAmountSplitWithNoExactShare_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.ExactAmount)
            .WithParticipants([
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithExactShare(null)
            ])
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(UpdateExpense.Request.Participant.ExactShare)}"
            )
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void ExactAmountSplitWithNegativeExactShare_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.ExactAmount)
            .WithParticipants([
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithExactShare(-1)
            ])
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(UpdateExpense.Request.Participant.ExactShare)}"
            )
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    public static IEnumerable<Func<(decimal, decimal[])>> GetExactShares()
    {
        yield return () => (1000, [0, 1000]);
        yield return () => (1000, [500, 500]);
        yield return () => (1000, [250, 250, 500]);
        yield return () => (1000, [100, 200, 300, 400]);
    }

    [Test]
    [MethodDataSource(nameof(GetExactShares))]
    public void ExactAmountSplitWithValidExactShare_Passes(decimal amount, decimal[] exactShares)
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.ExactAmount)
            .WithAmount(amount)
            .WithParticipants(exactShares
                .Select(v => new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithExactShare(v)
                    .Build()
                )
                .ToList()
            )
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(nameof(request.Participants));
    }

    [Test]
    [Arguments(ExpenseSplitType.Evenly)]
    [Arguments(ExpenseSplitType.Percentual)]
    public void NotExactAmountSplitWithExactShare_Fails(ExpenseSplitType splitType)
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType(splitType)
            .WithParticipants([
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithExactShare(10)
            ])
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(UpdateExpense.Request.Participant.ExactShare)}"
            )
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    [Arguments(998, 1)]
    [Arguments(999, 2)]
    public void ExactAmountSplitWithExactShareSumNotEqualToAmount_Fails(decimal exactShare1, decimal exactShare2)
    {
        var request = new ExpenseRequestBuilder()
            .WithAmount(1000)
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants([
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithExactShare(exactShare1),
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithExactShare(exactShare2)
            ])
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(nameof(request.Participants))
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    #endregion

    #endregion

    [Test]
    public void ValidRequest_Passes()
    {
        var request = new ExpenseRequestBuilder()
            .WithGroupId(Guid.NewGuid())
            .WithExpenseId(Guid.NewGuid())
            .WithDescription("Expense description")
            .WithAmount(200m)
            .WithPaidByMemberId(Guid.NewGuid())
            .WithSplitType(ExpenseSplitType.Evenly)
            .WithParticipants([
                new ExpenseRequestBuilder.ParticipantBuilder()
                    .WithMemberId(Guid.NewGuid())
                    .WithPercentualShare(50)
            ])
            .BuildUpdateRequest();
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}