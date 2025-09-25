using Sastral.DbGenerator.Domain.Common;
using Sastral.DbGenerator.Domain.Entities.Products;

namespace Sastral.DbGenerator.Domain.Entities.Carts;

public class CartItem : BaseEntity
{
    public Guid CartId { get; private set; }
    public Guid VariantId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    // Navigation properties
    public Cart Cart { get; private set; } = null!;
    public ProductVariant Variant { get; private set; } = null!;

    private CartItem() { } // For EF Core

    public CartItem(Guid cartId, Guid variantId, int quantity, decimal unitPrice)
    {
        CartId = cartId;
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
