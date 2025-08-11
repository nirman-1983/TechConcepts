namespace ProductCatalog.Application.DTOs;

/// <summary>
/// Request DTO for adding a new productCategory
/// </summary>
public class AddProductCategoryRequest
{
    public string ProductCategoryName { get; set; } = string.Empty;
}

/// <summary>
/// Request DTO for updating an existing productCategory
/// </summary>
public class UpdateProductCategoryRequest
{
    public string ProductCategoryName { get; set; } = string.Empty;
}

/// <summary>
/// Response DTO for productCategory queries
/// </summary>
public class GetProductCategoryResponse
{
    public int Id { get; set; }
    public string ProductCategoryName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public ICollection<GetProductResponse> Products { get; set; } = new List<GetProductResponse>();
}

/// <summary>
/// Response DTO for productCategories list queries
/// </summary>
public class GetProductCategoriesResponse
{
    public IEnumerable<GetProductCategoryResponse> ProductCategories { get; set; } = Enumerable.Empty<GetProductCategoryResponse>();
    public int TotalCount { get; set; }
}
