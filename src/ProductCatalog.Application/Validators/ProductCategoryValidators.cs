using FluentValidation;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Domain.ValueObjects;

namespace ProductCatalog.Application.Validators;

/// <summary>
/// Validator interface for AddProductCategoryRequest
/// </summary>
public interface IAddProductCategoryRequestValidator : IValidator<AddProductCategoryRequest>
{
}

/// <summary>
/// Validator interface for UpdateProductCategoryRequest
/// </summary>
public interface IUpdateProductCategoryRequestValidator : IValidator<UpdateProductCategoryRequest>
{
}

/// <summary>
/// FluentValidation implementation for AddProductCategoryRequest
/// </summary>
public class AddProductCategoryRequestValidator : AbstractValidator<AddProductCategoryRequest>, IAddProductCategoryRequestValidator
{
    public AddProductCategoryRequestValidator()
    {
        RuleFor(x => x.ProductCategoryName)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("ProductCategory name is required")
            .Must(name => name == null || name.Length <= ProductCategoryName.MaxLength)
            .WithMessage($"ProductCategory name must not exceed {ProductCategoryName.MaxLength} characters")
            .Must(BeValidProductCategoryName)
            .WithMessage("ProductCategory name contains invalid characters");
    }

    private static bool BeValidProductCategoryName(string productCategoryName)
    {
        if (string.IsNullOrWhiteSpace(productCategoryName))
            return true; // Let other rules handle null/empty

        // Only check for invalid characters
        char[] invalidChars = { '>', '<', '&', '"', '\'' };
        return !productCategoryName.Any(c => invalidChars.Contains(c));
    }

    async Task<ValidationResult> IValidator<AddProductCategoryRequest>.ValidateAsync(AddProductCategoryRequest instance, CancellationToken cancellationToken)
    {
        var result = await base.ValidateAsync(instance, cancellationToken);
        var errors = result.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage));
        return result.IsValid ? ValidationResult.Success() : ValidationResult.Failure(errors);
    }
}

/// <summary>
/// FluentValidation implementation for UpdateProductCategoryRequest
/// </summary>
public class UpdateProductCategoryRequestValidator : AbstractValidator<UpdateProductCategoryRequest>, IUpdateProductCategoryRequestValidator
{
    public UpdateProductCategoryRequestValidator()
    {
        RuleFor(x => x.ProductCategoryName)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("ProductCategory name is required")
            .Must(name => name == null || name.Length <= ProductCategoryName.MaxLength)
            .WithMessage($"ProductCategory name must not exceed {ProductCategoryName.MaxLength} characters")
            .Must(BeValidProductCategoryName)
            .WithMessage("ProductCategory name contains invalid characters");
    }

    private static bool BeValidProductCategoryName(string productCategoryName)
    {
        if (string.IsNullOrWhiteSpace(productCategoryName))
            return true; // Let other rules handle null/empty

        // Only check for invalid characters
        char[] invalidChars = { '>', '<', '&', '"', '\'' };
        return !productCategoryName.Any(c => invalidChars.Contains(c));
    }



    async Task<ValidationResult> IValidator<UpdateProductCategoryRequest>.ValidateAsync(UpdateProductCategoryRequest instance, CancellationToken cancellationToken)
    {
        var result = await base.ValidateAsync(instance, cancellationToken);
        var errors = result.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage));
        return result.IsValid ? ValidationResult.Success() : ValidationResult.Failure(errors);
    }
}
