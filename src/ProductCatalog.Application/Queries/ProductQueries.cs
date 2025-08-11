using SharedKernel.Application;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Queries;

/// <summary>
/// Query to get all products
/// </summary>
public class GetAllProductsQuery : IQuery<GetProductsResponse>
{

    public GetAllProductsQuery()
    {
    }
}

/// <summary>
/// Query to get a product by ID
/// </summary>
public class GetProductByIdQuery : IQuery<GetProductResponse?>
{
    public int Id { get; }

    public GetProductByIdQuery(int id)
    {
        Id = id;
    }
}

/// <summary>
/// Query to get products by productCategory ID
/// </summary>
public class GetProductsByProductCategoryIdQuery : IQuery<GetProductsResponse>
{
    public int ProductCategoryId { get; }

    public GetProductsByProductCategoryIdQuery(int productCategoryId)
    {
        ProductCategoryId = productCategoryId;
    }
}

/// <summary>
/// Query to get a product by name and productCategory
/// </summary>
public class GetProductByNameAndProductCategoryQuery : IQuery<GetProductResponse?>
{
    public string ProductName { get; }
    public int ProductCategoryId { get; }

    public GetProductByNameAndProductCategoryQuery(string productName, int productCategoryId)
    {
        ProductName = productName;
        ProductCategoryId = productCategoryId;
    }
}
