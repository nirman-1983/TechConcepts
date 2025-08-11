using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Repositories;
using ProductCatalog.Infrastructure.Data;

namespace ProductCatalog.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for ProductCategory aggregate
/// </summary>
public class ProductCategoryRepository : BaseRepository<ProductCategory>, IProductCategoryRepository
{
    public ProductCategoryRepository(ContentDbContext context) : base(context)
    {
    }

    public async Task<ProductCategory?> GetByNameAsync(string productCategoryName, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(s => s.ProductCategoryName == productCategoryName, cancellationToken);
    }

    public async Task<IEnumerable<ProductCategory>> GetProductCategoriesWithProductsAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(s => s.Products)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string productCategoryName, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AnyAsync(s => s.ProductCategoryName == productCategoryName, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string productCategoryName, int excludeId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AnyAsync(s => s.ProductCategoryName == productCategoryName && s.Id != excludeId, cancellationToken);
    }

    public override async Task<ProductCategory?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<ProductCategory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(s => s.Products)
            .ToListAsync(cancellationToken);
    }
}
