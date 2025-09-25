using Sastral.DbGenerator.Domain.Common;
using Sastral.DbGenerator.Domain.Entities.Products;
using Sastral.DbGenerator.Domain.Entities.Users;

namespace Sastral.DbGenerator.Domain.Entities.Inventory;

public enum MovementReason
{
    Purchase,
    Sale,
    Adjustment,
    Return,
    Damage,
    Loss,
    Transfer
}

public class InventoryMovement : BaseAuditableEntity
{
    public Guid VariantId { get; private set; }
    public int Quantity { get; private set; }
    public MovementReason Reason { get; private set; }
    public Guid PerformedBy { get; private set; }
    public string? Notes { get; private set; }

    // Navigation properties
    public ProductVariant Variant { get; private set; } = null!;
    public User PerformedByUser { get; private set; } = null!;

    private InventoryMovement() { } // For EF Core

    public InventoryMovement(Guid variantId, int quantity, MovementReason reason, Guid performedBy, string? notes = null)
    {
        VariantId = variantId;
        Quantity = quantity;
        Reason = reason;
        PerformedBy = performedBy;
        Notes = notes;
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes;
        ModifiedAt = DateTime.UtcNow;
    }
}
