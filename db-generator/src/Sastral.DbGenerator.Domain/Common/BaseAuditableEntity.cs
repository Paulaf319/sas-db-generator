namespace Sastral.DbGenerator.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public string? CreatedBy { get; protected set; }
    public DateTime? ModifiedAt { get; protected set; }
    public string? ModifiedBy { get; protected set; }
}
