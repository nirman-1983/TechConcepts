using SharedKernel.Application;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Domain.Repositories;

/// <summary>
/// Repository interface for Product entity
/// </summary>
public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetByProductCategoryIdAsync(int productCategoryId, CancellationToken cancellationToken = default);
    Task<Product?> GetByNameAndProductCategoryAsync(string productName, int productCategoryId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAndProductCategoryAsync(string productName, int productCategoryId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAndProductCategoryAsync(string productName, int productCategoryId, int excludeId, CancellationToken cancellationToken = default);
}
