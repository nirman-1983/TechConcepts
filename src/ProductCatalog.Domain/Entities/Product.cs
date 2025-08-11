using SharedKernel.Domain;
using SharedKernel.Exceptions;
using ProductCatalog.Domain.Events;
using ProductCatalog.Domain.ValueObjects;

namespace ProductCatalog.Domain.Entities;

/// <summary>
/// Product entity representing a learning product within a productCategory
/// </summary>
public class Product : AuditableEntity
{

    public ProductName ProductName { get; private set; } = ProductName.Create("Default");
    public int ProductCategoryId { get; private set; }

    // Navigation property
    public ProductCategory? ProductCategory { get; private set; }

    // Private constructor for EF Core
    private Product() { }

    public Product(string productName, int productCategoryId)
    {
        ProductName = ProductName.Create(productName);
        ProductCategoryId = productCategoryId;
        AddDomainEvent(new ProductCreatedEvent(Id, ProductName.Value, productCategoryId));
    }

    public void UpdateProductName(string productName)
    {
        var oldName = ProductName.Value;
        ProductName = ProductName.Create(productName);
        AddDomainEvent(new ProductUpdatedEvent(Id, oldName, ProductName.Value));
    }
}
