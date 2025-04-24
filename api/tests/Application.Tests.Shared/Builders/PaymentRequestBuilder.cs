using SplitTheBill.Application.Modules.Groups.Payments;

namespace SplitTheBill.Application.Tests.Shared.Builders;

public sealed class PaymentRequestBuilder
{
    private Guid? _groupId = Guid.NewGuid();
    private Guid? _paymentId = Guid.NewGuid();
    private Guid? _sendingMemberId = Guid.NewGuid();
    private Guid? _receivingMemberId = Guid.NewGuid();
    private decimal? _amount = 100m;
    private DateTimeOffset? _timestamp = DateTimeOffset.UtcNow;

    public PaymentRequestBuilder WithGroupId(Guid? groupId)
    {
        _groupId = groupId;
        return this;
    }

    public PaymentRequestBuilder WithPaymentId(Guid? paymentId)
    {
        _paymentId = paymentId;
        return this;
    }

    public PaymentRequestBuilder WithSendingMemberId(Guid? sendingMemberId)
    {
        _sendingMemberId = sendingMemberId;
        return this;
    }

    public PaymentRequestBuilder WithReceivingMemberId(Guid? receivingMemberId)
    {
        _receivingMemberId = receivingMemberId;
        return this;
    }

    public PaymentRequestBuilder WithAmount(decimal? amount)
    {
        _amount = amount;
        return this;
    }

    public PaymentRequestBuilder WithTimestamp(DateTimeOffset? timestamp)
    {
        _timestamp = timestamp;
        return this;
    }

    public UpdatePayment.Request BuildUpdateRequest() =>
        new()
        {
            GroupId = _groupId,
            PaymentId = _paymentId,
            SendingMemberId = _sendingMemberId,
            ReceivingMemberId = _receivingMemberId,
            Amount = _amount,
            Timestamp = _timestamp,
        };

    public static implicit operator UpdatePayment.Request(PaymentRequestBuilder builder) =>
        builder.BuildUpdateRequest();

    public CreatePayment.Request BuildCreateRequest() =>
        new()
        {
            GroupId = _groupId,
            SendingMemberId = _sendingMemberId,
            ReceivingMemberId = _receivingMemberId,
            Amount = _amount,
            Timestamp = _timestamp,
        };

    public static implicit operator CreatePayment.Request(PaymentRequestBuilder builder) =>
        builder.BuildCreateRequest();
}
