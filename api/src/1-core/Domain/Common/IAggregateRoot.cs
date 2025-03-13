namespace SplitTheBill.Domain.Common;

public interface IAggregateRoot<TId>
{
    public TId Id { get; init; }
}