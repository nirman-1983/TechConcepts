using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.ValueObjects;

namespace ProductCatalog.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Product entity
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.ProductName)
            .IsRequired()
            .HasMaxLength(200)
            .HasConversion(
                v => v.Value,
                v => ProductName.Create(v));

        builder.Property(t => t.ProductCategoryId)
            .HasColumnName("ProductCategoryID")
            .IsRequired();

        builder.Property(t => t.CreatedDate)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(100);

        builder.Property(t => t.ModifiedDate);

        builder.Property(t => t.ModifiedBy)
            .HasMaxLength(100);

        // Unique constraint on ProductName and ProductCategoryId combination
        builder.HasIndex(t => new { t.ProductName, t.ProductCategoryId })
            .IsUnique();

        // Configure relationship with ProductCategory
        builder.HasOne(t => t.ProductCategory)
            .WithMany(s => s.Products)
            .HasForeignKey(t => t.ProductCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore domain events for EF Core
        builder.Ignore(t => t.DomainEvents);
    }
}
