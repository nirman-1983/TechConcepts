namespace ProductCatalog.Application.DTOs;

/// <summary>
/// Request DTO for adding a new product
/// </summary>
public class AddProductRequest
{
    public string ProductName { get; set; } = string.Empty;
    public int ProductCategoryId { get; set; }
}

/// <summary>
/// Request DTO for updating an existing product
/// </summary>
public class UpdateProductRequest
{
    public string ProductName { get; set; } = string.Empty;
    public int ProductCategoryId { get; set; }
}

/// <summary>
/// Response DTO for product queries
/// </summary>
public class GetProductResponse
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int ProductCategoryId { get; set; }
    public string ProductCategoryName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
}

/// <summary>
/// Response DTO for products list queries
/// </summary>
public class GetProductsResponse
{
    public IEnumerable<GetProductResponse> Products { get; set; } = Enumerable.Empty<GetProductResponse>();
    public int TotalCount { get; set; }
}
