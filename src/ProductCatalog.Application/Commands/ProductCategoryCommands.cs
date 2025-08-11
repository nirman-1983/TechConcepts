using SharedKernel.Application;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Commands;

/// <summary>
/// Command to create a new productCategory
/// </summary>
public class CreateProductCategoryCommand : ICommand<GetProductCategoryResponse>
{
    public string ProductCategoryName { get; }

    public CreateProductCategoryCommand(string productCategoryName)
    {
        ProductCategoryName = productCategoryName;
    }
}

/// <summary>
/// Command to update an existing productCategory
/// </summary>
public class UpdateProductCategoryCommand : ICommand<GetProductCategoryResponse>
{
    public int Id { get; }
    public string ProductCategoryName { get; }

    public UpdateProductCategoryCommand(int id, string productCategoryName)
    {
        Id = id;
        ProductCategoryName = productCategoryName;
    }
}

/// <summary>
/// Command to delete a productCategory
/// </summary>
public class DeleteProductCategoryCommand : ICommand
{
    public int Id { get; }

    public DeleteProductCategoryCommand(int id)
    {
        Id = id;
    }
}
