using FluentValidation;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Domain.ValueObjects;

namespace ProductCatalog.Application.Validators;

/// <summary>
/// Validator interface for AddProductRequest
/// </summary>
public interface IAddProductRequestValidator : IValidator<AddProductRequest>
{
}

/// <summary>
/// Validator interface for UpdateProductRequest
/// </summary>
public interface IUpdateProductRequestValidator : IValidator<UpdateProductRequest>
{
}

/// <summary>
/// FluentValidation implementation for AddProductRequest
/// </summary>
public class AddProductRequestValidator : AbstractValidator<AddProductRequest>, IAddProductRequestValidator
{
    public AddProductRequestValidator()
    {
        RuleFor(x => x.ProductName)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Product name is required")
            .Must(name => name == null || name.Length <= ProductName.MaxLength)
            .WithMessage($"Product name must not exceed {ProductName.MaxLength} characters")
            .Must(BeValidProductName)
            .WithMessage("Product name contains invalid characters");

        RuleFor(x => x.ProductCategoryId)
            .GreaterThan(0)
            .WithMessage("ProductCategory ID must be greater than 0");
    }

    private static bool BeValidProductName(string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
            return true; // Let other rules handle null/empty

        // Only check for invalid characters
        char[] invalidChars = { '>', '<', '&', '"', '\'' };
        return !productName.Any(c => invalidChars.Contains(c));
    }

    async Task<ValidationResult> IValidator<AddProductRequest>.ValidateAsync(AddProductRequest instance, CancellationToken cancellationToken)
    {
        var result = await base.ValidateAsync(instance, cancellationToken);
        var errors = result.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage));
        return result.IsValid ? ValidationResult.Success() : ValidationResult.Failure(errors);
    }
}

/// <summary>
/// FluentValidation implementation for UpdateProductRequest
/// </summary>
public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>, IUpdateProductRequestValidator
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.ProductName)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Product name is required")
            .Must(name => name == null || name.Length <= ProductName.MaxLength)
            .WithMessage($"Product name must not exceed {ProductName.MaxLength} characters")
            .Must(BeValidProductName)
            .WithMessage("Product name contains invalid characters");

        RuleFor(x => x.ProductCategoryId)
            .GreaterThan(0)
            .WithMessage("ProductCategory ID must be greater than 0");
    }

    private static bool BeValidProductName(string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
            return true; // Let other rules handle null/empty

        // Only check for invalid characters
        char[] invalidChars = { '>', '<', '&', '"', '\'' };
        return !productName.Any(c => invalidChars.Contains(c));
    }



    async Task<ValidationResult> IValidator<UpdateProductRequest>.ValidateAsync(UpdateProductRequest instance, CancellationToken cancellationToken)
    {
        var result = await base.ValidateAsync(instance, cancellationToken);
        var errors = result.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage));
        return result.IsValid ? ValidationResult.Success() : ValidationResult.Failure(errors);
    }
}
