using Sastral.DbGenerator.Domain.Common;
using Sastral.DbGenerator.Domain.Entities.Orders;
using Sastral.DbGenerator.Domain.Entities.Carts;
using Sastral.DbGenerator.Domain.Entities.Audits;

namespace Sastral.DbGenerator.Domain.Entities.Users;

public class User : BaseAuditableEntity
{
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public Guid RoleId { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation properties
    public Role Role { get; private set; } = null!;
    public ICollection<Order> Orders { get; private set; } = new List<Order>();
    public ICollection<Cart> Carts { get; private set; } = new List<Cart>();
    public ICollection<AuditLog> AuditLogs { get; private set; } = new List<AuditLog>();

    private User() { } // For EF Core

    public User(string email, string passwordHash, Guid roleId)
    {
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;
        IsActive = true;
    }

    public void UpdateEmail(string email)
    {
        Email = email;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        ModifiedAt = DateTime.UtcNow;
    }
}
