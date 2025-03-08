using SplitTheBillPocV1.Models;

namespace SplitTheBillPoc.Tests.TestData.Builders;

internal class GroupBuilder
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
    
    internal GroupBuilder AddMember(Member member)
    {
        _members.Add(member);
        return this;
    }
    
    internal GroupBuilder WithExpenses(List<Expense> expenses)
    {
        _expenses = expenses;
        return this;
    }
    
    internal GroupBuilder AddExpense(Expense expense)
    {
        _expenses.Add(expense);
        return this;
    }
    
    internal GroupBuilder WithPayments(List<Payment> payments)
    {
        _payments = payments;
        return this;
    }
    
    internal GroupBuilder AddPayment(Payment payment)
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
        Payments = _payments
    };
    
    public static implicit operator Group(GroupBuilder builder) => builder.Build();
}