namespace SplitTheBill.Domain.Models.Groups;

public enum ExpenseSplitType : byte
{
    Evenly = 0,
    Percentual = 1,
    ExactAmount = 2,
}
