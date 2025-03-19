using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Application.Tests.Shared.TestData.Builders;

public sealed class GroupBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _name = "group name";
    private List<Member>? _members = null;
    private List<Guid>? _memberIds = null;
    private List<Expense> _expenses = [];
    private List<Payment> _payments = [];

    public GroupBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public GroupBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public GroupBuilder WithMembers(List<Member> members)
    {
        _members = members;
        return this;
    }

    public GroupBuilder WithMembers(List<Guid> memberIds)
    {
        _memberIds = memberIds;
        return this;
    }

    public GroupBuilder WithExpenses(List<Expense> expenses)
    {
        _expenses = expenses;
        return this;
    }

    public GroupBuilder AddExpense(Expense expense)
    {
        _expenses.Add(expense);
        return this;
    }

    public GroupBuilder WithPayments(List<Payment> payments)
    {
        _payments = payments;
        return this;
    }

    public GroupBuilder AddPayment(Payment payment)
    {
        _payments.Add(payment);
        return this;
    }

    public Group Build() => new()
    {
        Id = _id,
        Name = _name,
        // if memberIds is defined, set null, otherwise use members and finally fall back to empty list
        Members = _memberIds?.Any() == true ? null! : _members ?? [],
        // not defined -> fall back to null, Members will fall back to empty list
        GroupMembers = _memberIds?
                           .Select(id => new GroupMember { MemberId = id })
                           .ToList()
                       ?? null!,
        Expenses = _expenses,
        Payments = _payments,
    };

    public static implicit operator Group(GroupBuilder builder) => builder.Build();
}