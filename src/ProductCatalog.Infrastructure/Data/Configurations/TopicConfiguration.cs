using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.ValueObjects;

namespace ProductCatalog.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Topic entity
/// </summary>
public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.ToTable("Topics");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.TopicName)
            .IsRequired()
            .HasMaxLength(200)
            .HasConversion(
                v => v.Value,
                v => TopicName.Create(v));

        builder.Property(t => t.SubjectId)
            .HasColumnName("SubjectID")
            .IsRequired();

        builder.Property(t => t.CreatedDate)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(100);

        builder.Property(t => t.ModifiedDate);

        builder.Property(t => t.ModifiedBy)
            .HasMaxLength(100);

        // Unique constraint on TopicName and SubjectId combination
        builder.HasIndex(t => new { t.TopicName, t.SubjectId })
            .IsUnique();

        // Configure relationship with Subject
        builder.HasOne(t => t.Subject)
            .WithMany(s => s.Topics)
            .HasForeignKey(t => t.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore domain events for EF Core
        builder.Ignore(t => t.DomainEvents);
    }
}
