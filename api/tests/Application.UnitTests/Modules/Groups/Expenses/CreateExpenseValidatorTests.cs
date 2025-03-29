using FluentValidation.TestHelper;
using Shouldly;
using SplitTheBill.Application.Common.Validation;
using SplitTheBill.Application.Modules.Groups.Expenses;
using SplitTheBill.Application.Tests.Shared.Builders;
using SplitTheBill.Application.Tests.Shared.TestData;
using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.UnitTests.Modules.Groups.Expenses;

internal sealed class CreateExpenseValidatorTests
{
    private readonly CreateExpense.Validator _sut = new();

    #region GroupId

    [Test]
    public void NullGroupId_Fails()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithGroupId(null)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.GroupId)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptyGroupId_Fails()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithGroupId(Guid.Empty)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.GroupId)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void EmptyNullableGuid_OnlyReturnsOneErrorCode()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithGroupId(Guid.Empty)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.GroupId)
            .Count().ShouldBe(1);
    }

    [Test]
    public void NonEmptyGroupId_Passes()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithGroupId(Guid.NewGuid())
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.GroupId);
    }

    #endregion

    #region Description

    [Test]
    [MethodDataSource(typeof(TestValues), nameof(TestValues.EmptyStrings))]
    public void NullOrEmptyDescription_Fails(string? name)
    {
        var request = new CreateExpenseRequestBuilder()
            .WithDescription(name)
            .Build();

        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.Description)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptyDescription_OnlyReturnsOneErrorCode()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithDescription(null)
            .Build();

        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.Description)
            .Count().ShouldBe(1);
    }

    [Test]
    public void DescriptionTooLong_Fails()
    {
        var name = TestUtilities.GenerateString(513);
        var request = new CreateExpenseRequestBuilder()
            .WithDescription(name)
            .Build();
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
        var name = TestUtilities.GenerateString(length);
        var request = new CreateExpenseRequestBuilder()
            .WithDescription(name)
            .Build();

        var result = _sut.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Description);
    }

    #endregion

    #region PaidByMemberId

    [Test]
    public void NullPaidByMemberId_Fails()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithPaidByMemberId(null)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.PaidByMemberId)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptyPaidByMemberId_Fails()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithPaidByMemberId(Guid.Empty)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.PaidByMemberId)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void NonEmptyPaidByMemberId_Passes()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithPaidByMemberId(Guid.NewGuid())
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.PaidByMemberId);
    }

    #endregion

    #region Amount

    [Test]
    public void NullAmount_Fails()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithAmount(null)
            .Build();
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
        var request = new CreateExpenseRequestBuilder()
            .WithAmount(amount)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.Amount)
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void PositiveAmount_Passes()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithAmount(1.0m)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.Amount);
    }

    #endregion

    #region SplitType

    [Test]
    public void NullSplitType_Fails()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithSplitType(null)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.SplitType)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void InvalidSplitType_Fails()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithSplitType((ExpenseSplitType)100) // Invalid enum value
            .Build();
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
        var request = new CreateExpenseRequestBuilder()
            .WithSplitType(splitType)
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(r => r.SplitType);
    }

    #endregion

    #region Participants

    [Test]
    public void EmptyParticipants_Fails()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithParticipants([])
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.Participants)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void NullParticipant_Fails()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithParticipants([null!])
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor($"{nameof(request.Participants)}[0]")
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    #region MemberId

    [Test]
    public void ParticipantWithNullMemberId_Fails()
    {
        var participant = new CreateExpenseRequestBuilder.ParticipantBuilder()
            .WithMemberId(null)
            .Build();

        var request = new CreateExpenseRequestBuilder()
            .WithParticipants([participant])
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.MemberId)}"
            )
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void ParticipantWithEmptyMemberId_Fails()
    {
        var participant = new CreateExpenseRequestBuilder.ParticipantBuilder()
            .WithMemberId(Guid.Empty)
            .Build();

        var request = new CreateExpenseRequestBuilder()
            .WithParticipants([participant])
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.MemberId)}"
            )
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void ParticipantWithValidMemberId_Passes()
    {
        var participant = new CreateExpenseRequestBuilder.ParticipantBuilder()
            .WithMemberId(Guid.NewGuid())
            .Build();

        var request = new CreateExpenseRequestBuilder()
            .WithParticipants([participant])
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.MemberId)}"
            );
    }

    #endregion

    #region SplitTypeEvenly

    [Test]
    public void EvenlySplitWithoutShareValues_Passes()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.Evenly)
            .WithParticipants([
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithPercentualShare(null)
                    .WithExactShare(null)
            ])
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldNotHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.PercentualShare)}"
            );
        result
            .ShouldNotHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.ExactShare)}"
            );
    }

    #endregion

    #region SplitTypePercentual

    [Test]
    public void PercentualSplitWithNoPercentualShare_Fails()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants([
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithPercentualShare(null)
            ])
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.PercentualShare)}"
            )
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void PercentualSplitWithNegativePercentualShare_Fails()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants([
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithPercentualShare(-1)
            ])
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.PercentualShare)}"
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
        var request = new CreateExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants(percentageShares
                .Select(p => new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithPercentualShare(p)
                    .Build()
                )
                .ToList()
            )
            .Build();
        var result = _sut.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(nameof(request.Participants));
    }

    [Test]
    [Arguments(ExpenseSplitType.Evenly)]
    [Arguments(ExpenseSplitType.ExactAmount)]
    public void NotPercentualSplitWithPercentualShare_Fails(ExpenseSplitType splitType)
    {
        var request = new CreateExpenseRequestBuilder()
            .WithSplitType(splitType)
            .WithParticipants([
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithPercentualShare(10)
            ])
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.PercentualShare)}"
            )
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    [Arguments(50, 49)]
    [Arguments(50, 51)]
    public void PercentualSplitWithPercentualShareSumNotEqualTo100_Fails(int percentage1, int percentage2)
    {
        var request = new CreateExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants([
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithPercentualShare(percentage1),
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithPercentualShare(percentage2)
            ])
            .Build();
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
        var request = new CreateExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.ExactAmount)
            .WithParticipants([
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithExactShare(null)
            ])
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.ExactShare)}"
            )
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void ExactAmountSplitWithNegativeExactShare_Fails()
    {
        var request = new CreateExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.ExactAmount)
            .WithParticipants([
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithExactShare(-1)
            ])
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.ExactShare)}"
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
        var request = new CreateExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.ExactAmount)
            .WithAmount(amount)
            .WithParticipants(exactShares
                .Select(v => new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithExactShare(v)
                    .Build()
                )
                .ToList()
            )
            .Build();
        var result = _sut.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(nameof(request.Participants));
    }

    [Test]
    [Arguments(ExpenseSplitType.Evenly)]
    [Arguments(ExpenseSplitType.Percentual)]
    public void NotExactAmountSplitWithExactShare_Fails(ExpenseSplitType splitType)
    {
        var request = new CreateExpenseRequestBuilder()
            .WithSplitType(splitType)
            .WithParticipants([
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithExactShare(10)
            ])
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.ExactShare)}"
            )
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    [Arguments(998, 1)]
    [Arguments(999, 2)]
    public void ExactAmountSplitWithExactShareSumNotEqualToAmount_Fails(decimal exactShare1, decimal exactShare2)
    {
        var request = new CreateExpenseRequestBuilder()
            .WithAmount(1000)
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants([
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithExactShare(exactShare1),
                new CreateExpenseRequestBuilder.ParticipantBuilder()
                    .WithExactShare(exactShare2)
            ])
            .Build();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(nameof(request.Participants))
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    #endregion

    [Test]
    public void MultipleParticipants_ValidationAppliedToEach()
    {
        var participant1 = new CreateExpense.Request.Participant
        {
            MemberId = Guid.NewGuid(),
            ExactShare = 10.5m
        };

        var participant2 = new CreateExpense.Request.Participant
        {
            MemberId = null,
            ExactShare = -1,
        };

        var request = new CreateExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.ExactAmount)
            .WithParticipants([participant1, participant2])
            .Build();
        var result = _sut.TestValidate(request);

        // First participant should be valid
        result.ShouldNotHaveValidationErrorFor(
            $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.MemberId)}"
        );
        result.ShouldNotHaveValidationErrorFor(
            $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.ExactShare)}"
        );

        // Second participant should have errors
        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[1].{nameof(CreateExpense.Request.Participant.MemberId)}"
            )
            .WithErrorMessage(ErrorCodes.Required);

        result
            .ShouldHaveValidationErrorFor(
                $"{nameof(request.Participants)}[1].{nameof(CreateExpense.Request.Participant.ExactShare)}"
            )
            .WithErrorMessage(ErrorCodes.Invalid);

        result
            .ShouldHaveValidationErrorFor(nameof(request.Participants))
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    #endregion
}