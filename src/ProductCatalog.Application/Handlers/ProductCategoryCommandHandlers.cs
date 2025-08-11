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
/// Handler for CreateProductCategoryCommand
/// </summary>
public class CreateProductCategoryCommandHandler : ICommandHandler<CreateProductCategoryCommand, GetProductCategoryResponse>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggingService _logger;

    public CreateProductCategoryCommandHandler(
        IProductCategoryRepository productCategoryRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggingService logger)
    {
        _productCategoryRepository = productCategoryRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetProductCategoryResponse> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var logContext = LogContext.Create("CreateProductCategory");
        
        try
        {
            _logger.LogOperationStart<ProductCategory>(logContext, "Creating productCategory with name: {ProductCategoryName}", request.ProductCategoryName);

            // Check for duplicate
            if (await _productCategoryRepository.ExistsByNameAsync(request.ProductCategoryName, cancellationToken))
            {
                throw new DuplicateEntityException(nameof(ProductCategory), nameof(ProductCategory.ProductCategoryName), request.ProductCategoryName);
            }

            // Create domain entity
            var productCategory = new ProductCategory(request.ProductCategoryName);
            
            // Save to repository
            var createdProductCategory = await _productCategoryRepository.AddAsync(productCategory, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogOperationSuccess<ProductCategory>(logContext, "Successfully created productCategory with ID: {ProductCategoryId}", createdProductCategory.Id);

            // Map to response DTO
            return _mapper.Map<GetProductCategoryResponse>(createdProductCategory);
        }
        catch (Exception ex)
        {
            _logger.LogOperationError<ProductCategory>(logContext, ex, "Error creating productCategory");
            throw;
        }
    }
}

/// <summary>
/// Handler for UpdateProductCategoryCommand
/// </summary>
public class UpdateProductCategoryCommandHandler : ICommandHandler<UpdateProductCategoryCommand, GetProductCategoryResponse>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggingService _logger;

    public UpdateProductCategoryCommandHandler(
        IProductCategoryRepository productCategoryRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggingService logger)
    {
        _productCategoryRepository = productCategoryRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetProductCategoryResponse> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var logContext = LogContext.Create("UpdateProductCategory");
        
        try
        {
            _logger.LogOperationStart<ProductCategory>(logContext, "Updating productCategory with ID: {ProductCategoryId}", request.Id);

            // Get existing productCategory
            var productCategory = await _productCategoryRepository.GetByIdAsync(request.Id, cancellationToken);
            if (productCategory == null)
            {
                throw new EntityNotFoundException(nameof(ProductCategory), request.Id);
            }

            // Check for duplicate name (excluding current productCategory)
            if (await _productCategoryRepository.ExistsByNameAsync(request.ProductCategoryName, request.Id, cancellationToken))
            {
                throw new DuplicateEntityException(nameof(ProductCategory), nameof(ProductCategory.ProductCategoryName), request.ProductCategoryName);
            }

            // Update domain entity
            productCategory.UpdateProductCategoryName(request.ProductCategoryName);
            
            // Save changes
            var updatedProductCategory = await _productCategoryRepository.UpdateAsync(productCategory, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogOperationSuccess<ProductCategory>(logContext, "Successfully updated productCategory with ID: {ProductCategoryId}", updatedProductCategory.Id);

            // Map to response DTO
            return _mapper.Map<GetProductCategoryResponse>(updatedProductCategory);
        }
        catch (Exception ex)
        {
            _logger.LogOperationError<ProductCategory>(logContext, ex, "Error updating productCategory");
            throw;
        }
    }
}

