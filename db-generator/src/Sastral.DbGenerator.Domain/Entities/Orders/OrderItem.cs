using Sastral.DbGenerator.Domain.Common;
using Sastral.DbGenerator.Domain.Entities.Products;

namespace Sastral.DbGenerator.Domain.Entities.Orders;

public class OrderItem : BaseEntity
{
    public int OrderId { get; private set; }
    public int VariantId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    // Navigation properties
    public Order Order { get; private set; } = null!;
    public ProductVariant Variant { get; private set; } = null!;

    private OrderItem() { } // For EF Core

    public OrderItem(int orderId, int variantId, int quantity, decimal unitPrice)
    {
        OrderId = orderId;
        VariantId = variantId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
    }

    public void UpdateUnitPrice(decimal unitPrice)
    {
        UnitPrice = unitPrice;
    }
}
