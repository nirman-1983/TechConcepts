using SharedKernel.Exceptions;

namespace ProductCatalog.Domain.ValueObjects;

/// <summary>
/// Value object representing a productCategory name with validation rules
/// </summary>
public class ProductCategoryName : ValueObject
{
    public const int MaxLength = 200;
    private static readonly char[] InvalidCharacters = { '>', '<', '&', '"', '\'' };

    public string Value { get; private set; }

    private ProductCategoryName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new ProductCategoryName with validation
    /// </summary>
    /// <param name="value">The productCategory name value</param>
    /// <returns>A valid ProductCategoryName instance</returns>
    /// <exception cref="DomainValidationException">Thrown when validation fails</exception>
    public static ProductCategoryName Create(string value)
    {
        ValidateProductCategoryName(value);
        return new ProductCategoryName(value.Trim());
    }

    /// <summary>
    /// Validates a productCategory name value
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <exception cref="DomainValidationException">Thrown when validation fails</exception>
    private static void ValidateProductCategoryName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException("ProductCategory name cannot be empty");
        }

        if (value.Length > MaxLength)
        {
            throw new DomainValidationException("ProductCategory name cannot exceed {0} characters", MaxLength);
        }

        if (ContainsInvalidCharacters(value))
        {
            throw new DomainValidationException("ProductCategory name contains invalid characters");
        }
    }

    /// <summary>
    /// Checks if the value contains invalid characters
    /// </summary>
    private static bool ContainsInvalidCharacters(string input)
    {
        return input.Any(c => InvalidCharacters.Contains(c));
    }

    /// <summary>
    /// Tries to create a ProductCategoryName without throwing exceptions
    /// </summary>
    /// <param name="value">The productCategory name value</param>
    /// <param name="productCategoryName">The created ProductCategoryName if successful</param>
    /// <returns>True if creation was successful, false otherwise</returns>
    public static bool TryCreate(string value, out ProductCategoryName? productCategoryName)
    {
        try
        {
            productCategoryName = Create(value);
            return true;
        }
        catch (DomainValidationException)
        {
            productCategoryName = null;
            return false;
        }
    }

    /// <summary>
    /// Implicit conversion from ProductCategoryName to string
    /// </summary>
    public static implicit operator string(ProductCategoryName productCategoryName)
    {
        return productCategoryName.Value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }
}
