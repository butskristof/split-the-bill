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
        var request = new ExpenseRequestBuilder().WithGroupId(null).BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.GroupId).WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptyGroupId_Fails()
    {
        var request = new ExpenseRequestBuilder().WithGroupId(Guid.Empty).BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.GroupId).WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void EmptyNullableGuid_OnlyReturnsOneErrorCode()
    {
        var request = new ExpenseRequestBuilder().WithGroupId(Guid.Empty).BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.GroupId).Count().ShouldBe(1);
    }

    [Test]
    public void NonEmptyGroupId_Passes()
    {
        var request = new ExpenseRequestBuilder().WithGroupId(Guid.NewGuid()).BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.GroupId);
    }

    #endregion

    #region Description

    [Test]
    [MethodDataSource(typeof(TestValues), nameof(TestValues.EmptyStrings))]
    public void NullOrEmptyDescription_Fails(string? name)
    {
        var request = new ExpenseRequestBuilder().WithDescription(name).BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.Description)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void EmptyDescription_OnlyReturnsOneErrorCode()
    {
        var request = new ExpenseRequestBuilder().WithDescription(null).BuildCreateRequest();

        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Description).Count().ShouldBe(1);
    }

    [Test]
    public void DescriptionTooLong_Fails()
    {
        var description = TestUtilities.GenerateString(513);
        var request = new ExpenseRequestBuilder().WithDescription(description).BuildCreateRequest();
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
        var request = new ExpenseRequestBuilder().WithDescription(description).BuildCreateRequest();

        var result = _sut.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Description);
    }

    #endregion

    #region PaidByMemberId

    [Test]
    public void NullPaidByMemberId_Fails()
    {
        var request = new ExpenseRequestBuilder().WithPaidByMemberId(null).BuildCreateRequest();
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
            .BuildCreateRequest();
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
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.PaidByMemberId);
    }

    #endregion

    #region Timestamp

    [Test]
    public void NullTimestamp_Fails()
    {
        var request = new ExpenseRequestBuilder().WithTimestamp(null).BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Timestamp).WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void ValidTimestamp_Passes()
    {
        var request = new ExpenseRequestBuilder()
            .WithTimestamp(new DateTimeOffset(2025, 04, 03, 01, 14, 21, TimeSpan.Zero))
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Timestamp);
    }

    #endregion

    #region Amount

    [Test]
    public void NullAmount_Fails()
    {
        var request = new ExpenseRequestBuilder().WithAmount(null).BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Amount).WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public void NegativeOrZeroAmount_Fails(decimal amount)
    {
        var request = new ExpenseRequestBuilder().WithAmount(amount).BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Amount).WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void PositiveAmount_Passes()
    {
        var request = new ExpenseRequestBuilder().WithAmount(1.0m).BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.Amount);
    }

    #endregion

    #region SplitType

    [Test]
    public void NullSplitType_Fails()
    {
        var request = new ExpenseRequestBuilder().WithSplitType(null).BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.SplitType).WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void InvalidSplitType_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType((ExpenseSplitType)100) // Invalid enum value
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.SplitType).WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    [Arguments(ExpenseSplitType.Evenly)]
    [Arguments(ExpenseSplitType.Percentual)]
    [Arguments(ExpenseSplitType.ExactAmount)]
    public void ValidSplitType_Passes(ExpenseSplitType splitType)
    {
        var request = new ExpenseRequestBuilder().WithSplitType(splitType).BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.SplitType);
    }

    #endregion

    #region Participants

    [Test]
    public void EmptyParticipants_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithParticipants(new List<CreateExpense.Request.Participant>())
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(r => r.Participants)
            .WithErrorMessage(ErrorCodes.Required);
    }

    [Test]
    public void NullParticipant_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithParticipants(new List<CreateExpense.Request.Participant> { null! })
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor($"{nameof(request.Participants)}[0]")
            .WithErrorMessage(ErrorCodes.Invalid);
    }

    [Test]
    public void DuplicateParticipants_Fails()
    {
        var id = Guid.NewGuid();
        var participant1 = new ExpenseRequestBuilder.ParticipantBuilder()
            .WithMemberId(id)
            .BuildCreateParticipant();
        var participant2 = new ExpenseRequestBuilder.ParticipantBuilder()
            .WithMemberId(id)
            .BuildCreateParticipant();

        var request = new ExpenseRequestBuilder()
            .WithParticipants([participant1, participant2])
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(nameof(request.Participants))
            .WithErrorMessage(ErrorCodes.NotUnique);
    }

    #region MemberId

    [Test]
    public void ParticipantWithNullMemberId_Fails()
    {
        var participant = new ExpenseRequestBuilder.ParticipantBuilder()
            .WithMemberId(null)
            .BuildCreateParticipant();

        var request = new ExpenseRequestBuilder()
            .WithParticipants([participant])
            .BuildCreateRequest();
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
        var participant = new ExpenseRequestBuilder.ParticipantBuilder()
            .WithMemberId(Guid.Empty)
            .BuildCreateParticipant();

        var request = new ExpenseRequestBuilder()
            .WithParticipants([participant])
            .BuildCreateRequest();
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
        var participant = new ExpenseRequestBuilder.ParticipantBuilder()
            .WithMemberId(Guid.NewGuid())
            .BuildCreateParticipant();

        var request = new ExpenseRequestBuilder()
            .WithParticipants([participant])
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(
            $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.MemberId)}"
        );
    }

    #endregion

    #region SplitTypeEvenly

    [Test]
    public void EvenlySplitWithoutShareValues_Passes()
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.Evenly)
            .WithParticipants(
                new List<CreateExpense.Request.Participant>
                {
                    new ExpenseRequestBuilder.ParticipantBuilder()
                        .WithPercentualShare(null)
                        .WithExactShare(null),
                }
            )
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(
            $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.PercentualShare)}"
        );
        result.ShouldNotHaveValidationErrorFor(
            $"{nameof(request.Participants)}[0].{nameof(CreateExpense.Request.Participant.ExactShare)}"
        );
    }

    #endregion

    #region SplitTypePercentual

    [Test]
    public void PercentualSplitWithNoPercentualShare_Fails()
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants(
                new List<CreateExpense.Request.Participant>
                {
                    new ExpenseRequestBuilder.ParticipantBuilder().WithPercentualShare(null),
                }
            )
            .BuildCreateRequest();
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
        var request = new ExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants(
                new List<CreateExpense.Request.Participant>
                {
                    new ExpenseRequestBuilder.ParticipantBuilder().WithPercentualShare(-1),
                }
            )
            .BuildCreateRequest();
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
        var request = new ExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants(
                percentageShares
                    .Select(p =>
                        new ExpenseRequestBuilder.ParticipantBuilder()
                            .WithPercentualShare(p)
                            .BuildCreateParticipant()
                    )
                    .ToList()
            )
            .BuildCreateRequest();
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
            .WithParticipants(
                new List<CreateExpense.Request.Participant>
                {
                    new ExpenseRequestBuilder.ParticipantBuilder().WithPercentualShare(10),
                }
            )
            .BuildCreateRequest();
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
    public void PercentualSplitWithPercentualShareSumNotEqualTo100_Fails(
        int percentage1,
        int percentage2
    )
    {
        var request = new ExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants(
                new List<CreateExpense.Request.Participant>
                {
                    new ExpenseRequestBuilder.ParticipantBuilder().WithPercentualShare(percentage1),
                    new ExpenseRequestBuilder.ParticipantBuilder().WithPercentualShare(percentage2),
                }
            )
            .BuildCreateRequest();
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
            .WithParticipants(
                new List<CreateExpense.Request.Participant>
                {
                    new ExpenseRequestBuilder.ParticipantBuilder().WithExactShare(null),
                }
            )
            .BuildCreateRequest();
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
        var request = new ExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.ExactAmount)
            .WithParticipants(
                new List<CreateExpense.Request.Participant>
                {
                    new ExpenseRequestBuilder.ParticipantBuilder().WithExactShare(-1),
                }
            )
            .BuildCreateRequest();
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
        var request = new ExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.ExactAmount)
            .WithAmount(amount)
            .WithParticipants(
                exactShares
                    .Select(v =>
                        new ExpenseRequestBuilder.ParticipantBuilder()
                            .WithExactShare(v)
                            .BuildCreateParticipant()
                    )
                    .ToList()
            )
            .BuildCreateRequest();
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
            .WithParticipants(
                new List<CreateExpense.Request.Participant>
                {
                    new ExpenseRequestBuilder.ParticipantBuilder().WithExactShare(10),
                }
            )
            .BuildCreateRequest();
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
    public void ExactAmountSplitWithExactShareSumNotEqualToAmount_Fails(
        decimal exactShare1,
        decimal exactShare2
    )
    {
        var request = new ExpenseRequestBuilder()
            .WithAmount(1000)
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants(
                new List<CreateExpense.Request.Participant>
                {
                    new ExpenseRequestBuilder.ParticipantBuilder().WithExactShare(exactShare1),
                    new ExpenseRequestBuilder.ParticipantBuilder().WithExactShare(exactShare2),
                }
            )
            .BuildCreateRequest();
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
            ExactShare = 10.5m,
        };

        var participant2 = new CreateExpense.Request.Participant
        {
            MemberId = null,
            ExactShare = -1,
        };

        var request = new ExpenseRequestBuilder()
            .WithSplitType(ExpenseSplitType.ExactAmount)
            .WithParticipants([participant1, participant2])
            .BuildCreateRequest();
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

    [Test]
    public void ValidRequest_Passes()
    {
        var request = new ExpenseRequestBuilder()
            .WithGroupId(Guid.NewGuid())
            .WithDescription("Expense description")
            .WithPaidByMemberId(Guid.NewGuid())
            .WithTimestamp(new DateTimeOffset(2025, 04, 03, 01, 28, 11, TimeSpan.Zero))
            .WithAmount(200m)
            .WithSplitType(ExpenseSplitType.Percentual)
            .WithParticipants(
                new List<CreateExpense.Request.Participant>
                {
                    new ExpenseRequestBuilder.ParticipantBuilder()
                        .WithMemberId(Guid.NewGuid())
                        .WithPercentualShare(100),
                }
            )
            .BuildCreateRequest();
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
