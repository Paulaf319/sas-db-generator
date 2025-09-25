using Sastral.DbGenerator.Domain.Common;
using Sastral.DbGenerator.Domain.Entities.Orders;

namespace Sastral.DbGenerator.Domain.Entities.Shipments;

public enum ShipmentProvider
{
    MercadoEnvios,
    Correo,
    Pickup
}

public enum ShipmentMethodStatus
{
    Pending,
    Processing,
    Shipped,
    InTransit,
    Delivered,
    Failed,
    Returned
}

public class Shipment : BaseAuditableEntity
{
    public Guid OrderId { get; private set; }
    public ShipmentProvider Provider { get; private set; }
    public string? TrackingCode { get; private set; }
    public string Address { get; private set; } = string.Empty;
    public string? City { get; private set; }
    public string? State { get; private set; }
    public string? PostalCode { get; private set; }
    public string? Country { get; private set; }
    public decimal Cost { get; private set; }
    public ShipmentMethodStatus Status { get; private set; }
    public DateTime? EstimatedDeliveryDate { get; private set; }
    public DateTime? ActualDeliveryDate { get; private set; }

    // Navigation properties
    public Order Order { get; private set; } = null!;

    private Shipment() { } // For EF Core

    public Shipment(Guid orderId, ShipmentProvider provider, string address, decimal cost)
    {
        OrderId = orderId;
        Provider = provider;
        Address = address;
        Cost = cost;
        Status = ShipmentMethodStatus.Pending;
    }

    public void UpdateAddress(string address, string? city = null, string? state = null, string? postalCode = null, string? country = null)
    {
        Address = address;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateTrackingCode(string trackingCode)
    {
        TrackingCode = trackingCode;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(ShipmentMethodStatus status)
    {
        Status = status;
        ModifiedAt = DateTime.UtcNow;

        if (status == ShipmentMethodStatus.Delivered && ActualDeliveryDate == null)
        {
            ActualDeliveryDate = DateTime.UtcNow;
        }
    }

    public void SetEstimatedDeliveryDate(DateTime estimatedDate)
    {
        EstimatedDeliveryDate = estimatedDate;
        ModifiedAt = DateTime.UtcNow;
    }

    public void MarkAsDelivered()
    {
        Status = ShipmentMethodStatus.Delivered;
        ActualDeliveryDate = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;
    }
}
