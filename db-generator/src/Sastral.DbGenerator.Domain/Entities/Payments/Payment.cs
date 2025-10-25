using Sastral.DbGenerator.Domain.Common;
using Sastral.DbGenerator.Domain.Entities.Orders;

namespace Sastral.DbGenerator.Domain.Entities.Payments;

public enum PaymentProvider
{
    MercadoPago,
    Stripe,
    Cash
}

public enum PaymentMethodStatus
{
    Pending,
    Processing,
    Approved,
    Rejected,
    Cancelled,
    Refunded
}

public class Payment : BaseAuditableEntity
{
    public int OrderId { get; private set; }
    public PaymentProvider Provider { get; private set; }
    public string? ProviderPaymentId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentMethodStatus Status { get; private set; }
    public string? FailureReason { get; private set; }

    // Navigation properties
    public Order Order { get; private set; } = null!;

    private Payment() { } // For EF Core

    public Payment(int orderId, PaymentProvider provider, decimal amount, string? providerPaymentId = null)
    {
        OrderId = orderId;
        Provider = provider;
        Amount = amount;
        ProviderPaymentId = providerPaymentId;
        Status = PaymentMethodStatus.Pending;
    }

    public void UpdateStatus(PaymentMethodStatus status, string? failureReason = null)
    {
        Status = status;
        FailureReason = failureReason;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateProviderPaymentId(string providerPaymentId)
    {
        ProviderPaymentId = providerPaymentId;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Approve()
    {
        Status = PaymentMethodStatus.Approved;
        FailureReason = null;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Reject(string reason)
    {
        Status = PaymentMethodStatus.Rejected;
        FailureReason = reason;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Refund()
    {
        Status = PaymentMethodStatus.Refunded;
        ModifiedAt = DateTime.UtcNow;
    }
}
