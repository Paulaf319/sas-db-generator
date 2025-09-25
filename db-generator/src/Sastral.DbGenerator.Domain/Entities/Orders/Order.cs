using Sastral.DbGenerator.Domain.Common;
using Sastral.DbGenerator.Domain.Entities.Users;

namespace Sastral.DbGenerator.Domain.Entities.Orders;

public enum OrderStatus
{
    Draft,
    Confirmed,
    Paid,
    Shipped,
    Delivered,
    Cancelled
}

public enum PaymentStatus
{
    Pending,
    Paid,
    Failed,
    Refunded
}

public enum ShippingStatus
{
    Pending,
    Processing,
    Shipped,
    InTransit,
    Delivered,
    Failed
}

public class Order : BaseAuditableEntity
{
    public string Number { get; private set; } = string.Empty;
    public Guid? UserId { get; private set; }
    public decimal Total { get; private set; }
    public OrderStatus Status { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
    public ShippingStatus ShippingStatus { get; private set; }

    // Navigation properties
    public User? User { get; private set; }
    public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();
    public ICollection<Payment> Payments { get; private set; } = new List<Payment>();
    public ICollection<Shipment> Shipments { get; private set; } = new List<Shipment>();

    private Order() { } // For EF Core

    public Order(string number, Guid? userId = null)
    {
        Number = number;
        UserId = userId;
        Status = OrderStatus.Draft;
        PaymentStatus = PaymentStatus.Pending;
        ShippingStatus = ShippingStatus.Pending;
        Total = 0;
    }

    public void AddItem(Guid variantId, int quantity, decimal unitPrice)
    {
        var existingItem = Items.FirstOrDefault(i => i.VariantId == variantId);
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var item = new OrderItem(Id, variantId, quantity, unitPrice);
            Items.Add(item);
        }
        RecalculateTotal();
    }

    public void RemoveItem(Guid variantId)
    {
        var item = Items.FirstOrDefault(i => i.VariantId == variantId);
        if (item != null)
        {
            Items.Remove(item);
            RecalculateTotal();
        }
    }

    public void UpdateStatus(OrderStatus status)
    {
        Status = status;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdatePaymentStatus(PaymentStatus paymentStatus)
    {
        PaymentStatus = paymentStatus;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateShippingStatus(ShippingStatus shippingStatus)
    {
        ShippingStatus = shippingStatus;
        ModifiedAt = DateTime.UtcNow;
    }

    private void RecalculateTotal()
    {
        Total = Items.Sum(i => i.Quantity * i.UnitPrice);
        ModifiedAt = DateTime.UtcNow;
    }
}
