using Sastral.DbGenerator.Domain.Common;
using Sastral.DbGenerator.Domain.Entities.Users;

namespace Sastral.DbGenerator.Domain.Entities.Carts;

public enum CartStatus
{
    Active,
    Abandoned,
    Converted
}

public class Cart : BaseAuditableEntity
{
    public int? UserId { get; private set; }
    public CartStatus Status { get; private set; }

    // Navigation properties
    public User? User { get; private set; }
    public ICollection<CartItem> Items { get; private set; } = new List<CartItem>();

    private Cart() { } // For EF Core

    public Cart(int? userId = null)
    {
        UserId = userId;
        Status = CartStatus.Active;
    }

    public void AddItem(int variantId, int quantity, decimal unitPrice)
    {
        var existingItem = Items.FirstOrDefault(i => i.VariantId == variantId);
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var item = new CartItem(Id, variantId, quantity, unitPrice);
            Items.Add(item);
        }
        ModifiedAt = DateTime.UtcNow;
    }

    public void RemoveItem(int variantId)
    {
        var item = Items.FirstOrDefault(i => i.VariantId == variantId);
        if (item != null)
        {
            Items.Remove(item);
            ModifiedAt = DateTime.UtcNow;
        }
    }

    public void UpdateItemQuantity(int variantId, int quantity)
    {
        var item = Items.FirstOrDefault(i => i.VariantId == variantId);
        if (item != null)
        {
            if (quantity <= 0)
            {
                Items.Remove(item);
            }
            else
            {
                item.UpdateQuantity(quantity);
            }
            ModifiedAt = DateTime.UtcNow;
        }
    }

    public void Clear()
    {
        Items.Clear();
        ModifiedAt = DateTime.UtcNow;
    }

    public void MarkAsAbandoned()
    {
        Status = CartStatus.Abandoned;
        ModifiedAt = DateTime.UtcNow;
    }

    public void MarkAsConverted()
    {
        Status = CartStatus.Converted;
        ModifiedAt = DateTime.UtcNow;
    }

    public decimal GetTotal()
    {
        return Items.Sum(i => i.Quantity * i.UnitPrice);
    }
}
