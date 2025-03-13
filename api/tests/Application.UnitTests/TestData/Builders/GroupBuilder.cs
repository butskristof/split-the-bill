using SplitTheBill.Domain.Models.Groups;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Application.UnitTests.TestData.Builders;

internal sealed class GroupBuilder
{
    private Guid _id = Guid.Empty;
    private string _name = string.Empty;
    private List<Member> _members = [];
    private List<Expense> _expenses = [];
    private List<Payment> _payments = [];

    internal GroupBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    internal GroupBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
    
    internal GroupBuilder WithMembers(List<Member> members)
    {
        _members = members;
        return this;
    }
    
    internal GroupBuilder WithMember(Member member)
    {
        _members.Add(member);
        return this;
    }
    
    internal GroupBuilder WithExpenses(List<Expense> expenses)
    {
        _expenses = expenses;
        return this;
    }
    
    internal GroupBuilder WithExpense(Expense expense)
    {
        _expenses.Add(expense);
        return this;
    }
    
    internal GroupBuilder WithPayments(List<Payment> payments)
    {
        _payments = payments;
        return this;
    }
    
    internal GroupBuilder WithPayment(Payment payment)
    {
        _payments.Add(payment);
        return this;
    }
    
    internal Group Build() => new()
    {
        Id = _id,
        Name = _name,
        Members = _members,
        Expenses = _expenses,
        Payments = _payments,
    };

    public static implicit operator Group(GroupBuilder builder) => builder.Build();
}