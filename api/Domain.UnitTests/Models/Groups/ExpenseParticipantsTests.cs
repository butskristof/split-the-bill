using Shouldly;
using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.UnitTests.Builders;

namespace SplitTheBill.Domain.UnitTests.Models.Groups;

internal sealed class ExpenseParticipantsTests
{
    [Test]
    public void NewExpense_ZeroAmountSplitEvenly()
    {
        var expense = ExpenseBuilder.Create();
        expense.Amount.ShouldBe(0);
        expense.SplitType.ShouldBe(ExpenseSplitType.Evenly);
        expense.Participants.ShouldBeEmpty();
    }

    #region Amount

    [Test]
    public void SetAmount_NotExactAmountSplit_SetsValue()
    {
        true.ShouldBeFalse();
    }

    [Test]
    public void SetAmount_ExactAmountSplit_DoesNotMatchSumOfParticipantAmounts_Throws()
    {
        true.ShouldBeFalse();
    }

    [Test]
    public void SetAmount_ExactAmountSplit_SetsValueIfAmountMatchesParticipants()
    {
        true.ShouldBeFalse();
    }

    #endregion

    #region Evenly

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public void SetAmountAndParticipantsWithEvenSplit_NegativeOrZeroAmount_Throws(decimal amount)
    {
        var expense = ExpenseBuilder.Create();
        Should.Throw<ArgumentException>(() =>
            expense.SetAmountAndParticipantsWithEvenSplit(amount, new HashSet<Guid>([Guid.NewGuid()]))
        );
    }

    [Test]
    public void SetAmountAndParticipantsWithEvenSplit_EmptyParticipants_Throws()
    {
        var expense = ExpenseBuilder.Create();
        Should.Throw<ArgumentException>(() =>
                expense.SetAmountAndParticipantsWithEvenSplit(1, new HashSet<Guid>())
            )
            .ParamName.ShouldBe("participants");
    }

    [Test]
    public void SetAmountAndParticipantsWithEvenSplit_SetsSplitTypeAndCreatesParticipants()
    {
        var expense = ExpenseBuilder.Create();
        const decimal amount = 200.0m;
        HashSet<Guid> memberIds = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

        expense.SetAmountAndParticipantsWithEvenSplit(amount, memberIds);

        expense.SplitType.ShouldBe(ExpenseSplitType.Evenly);
        expense.Amount.ShouldBe(amount);
        expense.Participants.Count.ShouldBe(3);
        expense.Participants
            .ShouldAllBe(p =>
                !p.PercentualShare.HasValue &&
                !p.ExactShare.HasValue
            );

        memberIds.ShouldAllBe(m => expense.Participants.Any(p => p.MemberId == m));
    }

    [Test]
    public void SetAmountAndParticipantsWithEvenSplit_ClearsExistingPercentualValues()
    {
        var expense = ExpenseBuilder.Create();
        const decimal amount = 200.0m;
        HashSet<Guid> memberIds = [Guid.NewGuid(), Guid.NewGuid()];
        expense.SetAmountAndParticipantsWithPercentualSplit(
            amount,
            memberIds.ToDictionary(id => id, _ => 50)
        );

        expense.SetAmountAndParticipantsWithEvenSplit(amount, memberIds);

        expense.Participants
            .ShouldAllBe(p =>
                !p.PercentualShare.HasValue &&
                !p.ExactShare.HasValue
            );
    }

    [Test]
    public void SetAmountAndParticipantsWithEvenSplit_ClearsExistingExactAmountValues()
    {
        var expense = ExpenseBuilder.Create();
        const decimal amount = 200.0m;
        HashSet<Guid> memberIds = [Guid.NewGuid(), Guid.NewGuid()];
        expense.SetAmountAndParticipantsWithExactSplit(
            amount,
            memberIds.ToDictionary(id => id, _ => 100m)
        );

        expense.SetAmountAndParticipantsWithEvenSplit(amount, memberIds);

        expense.Participants
            .ShouldAllBe(p =>
                !p.PercentualShare.HasValue &&
                !p.ExactShare.HasValue
            );
    }

    #endregion

    #region Percentual

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public void SetAmountAndParticipantsWithPercentualSplit_NegativeOrZeroAmount_Throws(decimal amount)
    {
        var expense = ExpenseBuilder.Create();
        Should.Throw<ArgumentException>(() =>
            expense.SetAmountAndParticipantsWithPercentualSplit(
                amount,
                new Dictionary<Guid, int> { { Guid.NewGuid(), 100 } }
            )
        );
    }

    [Test]
    public void SetAmountAndParticipantsWithPercentualSplit_EmptyParticipants_Throws()
    {
        var expense = ExpenseBuilder.Create();
        Should.Throw<ArgumentException>(() =>
                expense.SetAmountAndParticipantsWithPercentualSplit(100, new Dictionary<Guid, int>())
            )
            .ParamName.ShouldBe("participants");
    }

    public static IEnumerable<Func<int[]>> GetFailingPercentages() =>
    [
        () => [10, 10],
        () => [60, 60],
        () => [10, 20, 30],
        () => [33, 33, 33],
        () => [99],
        () => [101],
    ];

    [Test]
    [MethodDataSource(nameof(GetFailingPercentages))]
    public void SetAmountAndParticipantsWithPercentualSplit_SumOfPercentagesDoesntAddUpTo100_Throws(
        int[] percentages)
    {
        true.ShouldBeFalse();
    }

    public static IEnumerable<Func<int[]>> GetValidPercentages() =>
    [
        () => [33, 33, 34],
        () => [33, 67],
        () => [65, 35],
    ];

    [Test]
    [MethodDataSource(nameof(GetValidPercentages))]
    public void SetAmountAndParticipantsWithPercentualSplit_SetsSplitTypeAndCreatesParticipants(int[] percentages)
    {
        true.ShouldBeFalse();
    }

    [Test]
    public void SetAmountAndParticipantsWithPercentualSplit_ClearsExistingExactAmountValues()
    {
        true.ShouldBeFalse();
    }

    #endregion

    #region ExactAmount

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public void SetAmountAndParticipantsWithExactSplit_NegativeOrZeroAmount_Throws(decimal amount)
    {
        true.ShouldBeFalse();
    }

    [Test]
    public void SetAmountAndParticipantsWithExactSplit_EmptyParticipants_Throws()
    {
        true.ShouldBeFalse();
    }

    [Test]
    public void SetAmountAndParticipantsWithExactSplit_SumOfAmountsDoesNotAddUpToAmount_Throws()
    {
        true.ShouldBeFalse();
    }


    [Test]
    public void SetAmountAndParticipantsWithExactSplit_SetsSplitTypeAndCreatesParticipants()
    {
        true.ShouldBeFalse();
    }


    [Test]
    public void SetAmountAndParticipantsWithExactSplit_ClearsExistingPercentualValues()
    {
        true.ShouldBeFalse();
    }

    #endregion
}