using SplitTheBill.Application.Modules.Groups.Payments;

namespace SplitTheBill.Application.Tests.Shared.Builders;

public sealed class UpdatePaymentRequestBuilder
{
    private Guid? _groupId = null;
    private Guid? _paymentId = null;
    private Guid? _sendingMemberId = null;
    private Guid? _receivingMemberId = null;
    private decimal? _amount = null;

    public UpdatePaymentRequestBuilder WithGroupId(Guid? groupId)
    {
        _groupId = groupId;
        return this;
    }

    public UpdatePaymentRequestBuilder WithPaymentId(Guid? paymentId)
    {
        _paymentId = paymentId;
        return this;
    }

    public UpdatePaymentRequestBuilder WithSendingMemberId(Guid? sendingMemberId)
    {
        _sendingMemberId = sendingMemberId;
        return this;
    }

    public UpdatePaymentRequestBuilder WithReceivingMemberId(Guid? receivingMemberId)
    {
        _receivingMemberId = receivingMemberId;
        return this;
    }

    public UpdatePaymentRequestBuilder WithAmount(decimal? amount)
    {
        _amount = amount;
        return this;
    }

    public UpdatePayment.Request Build() => new()
    {
        GroupId = _groupId,
        PaymentId = _paymentId,
        SendingMemberId = _sendingMemberId,
        ReceivingMemberId = _receivingMemberId,
        Amount = _amount
    };

    public static implicit operator UpdatePayment.Request(UpdatePaymentRequestBuilder builder) => builder.Build();
}