/// <summary>
/// Handler for DeleteProductCategoryCommand
/// </summary>
public class DeleteProductCategoryCommandHandler : ICommandHandler<DeleteProductCategoryCommand>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggingService _logger;

    public DeleteProductCategoryCommandHandler(
        IProductCategoryRepository productCategoryRepository,
        IUnitOfWork unitOfWork,
        ILoggingService logger)
    {
        _productCategoryRepository = productCategoryRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var logContext = LogContext.Create("DeleteProductCategory");

        try
        {
            _logger.LogOperationStart<ProductCategory>(logContext, "Deleting productCategory with ID: {ProductCategoryId}", request.Id);

            // Check if productCategory exists
            if (!await _productCategoryRepository.ExistsAsync(request.Id, cancellationToken))
            {
                throw new EntityNotFoundException(nameof(ProductCategory), request.Id);
            }

            // Delete productCategory
            await _productCategoryRepository.DeleteAsync(request.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogOperationSuccess<ProductCategory>(logContext, "Successfully deleted productCategory with ID: {ProductCategoryId}", request.Id);
        }
        catch (Exception ex)
        {
            _logger.LogOperationError<ProductCategory>(logContext, ex, "Error deleting productCategory");
            throw;
        }
    }
}

/// <summary>
/// Handler for GetAllProductCategoriesQuery
/// </summary>
public class GetAllProductCategoriesQueryHandler : IQueryHandler<GetAllProductCategoriesQuery, GetProductCategoriesResponse>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IMapper _mapper;
    private readonly ILoggingService _logger;

    public GetAllProductCategoriesQueryHandler(
        IProductCategoryRepository productCategoryRepository,
        IMapper mapper,
        ILoggingService logger)
    {
        _productCategoryRepository = productCategoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetProductCategoriesResponse> Handle(GetAllProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        var logContext = LogContext.Create("GetAllProductCategories");

        try
        {
            _logger.LogOperationStart<ProductCategory>(logContext, "Retrieving all productCategories");

            var productCategories = request.IncludeProducts
                ? await _productCategoryRepository.GetProductCategoriesWithProductsAsync(cancellationToken)
                : await _productCategoryRepository.GetAllAsync(cancellationToken);

            var productCategoryDtos = _mapper.Map<IEnumerable<GetProductCategoryResponse>>(productCategories);

            _logger.LogOperationSuccess<ProductCategory>(logContext, "Successfully retrieved {Count} productCategories", productCategories.Count());

            return new GetProductCategoriesResponse
            {
                ProductCategories = productCategoryDtos,
                TotalCount = productCategories.Count()
            };
        }
        catch (Exception ex)
        {
            _logger.LogOperationError<ProductCategory>(logContext, ex, "Error retrieving productCategories");
            throw;
        }
    }
}

/// <summary>
/// Handler for GetProductCategoryByIdQuery
/// </summary>
public class GetProductCategoryByIdQueryHandler : IQueryHandler<GetProductCategoryByIdQuery, GetProductCategoryResponse?>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IMapper _mapper;
    private readonly ILoggingService _logger;

    public GetProductCategoryByIdQueryHandler(
        IProductCategoryRepository productCategoryRepository,
        IMapper mapper,
        ILoggingService logger)
    {
        _productCategoryRepository = productCategoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetProductCategoryResponse?> Handle(GetProductCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var logContext = LogContext.Create("GetProductCategoryById");

        try
        {
            _logger.LogOperationStart<ProductCategory>(logContext, "Retrieving productCategory with ID: {ProductCategoryId}", request.Id);

            var productCategory = await _productCategoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (productCategory == null)
            {
                _logger.LogOperationWarning<ProductCategory>(logContext, "ProductCategory with ID {ProductCategoryId} not found", request.Id);
                return null;
            }

            _logger.LogOperationSuccess<ProductCategory>(logContext, "Successfully retrieved productCategory with ID: {ProductCategoryId}", request.Id);

            return _mapper.Map<GetProductCategoryResponse>(productCategory);
        }
        catch (Exception ex)
        {
            _logger.LogOperationError<ProductCategory>(logContext, ex, "Error retrieving productCategory");
            throw;
        }
    }
}
