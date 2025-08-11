using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.ValueObjects;

namespace ProductCatalog.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for ProductCategory entity
/// </summary>
public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("ProductCategories");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(s => s.ProductCategoryName)
            .IsRequired()
            .HasMaxLength(200)
            .HasConversion(
                v => v.Value,
                v => ProductCategoryName.Create(v));

        builder.Property(s => s.CreatedDate)
            .IsRequired();

        builder.Property(s => s.CreatedBy)
            .HasMaxLength(100);

        builder.Property(s => s.ModifiedDate);

        builder.Property(s => s.ModifiedBy)
            .HasMaxLength(100);

        // Unique constraint on ProductCategoryName
        builder.HasIndex(s => s.ProductCategoryName)
            .IsUnique();

        // Configure relationship with Products
        builder.HasMany(s => s.Products)
            .WithOne(t => t.ProductCategory)
            .HasForeignKey(t => t.ProductCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore domain events for EF Core
        builder.Ignore(s => s.DomainEvents);
    }
}
