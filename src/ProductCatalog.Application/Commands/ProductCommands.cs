using SharedKernel.Application;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Commands;

/// <summary>
/// Command to create a new product
/// </summary>
public class CreateProductCommand : ICommand<GetProductResponse>
{
    public string ProductName { get; }
    public int ProductCategoryId { get; }

    public CreateProductCommand(string productName, int productCategoryId)
    {
        ProductName = productName;
        ProductCategoryId = productCategoryId;
    }
}

/// <summary>
/// Command to update an existing product
/// </summary>
public class UpdateProductCommand : ICommand<GetProductResponse>
{
    public int Id { get; }
    public string ProductName { get; }
    public int ProductCategoryId { get; }

    public UpdateProductCommand(int id, string productName, int productCategoryId)
    {
        Id = id;
        ProductName = productName;
        ProductCategoryId = productCategoryId;
    }
}

/// <summary>
/// Command to delete a product
/// </summary>
public class DeleteProductCommand : ICommand
{
    public int Id { get; }

    public DeleteProductCommand(int id)
    {
        Id = id;
    }
}
