using SharedKernel.Domain;

namespace ProductCatalog.Domain.Events;

public class ProductCategoryCreatedEvent : BaseDomainEvent
{
    public int ProductCategoryId { get; }
    public string ProductCategoryName { get; }

    public ProductCategoryCreatedEvent(int productCategoryId, string productCategoryName)
    {
        ProductCategoryId = productCategoryId;
        ProductCategoryName = productCategoryName;
    }
}

public class ProductCategoryUpdatedEvent : BaseDomainEvent
{
    public int ProductCategoryId { get; }
    public string OldName { get; }
    public string NewName { get; }

    public ProductCategoryUpdatedEvent(int productCategoryId, string oldName, string newName)
    {
        ProductCategoryId = productCategoryId;
        OldName = oldName;
        NewName = newName;
    }
}

public class ProductAddedToProductCategoryEvent : BaseDomainEvent
{
    public int ProductCategoryId { get; }
    public int ProductId { get; }
    public string ProductName { get; }

    public ProductAddedToProductCategoryEvent(int productCategoryId, int productId, string productName)
    {
        ProductCategoryId = productCategoryId;
        ProductId = productId;
        ProductName = productName;
    }
}

public class ProductRemovedFromProductCategoryEvent : BaseDomainEvent
{
    public int ProductCategoryId { get; }
    public int ProductId { get; }
    public string ProductName { get; }

    public ProductRemovedFromProductCategoryEvent(int productCategoryId, int productId, string productName)
    {
        ProductCategoryId = productCategoryId;
        ProductId = productId;
        ProductName = productName;
    }
}
