using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sastral.DbGenerator.Domain.Entities.Users;

namespace Sastral.DbGenerator.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        // Seed data
        builder.HasData(
            new { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Admin" },
            new { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Operador" }
        );
    }
}
