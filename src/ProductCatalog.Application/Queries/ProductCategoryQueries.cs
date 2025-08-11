using SharedKernel.Application;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Queries;

/// <summary>
/// Query to get all productCategories
/// </summary>
public class GetAllProductCategoriesQuery : IQuery<GetProductCategoriesResponse>
{
    public bool IncludeProducts { get; }

    public GetAllProductCategoriesQuery(bool includeProducts = false)
    {
        IncludeProducts = includeProducts;
    }
}

/// <summary>
/// Query to get a productCategory by ID
/// </summary>
public class GetProductCategoryByIdQuery : IQuery<GetProductCategoryResponse?>
{
    public int Id { get; }
    public bool IncludeProducts { get; }

    public GetProductCategoryByIdQuery(int id, bool includeProducts = false)
    {
        Id = id;
        IncludeProducts = includeProducts;
    }
}

/// <summary>
/// Query to get a productCategory by name
/// </summary>
public class GetProductCategoryByNameQuery : IQuery<GetProductCategoryResponse?>
{
    public string ProductCategoryName { get; }
    public bool IncludeProducts { get; }

    public GetProductCategoryByNameQuery(string productCategoryName, bool includeProducts = false)
    {
        ProductCategoryName = productCategoryName;
        IncludeProducts = includeProducts;
    }
}
