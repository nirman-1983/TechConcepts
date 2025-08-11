using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Commands;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Queries;
using ProductCatalog.Application.Validators;

namespace ProductCatalog.API.Controllers;

/// <summary>
/// API Controller for Product management
/// </summary>
[ApiController]
[Route("api/catalog/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAddProductRequestValidator _addProductValidator;
    private readonly IUpdateProductRequestValidator _updateProductValidator;

    public ProductsController(
        IMediator mediator,
        IAddProductRequestValidator addProductValidator,
        IUpdateProductRequestValidator updateProductValidator)
    {
        _mediator = mediator;
        _addProductValidator = addProductValidator;
        _updateProductValidator = updateProductValidator;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<GetProductsResponse>> GetProducts()
    {
        var query = new GetAllProductsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<GetProductResponse>> GetProduct(int id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (result == null)
        {
            return NotFound($"Product with ID {id} not found");
        }

        return Ok(result);
    }

    /// <summary>
    /// Get products by productCategory ID
    /// </summary>
    [HttpGet("by-productCategory/{productCategoryId}")]
    public async Task<ActionResult<GetProductsResponse>> GetProductsByProductCategory(int productCategoryId)
    {
        var query = new GetProductsByProductCategoryIdQuery(productCategoryId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<GetProductResponse>> CreateProduct([FromBody] AddProductRequest request)
    {
        // Validate request
        var validationResult = await _addProductValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = new CreateProductCommand(request.ProductName, request.ProductCategoryId);
        var result = await _mediator.Send(command);
        
        return CreatedAtAction(nameof(GetProduct), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<GetProductResponse>> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
    {
        // Validate request
        var validationResult = await _updateProductValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = new UpdateProductCommand(id, request.ProductName, request.ProductCategoryId);
        var result = await _mediator.Send(command);
        
        return Ok(result);
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var command = new DeleteProductCommand(id);
        await _mediator.Send(command);
        
        return NoContent();
    }
}
