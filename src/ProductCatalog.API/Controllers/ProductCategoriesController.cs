using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Commands;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Queries;
using ProductCatalog.Application.Validators;

namespace ProductCatalog.API.Controllers;

/// <summary>
/// API Controller for ProductCategory management
/// </summary>
[ApiController]
[Route("api/content/productCategories")]
public class ProductCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAddProductCategoryRequestValidator _addProductCategoryValidator;
    private readonly IUpdateProductCategoryRequestValidator _updateProductCategoryValidator;

    public ProductCategoriesController(
        IMediator mediator,
        IAddProductCategoryRequestValidator addProductCategoryValidator,
        IUpdateProductCategoryRequestValidator updateProductCategoryValidator)
    {
        _mediator = mediator;
        _addProductCategoryValidator = addProductCategoryValidator;
        _updateProductCategoryValidator = updateProductCategoryValidator;
    }

    /// <summary>
    /// Get all productCategories
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<GetProductCategoriesResponse>> GetProductCategories([FromQuery] bool includeProducts = false)
    {
        var query = new GetAllProductCategoriesQuery(includeProducts);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get productCategory by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<GetProductCategoryResponse>> GetProductCategory(int id, [FromQuery] bool includeProducts = false)
    {
        var query = new GetProductCategoryByIdQuery(id, includeProducts);
        var result = await _mediator.Send(query);
        
        if (result == null)
        {
            return NotFound($"ProductCategory with ID {id} not found");
        }

        return Ok(result);
    }

    /// <summary>
    /// Create a new productCategory
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<GetProductCategoryResponse>> CreateProductCategory([FromBody] AddProductCategoryRequest request)
    {
        // Validate request
        var validationResult = await _addProductCategoryValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = new CreateProductCategoryCommand(request.ProductCategoryName);
        var result = await _mediator.Send(command);
        
        return CreatedAtAction(nameof(GetProductCategory), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update an existing productCategory
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<GetProductCategoryResponse>> UpdateProductCategory(int id, [FromBody] UpdateProductCategoryRequest request)
    {
        // Validate request
        var validationResult = await _updateProductCategoryValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = new UpdateProductCategoryCommand(id, request.ProductCategoryName);
        var result = await _mediator.Send(command);
        
        return Ok(result);
    }

    /// <summary>
    /// Delete a productCategory
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProductCategory(int id)
    {
        var command = new DeleteProductCategoryCommand(id);
        await _mediator.Send(command);
        
        return NoContent();
    }
}
