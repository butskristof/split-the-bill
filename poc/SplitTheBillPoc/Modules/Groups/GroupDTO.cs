using SplitTheBillPoc.Models;

namespace SplitTheBillPoc.Modules.Groups;

internal sealed class GroupDTO
{
    public Guid Id { get; init; }
    public required string Name { get; set; }

    public ICollection<MemberDTO> Members { get; set; } = [];
    public ICollection<ExpenseDTO> Expenses { get; set; } = [];
    public ICollection<PaymentDTO> Payments { get; set; } = [];

    public decimal TotalExpenses => Expenses.Sum(e => e.Amount);
    public decimal TotalPaymentAmount => Payments.Sum(p => p.Amount);
    public decimal TotalAmountDue => TotalExpenses - TotalPaymentAmount;

    internal sealed class MemberDTO
    {
        public Guid Id { get; init; }
        public required string Name { get; set; }
    }

    internal sealed class ExpenseDTO
    {
        public Guid Id { get; set; }
        public required decimal Amount { get; set; }

        public required Guid PaidByMemberId { get; set; }
    }

    internal sealed class PaymentDTO
    {
        public Guid Id { get; set; }
        public required decimal Amount { get; set; }

        public required Guid PaidByMemberId { get; set; }
        public required Guid PaidToMemberId { get; set; }
    }
}

internal static class GroupDTOExtensions
{
    internal static GroupDTO ToDTO(this Group group)
        => new()
        {
            Id = group.Id,
            Name = group.Name,
            Members = group.Members.Select(m => new GroupDTO.MemberDTO
            {
                Id = m.Id,
                Name = m.Name,
            }).ToList(),
            Expenses = group.Expenses.Select(g => new GroupDTO.ExpenseDTO
            {
                Id = g.Id,
                Amount = g.Amount,
                PaidByMemberId = g.PaidByMemberId,
            }).ToList(),
            Payments = group.Payments.Select(p => new GroupDTO.PaymentDTO
            {
                Id = p.Id,
                Amount = p.Amount,
                PaidByMemberId = p.PaidByMemberId,
                PaidToMemberId = p.PaidToMemberId,
            }).ToList(),
        };
}