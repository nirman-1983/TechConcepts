using SharedKernel.Domain;
using SharedKernel.Exceptions;
using ProductCatalog.Domain.Events;
using ProductCatalog.Domain.ValueObjects;

namespace ProductCatalog.Domain.Entities;

/// <summary>
/// ProductCategory aggregate root representing a learning productCategory
/// </summary>
public class ProductCategory : AuditableEntity
{
    private readonly List<Product> _products = new();

    public ProductCategoryName ProductCategoryName { get; private set; } = ProductCategoryName.Create("Default");
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    // Private constructor for EF Core
    private ProductCategory() { }

    public ProductCategory(string productCategoryName)
    {
        ProductCategoryName = ProductCategoryName.Create(productCategoryName);
        AddDomainEvent(new ProductCategoryCreatedEvent(Id, ProductCategoryName.Value));
    }

    public void UpdateProductCategoryName(string productCategoryName)
    {
        var oldName = ProductCategoryName.Value;
        ProductCategoryName = ProductCategoryName.Create(productCategoryName);
        AddDomainEvent(new ProductCategoryUpdatedEvent(Id, oldName, ProductCategoryName.Value));
    }

    public Product AddProduct(string productName)
    {
        if (_products.Any(t => t.ProductName.Value.Equals(productName, StringComparison.OrdinalIgnoreCase)))
        {
            throw new DuplicateEntityException(nameof(Product), nameof(Product.ProductName), productName);
        }

        var product = new Product(productName, Id);
        _products.Add(product);
        AddDomainEvent(new ProductAddedToProductCategoryEvent(Id, product.Id, productName));
        return product;
    }

    public void RemoveProduct(int productId)
    {
        var product = _products.FirstOrDefault(t => t.Id == productId);
        if (product == null)
        {
            throw new EntityNotFoundException(nameof(Product), productId);
        }

        _products.Remove(product);
        AddDomainEvent(new ProductRemovedFromProductCategoryEvent(Id, productId, product.ProductName.Value));
    }


}
