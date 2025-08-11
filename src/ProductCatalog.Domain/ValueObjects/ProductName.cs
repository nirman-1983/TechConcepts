using SharedKernel.Exceptions;

namespace ProductCatalog.Domain.ValueObjects;

/// <summary>
/// Value object representing a product name with validation rules
/// </summary>
public class ProductName : ValueObject
{
    public const int MaxLength = 200;
    private static readonly char[] InvalidCharacters = { '>', '<', '&', '"', '\'' };

    public string Value { get; private set; }

    private ProductName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new ProductName with validation
    /// </summary>
    /// <param name="value">The product name value</param>
    /// <returns>A valid ProductName instance</returns>
    /// <exception cref="DomainValidationException">Thrown when validation fails</exception>
    public static ProductName Create(string value)
    {
        ValidateProductName(value);
        return new ProductName(value.Trim());
    }

    /// <summary>
    /// Validates a product name value
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <exception cref="DomainValidationException">Thrown when validation fails</exception>
    private static void ValidateProductName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException("Product name cannot be empty");
        }

        if (value.Length > MaxLength)
        {
            throw new DomainValidationException("Product name cannot exceed {0} characters", MaxLength);
        }

        if (ContainsInvalidCharacters(value))
        {
            throw new DomainValidationException("Product name contains invalid characters");
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
    /// Tries to create a ProductName without throwing exceptions
    /// </summary>
    /// <param name="value">The product name value</param>
    /// <param name="productName">The created ProductName if successful</param>
    /// <returns>True if creation was successful, false otherwise</returns>
    public static bool TryCreate(string value, out ProductName? productName)
    {
        try
        {
            productName = Create(value);
            return true;
        }
        catch (DomainValidationException)
        {
            productName = null;
            return false;
        }
    }

    /// <summary>
    /// Implicit conversion from ProductName to string
    /// </summary>
    public static implicit operator string(ProductName productName)
    {
        return productName.Value;
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
