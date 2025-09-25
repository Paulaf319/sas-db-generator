using Sastral.DbGenerator.Domain.Common;

namespace Sastral.DbGenerator.Domain.Entities.Products;

public class Product : BaseAuditableEntity
{
    public string Sku { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid? BrandId { get; private set; }
    public Guid CategoryId { get; private set; }
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation properties
    public Category Category { get; private set; } = null!;
    public ICollection<ProductVariant> Variants { get; private set; } = new List<ProductVariant>();

    private Product() { } // For EF Core

    public Product(string sku, string name, string? description, Guid categoryId, decimal price, Guid? brandId = null)
    {
        Sku = sku;
        Name = name;
        Description = description;
        CategoryId = categoryId;
        Price = price;
        BrandId = brandId;
        IsActive = true;
    }

    public void Update(string name, string? description, decimal price, bool isActive)
    {
        Name = name;
        Description = description;
        Price = price;
        IsActive = isActive;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal price)
    {
        Price = price;
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
