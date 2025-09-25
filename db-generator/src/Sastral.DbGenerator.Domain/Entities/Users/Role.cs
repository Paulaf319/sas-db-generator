using Sastral.DbGenerator.Domain.Common;

namespace Sastral.DbGenerator.Domain.Entities.Users;

public class Role : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    // Navigation properties
    public ICollection<User> Users { get; private set; } = new List<User>();

    private Role() { } // For EF Core

    public Role(string name)
    {
        Name = name;
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}
