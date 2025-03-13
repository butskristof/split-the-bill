using SplitTheBill.Domain.Models.Groups;

namespace SplitTheBill.Application.Modules.Groups;

internal sealed record GroupDto
{
    private readonly Group _group;

    public GroupDto(Group group)
    {
        _group = group;
    }

    public Guid Id => _group.Id;
    public string Name => _group.Name;

    public List<PaymentDto> Payments => _group.Payments
        .Select(p => new PaymentDto(p))
        .ToList();

    internal sealed record PaymentDto(
        Guid Id,
        Guid SendingMemberId,
        Guid ReceivingMemberId,
        decimal Amount
    )
    {
        public PaymentDto(Payment payment)
            : this(payment.Id, payment.SendingMemberId, payment.ReceivingMemberId, payment.Amount)
        {
        }
    }
}