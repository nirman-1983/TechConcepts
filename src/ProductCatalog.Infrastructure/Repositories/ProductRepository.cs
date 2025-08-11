using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Repositories;
using ProductCatalog.Infrastructure.Data;

namespace ProductCatalog.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Product entity
/// </summary>
public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(ContentDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetByProductCategoryIdAsync(int productCategoryId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(t => t.ProductCategoryId == productCategoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByNameAndProductCategoryAsync(string productName, int productCategoryId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(t => t.ProductName == productName && t.ProductCategoryId == productCategoryId, cancellationToken);
    }

    
    public async Task<bool> ExistsByNameAndProductCategoryAsync(string productName, int productCategoryId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AnyAsync(t => t.ProductName == productName && t.ProductCategoryId == productCategoryId, cancellationToken);
    }

    public async Task<bool> ExistsByNameAndProductCategoryAsync(string productName, int productCategoryId, int excludeId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AnyAsync(t => t.ProductName == productName && t.ProductCategoryId == productCategoryId && t.Id != excludeId, cancellationToken);
    }

    public override async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(t => t.ProductCategory)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(t => t.ProductCategory)
            .ToListAsync(cancellationToken);
    }
}
