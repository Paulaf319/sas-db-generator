using Sastral.DbGenerator.Domain.Common;
using Sastral.DbGenerator.Domain.Entities.Users;

namespace Sastral.DbGenerator.Domain.Entities.Audits;

public class AuditLog : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public string Entity { get; private set; } = string.Empty;
    public Guid EntityId { get; private set; }
    public string? DataBefore { get; private set; }
    public string? DataAfter { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public string? IpAddress { get; private set; }

    // Navigation properties
    public User User { get; private set; } = null!;

    private AuditLog() { } // For EF Core

    public AuditLog(Guid userId, string action, string entity, Guid entityId, string? dataBefore = null, string? dataAfter = null, string? ipAddress = null)
    {
        UserId = userId;
        Action = action;
        Entity = entity;
        EntityId = entityId;
        DataBefore = dataBefore;
        DataAfter = dataAfter;
        IpAddress = ipAddress;
        CreatedAt = DateTime.UtcNow;
    }
}
