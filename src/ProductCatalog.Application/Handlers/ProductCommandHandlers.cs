using AutoMapper;
using SharedKernel.Application;
using SharedKernel.Domain.Exceptions;
using SharedKernel.Infrastructure.Logging;
using ProductCatalog.Application.Commands;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Repositories;

namespace ProductCatalog.Application.Handlers;

/// <summary>
/// Handler for CreateProductCommand
/// </summary>
public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, GetProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggingService _logger;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IProductCategoryRepository productCategoryRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggingService logger)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var logContext = LogContext.Create("CreateProduct");
        
        try
        {
            _logger.LogOperationStart<Product>(logContext, "Creating product with name: {ProductName} for productCategory: {ProductCategoryId}", 
                request.ProductName, request.ProductCategoryId);

            // Check if productCategory exists
            if (!await _productCategoryRepository.ExistsAsync(request.ProductCategoryId, cancellationToken))
            {
                throw new EntityNotFoundException(nameof(ProductCategory), request.ProductCategoryId);
            }

            // Check for duplicate product name within the same productCategory
            if (await _productRepository.ExistsByNameAndProductCategoryAsync(request.ProductName, request.ProductCategoryId, cancellationToken))
            {
                throw new DuplicateEntityException(nameof(Product), nameof(Product.ProductName), request.ProductName);
            }

            // Create domain entity
            var product = new Product(request.ProductName, request.ProductCategoryId);
            
            // Save to repository
            var createdProduct = await _productRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogOperationSuccess<Product>(logContext, "Successfully created product with ID: {ProductId}", createdProduct.Id);

            // Map to response DTO
            return _mapper.Map<GetProductResponse>(createdProduct);
        }
        catch (Exception ex)
        {
            _logger.LogOperationError<Product>(logContext, ex, "Error creating product");
            throw;
        }
    }
}

/// <summary>
/// Handler for UpdateProductCommand
/// </summary>
public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, GetProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggingService _logger;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IProductCategoryRepository productCategoryRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggingService logger)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var logContext = LogContext.Create("UpdateProduct");
        
        try
        {
            _logger.LogOperationStart<Product>(logContext, "Updating product with ID: {ProductId}", request.Id);

            // Get existing product
            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
            {
                throw new EntityNotFoundException(nameof(Product), request.Id);
            }

            // Check if productCategory exists
            if (!await _productCategoryRepository.ExistsAsync(request.ProductCategoryId, cancellationToken))
            {
                throw new EntityNotFoundException(nameof(ProductCategory), request.ProductCategoryId);
            }

            // Check for duplicate name within the same productCategory (excluding current product)
            if (await _productRepository.ExistsByNameAndProductCategoryAsync(request.ProductName, request.ProductCategoryId, request.Id, cancellationToken))
            {
                throw new DuplicateEntityException(nameof(Product), nameof(Product.ProductName), request.ProductName);
            }

            // Update domain entity
            product.UpdateProductName(request.ProductName);
            
            // Save changes
            var updatedProduct = await _productRepository.UpdateAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogOperationSuccess<Product>(logContext, "Successfully updated product with ID: {ProductId}", updatedProduct.Id);

            // Map to response DTO
            return _mapper.Map<GetProductResponse>(updatedProduct);
        }
        catch (Exception ex)
        {
            _logger.LogOperationError<Product>(logContext, ex, "Error updating product");
            throw;
        }
    }
}

/// <summary>
/// Handler for DeleteProductCommand
/// </summary>
public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggingService _logger;

    public DeleteProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        ILoggingService logger)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var logContext = LogContext.Create("DeleteProduct");
        
        try
        {
            _logger.LogOperationStart<Product>(logContext, "Deleting product with ID: {ProductId}", request.Id);

            // Check if product exists
            if (!await _productRepository.ExistsAsync(request.Id, cancellationToken))
            {
                throw new EntityNotFoundException(nameof(Product), request.Id);
            }

            // Delete product
            await _productRepository.DeleteAsync(request.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogOperationSuccess<Product>(logContext, "Successfully deleted product with ID: {ProductId}", request.Id);
        }
        catch (Exception ex)
        {
            _logger.LogOperationError<Product>(logContext, ex, "Error deleting product");
            throw;
        }
    }
}

/// <summary>
/// Handler for GetAllProductsQuery
/// </summary>
public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, GetProductsResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILoggingService _logger;

    public GetAllProductsQueryHandler(
        IProductRepository productRepository,
        IMapper mapper,
        ILoggingService logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetProductsResponse> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var logContext = LogContext.Create("GetAllProducts");

        try
        {
            _logger.LogOperationStart<Product>(logContext, "Retrieving all products");

            var products = await _productRepository.GetAllAsync(cancellationToken);

            var productDtos = _mapper.Map<IEnumerable<GetProductResponse>>(products);

            _logger.LogOperationSuccess<Product>(logContext, "Successfully retrieved {Count} products", products.Count());

            return new GetProductsResponse
            {
                Products = productDtos,
                TotalCount = products.Count()
            };
        }
        catch (Exception ex)
        {
            _logger.LogOperationError<Product>(logContext, ex, "Error retrieving products");
            throw;
        }
    }
}

/// <summary>
/// Handler for GetProductByIdQuery
/// </summary>
public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, GetProductResponse?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILoggingService _logger;

    public GetProductByIdQueryHandler(
        IProductRepository productRepository,
        IMapper mapper,
        ILoggingService logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetProductResponse?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var logContext = LogContext.Create("GetProductById");

        try
        {
            _logger.LogOperationStart<Product>(logContext, "Retrieving product with ID: {ProductId}", request.Id);

            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

            if (product == null)
            {
                _logger.LogOperationWarning<Product>(logContext, "Product with ID {ProductId} not found", request.Id);
                return null;
            }

            _logger.LogOperationSuccess<Product>(logContext, "Successfully retrieved product with ID: {ProductId}", request.Id);

            return _mapper.Map<GetProductResponse>(product);
        }
        catch (Exception ex)
        {
            _logger.LogOperationError<Product>(logContext, ex, "Error retrieving product");
            throw;
        }
    }
}

/// <summary>
/// Handler for GetProductsByProductCategoryIdQuery
/// </summary>
public class GetProductsByProductCategoryIdQueryHandler : IQueryHandler<GetProductsByProductCategoryIdQuery, GetProductsResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILoggingService _logger;

    public GetProductsByProductCategoryIdQueryHandler(
        IProductRepository productRepository,
        IMapper mapper,
        ILoggingService logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetProductsResponse> Handle(GetProductsByProductCategoryIdQuery request, CancellationToken cancellationToken)
    {
        var logContext = LogContext.Create("GetProductsByProductCategoryId");

        try
        {
            _logger.LogOperationStart<Product>(logContext, "Retrieving products for productCategory ID: {ProductCategoryId}", request.ProductCategoryId);

            var products = await _productRepository.GetByProductCategoryIdAsync(request.ProductCategoryId, cancellationToken);

            var productDtos = _mapper.Map<IEnumerable<GetProductResponse>>(products);

            _logger.LogOperationSuccess<Product>(logContext, "Successfully retrieved {Count} products for productCategory ID: {ProductCategoryId}",
                products.Count(), request.ProductCategoryId);

            return new GetProductsResponse
            {
                Products = productDtos,
                TotalCount = products.Count()
            };
        }
        catch (Exception ex)
        {
            _logger.LogOperationError<Product>(logContext, ex, "Error retrieving products for productCategory ID: {ProductCategoryId}", request.ProductCategoryId);
            throw;
        }
    }
}
