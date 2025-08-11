using SharedKernel.Application;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Domain.Repositories;

/// <summary>
/// Repository interface for ProductCategory aggregate
/// </summary>
public interface IProductCategoryRepository : IRepository<ProductCategory>
{
    Task<ProductCategory?> GetByNameAsync(string productCategoryName, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductCategory>> GetProductCategoriesWithProductsAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string productCategoryName, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string productCategoryName, int excludeId, CancellationToken cancellationToken = default);
}
