using Sastral.DbGenerator.Domain.Common;
using Sastral.DbGenerator.Domain.Entities.Carts;
using Sastral.DbGenerator.Domain.Entities.Orders;
using Sastral.DbGenerator.Domain.Entities.Inventory;

namespace Sastral.DbGenerator.Domain.Entities.Products;

public class ProductVariant : BaseAuditableEntity
{
    public Guid ProductId { get; private set; }
    public string? Color { get; private set; }
    public string? Size { get; private set; }
    public string Barcode { get; private set; } = string.Empty;
    public int Stock { get; private set; }

    // Navigation properties
    public Product Product { get; private set; } = null!;
    public ICollection<CartItem> CartItems { get; private set; } = new List<CartItem>();
    public ICollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();
    public ICollection<InventoryMovement> InventoryMovements { get; private set; } = new List<InventoryMovement>();

    private ProductVariant() { } // For EF Core

    public ProductVariant(Guid productId, string barcode, int stock, string? color = null, string? size = null)
    {
        ProductId = productId;
        Color = color;
        Size = size;
        Barcode = barcode;
        Stock = stock;
    }

    public void UpdateStock(int stock)
    {
        Stock = stock;
        ModifiedAt = DateTime.UtcNow;
    }

    public void AdjustStock(int adjustment)
    {
        Stock += adjustment;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateVariant(string? color, string? size, string barcode)
    {
        Color = color;
        Size = size;
        Barcode = barcode;
        ModifiedAt = DateTime.UtcNow;
    }
}
