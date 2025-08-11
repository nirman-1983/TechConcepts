using SharedKernel.Domain;

namespace ProductCatalog.Domain.Events;

public class ProductCreatedEvent : BaseDomainEvent
{
    public int ProductId { get; }
    public string ProductName { get; }
    public int ProductCategoryId { get; }

    public ProductCreatedEvent(int productId, string productName, int productCategoryId)
    {
        ProductId = productId;
        ProductName = productName;
        ProductCategoryId = productCategoryId;
    }
}

public class ProductUpdatedEvent : BaseDomainEvent
{
    public int ProductId { get; }
    public string OldName { get; }
    public string NewName { get; }

    public ProductUpdatedEvent(int productId, string oldName, string newName)
    {
        ProductId = productId;
        OldName = oldName;
        NewName = newName;
    }
}
