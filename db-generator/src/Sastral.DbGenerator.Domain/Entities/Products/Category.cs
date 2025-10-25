using Sastral.DbGenerator.Domain.Common;

namespace Sastral.DbGenerator.Domain.Entities.Products;

public class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public int? ParentId { get; private set; }

    // Navigation properties
    public Category? Parent { get; private set; }
    public ICollection<Category> Children { get; private set; } = new List<Category>();
    public ICollection<Product> Products { get; private set; } = new List<Product>();

    private Category() { } // For EF Core

    public Category(string name, int? parentId = null)
    {
        Name = name;
        ParentId = parentId;
    }

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void UpdateParent(int? parentId)
    {
        ParentId = parentId;
    }
}
