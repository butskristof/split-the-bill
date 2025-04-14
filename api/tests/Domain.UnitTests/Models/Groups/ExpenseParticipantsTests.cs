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
    public void SetAmount_EvenSplit_SetsValue()
    {
        var expense = ExpenseBuilder.Create();
        expense.SetAmountAndParticipantsWithEvenSplit(
            100,
            new HashSet<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        );

        const decimal amount = 200;
        expense.Amount = amount;
        expense.Amount.ShouldBe(amount);
    }

    [Test]
    public void SetAmount_PercentualSplit_SetsValue()
    {
        var expense = ExpenseBuilder.Create();
        expense.SetAmountAndParticipantsWithPercentualSplit(
            100,
            new Dictionary<Guid, int>
            {
                { Guid.NewGuid(), 50 },
                { Guid.NewGuid(), 50 },
            });

        const decimal amount = 200;
        expense.Amount = amount;
        expense.Amount.ShouldBe(amount);
    }

    [Test]
    public void SetAmount_ExactAmountSplit_DoesNotMatchSumOfParticipantAmounts_Throws()
    {
        var expense = ExpenseBuilder.Create();
        expense.SetAmountAndParticipantsWithExactSplit(
            1000,
            new Dictionary<Guid, decimal>
            {
                { Guid.NewGuid(), 500 },
                { Guid.NewGuid(), 500 },
            });

        const decimal amount = 1100;
        Should.Throw<ArgumentException>(() =>
                expense.Amount = amount
            )
            .ParamName.ShouldBe("Amount");
    }

    [Test]
    public void SetAmount_ExactAmountSplit_SetsValueIfAmountMatchesParticipants()
    {
        var expense = ExpenseBuilder.Create();
        expense.SetAmountAndParticipantsWithExactSplit(
            1000,
            new Dictionary<Guid, decimal>
            {
                { Guid.NewGuid(), 500 },
                { Guid.NewGuid(), 500 },
            });

        const decimal amount = 1000;
        expense.Amount = amount;
        expense.Amount.ShouldBe(amount);
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
            )
            .ParamName.ShouldBe("Amount");
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
            )
            .ParamName.ShouldBe("Amount");
    }

    [Test]
    public void SetAmountAndParticipantsWithPercentualSplit_EmptyParticipants_Throws()
    {
        var expense = ExpenseBuilder.Create();
        Should.Throw<ArgumentException>(() =>
                expense.SetAmountAndParticipantsWithPercentualSplit(
                    100,
                    new Dictionary<Guid, int>()
                )
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
        var expense = ExpenseBuilder.Create();
        Should.Throw<ArgumentException>(() =>
                expense.SetAmountAndParticipantsWithPercentualSplit(
                    100,
                    percentages.ToDictionary(_ => Guid.NewGuid(), v => v)
                )
            )
            .ParamName.ShouldBe("participants");
    }

    [Test]
    public void SetAmountAndParticipantsWithPercentualSplit_NegativePercentage_Throws()
    {
        var expense = ExpenseBuilder.Create();
        Should.Throw<ArgumentException>(() =>
                expense.SetAmountAndParticipantsWithPercentualSplit(
                    100,
                    new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 101 },
                        { Guid.NewGuid(), -1 },
                    }
                )
            )
            .ParamName.ShouldBe("participants");
    }

    public static IEnumerable<Func<int[]>> GetValidPercentages() =>
    [
        () => [33, 33, 34],
        () => [33, 67],
        () => [65, 35],
        () => [65, 35, 0],
    ];

    [Test]
    [MethodDataSource(nameof(GetValidPercentages))]
    public void SetAmountAndParticipantsWithPercentualSplit_SetsSplitTypeAndCreatesParticipants(int[] percentages)
    {
        var expense = ExpenseBuilder.Create();
        const decimal amount = 100;
        var participants = percentages.ToDictionary(_ => Guid.NewGuid(), v => v);

        expense.SetAmountAndParticipantsWithPercentualSplit(amount, participants);

        expense.SplitType.ShouldBe(ExpenseSplitType.Percentual);
        expense.Amount.ShouldBe(amount);
        expense.Participants
            .ShouldAllBe(p => !p.ExactShare.HasValue);

        foreach (var (id, percentage) in participants)
        {
            var participant = expense.Participants
                .Where(p => p.MemberId == id)
                .ShouldHaveSingleItem();
            participant.PercentualShare.ShouldBe(percentage);
            expense.GetExpenseAmountForMember(id)
                .ShouldBe((percentage / 100.0m) * amount);
        }
    }

    [Test]
    public void SetAmountAndParticipantsWithPercentualSplit_ClearsExistingExactAmountValues()
    {
        var expense = ExpenseBuilder.Create();
        const decimal amount = 200.0m;
        HashSet<Guid> memberIds = [Guid.NewGuid(), Guid.NewGuid()];
        expense.SetAmountAndParticipantsWithExactSplit(
            amount,
            memberIds.ToDictionary(id => id, _ => 100m)
        );

        expense.SetAmountAndParticipantsWithPercentualSplit(amount,
            memberIds.ToDictionary(id => id, _ => 50)
        );

        expense.Participants
            .ShouldAllBe(p =>
                !p.ExactShare.HasValue
            );
    }

    #endregion

    #region ExactAmount

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public void SetAmountAndParticipantsWithExactSplit_NegativeOrZeroAmount_Throws(decimal amount)
    {
        var expense = ExpenseBuilder.Create();
        Should.Throw<ArgumentException>(() =>
                expense.SetAmountAndParticipantsWithExactSplit(
                    amount,
                    new Dictionary<Guid, decimal> { { Guid.NewGuid(), amount } }
                )
            );
    }

    [Test]
    public void SetAmountAndParticipantsWithExactSplit_EmptyParticipants_Throws()
    {
        var expense = ExpenseBuilder.Create();
        Should.Throw<ArgumentException>(() =>
                expense.SetAmountAndParticipantsWithExactSplit(
                    100,
                    new Dictionary<Guid, decimal>()
                )
            )
            .ParamName.ShouldBe("participants");
    }

    public static IEnumerable<Func<(decimal, decimal[])>> GetFailingExactAmounts() =>
    [
        () => (1m, [1m, 1m]),
        () => (1000m, [100m, 100]),
        () => (1000m, [330m, 330, 330m]),
        () => (1000m, [100m, 200, 300m]),
        () => (1000m, [2m, 999m]),
        () => (1000m, [1m, 998m]),
    ];

    [Test]
    [MethodDataSource(nameof(GetFailingExactAmounts))]
    public void SetAmountAndParticipantsWithExactSplit_SumOfAmountsDoesNotAddUpToAmount_Throws(
        decimal amount,
        decimal[] participantAmounts
    )
    {
        var expense = ExpenseBuilder.Create();
        Should.Throw<ArgumentException>(() =>
                expense.SetAmountAndParticipantsWithExactSplit(
                    amount,
                    participantAmounts.ToDictionary(_ => Guid.NewGuid(), v => v)
                )
            )
            .ParamName.ShouldBe("participants");
    }

    [Test]
    public void SetAmountAndParticipantsWithExactSplit_NegativeAmount_Throws()
    {
        var expense = ExpenseBuilder.Create();
        Should.Throw<ArgumentException>(() =>
                expense.SetAmountAndParticipantsWithExactSplit(
                    1000,
                    new Dictionary<Guid, decimal>
                    {
                        { Guid.NewGuid(), 1001 },
                        { Guid.NewGuid(), -1 },
                    }
                )
            )
            .ParamName.ShouldBe("participants");
    }

    public static IEnumerable<Func<(decimal, decimal[])>> GetValidExactAmounts() =>
    [
        () => (1000m, [500m, 500m]),
        () => (1000m, [100m, 900m]),
        () => (1000m, [333m, 333m, 334m]),
        () => (1000m, [333m, 667m]),
        () => (1000m, [1m, 999m]),
        () => (1000m, [1m, 999m, 0m]),
    ];

    [Test]
    [MethodDataSource(nameof(GetValidExactAmounts))]
    public void SetAmountAndParticipantsWithExactSplit_SetsSplitTypeAndCreatesParticipants(
        decimal amount,
        decimal[] participantAmounts)
    {
        var expense = ExpenseBuilder.Create();
        var participants = participantAmounts.ToDictionary(_ => Guid.NewGuid(), v => v);

        expense.SetAmountAndParticipantsWithExactSplit(amount, participants);

        expense.SplitType.ShouldBe(ExpenseSplitType.ExactAmount);
        expense.Amount.ShouldBe(amount);
        foreach (var (id, participantAmount) in participants)
        {
            var participant = expense.Participants
                .Where(p => p.MemberId == id)
                .ShouldHaveSingleItem();
            participant.ExactShare.ShouldBe(participantAmount);
            expense.GetExpenseAmountForMember(id)
                .ShouldBe(participantAmount);
        }
    }

    [Test]
    public void SetAmountAndParticipantsWithExactSplit_ClearsExistingPercentualValues()
    {
        var expense = ExpenseBuilder.Create();
        const decimal amount = 200m;
        HashSet<Guid> memberIds = [Guid.NewGuid(), Guid.NewGuid()];
        expense.SetAmountAndParticipantsWithPercentualSplit(
            amount,
            memberIds.ToDictionary(id => id, _ => 50)
        );

        expense.SetAmountAndParticipantsWithExactSplit(amount,
            memberIds.ToDictionary(id => id, _ => 100m)
        );

        expense.Participants
            .ShouldAllBe(p =>
                !p.PercentualShare.HasValue
            );
    }

    #endregion
}