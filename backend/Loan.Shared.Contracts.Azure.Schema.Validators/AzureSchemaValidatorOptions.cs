namespace Loan.Shared.Contracts.Azure.Schema.Validators;

/// <summary>
/// Configuration options for the Azure Schema Validator
/// </summary>
public class AzureSchemaValidatorOptions
{
    public const string SectionName = "AzureSchemaValidation";

    /// <summary>
    /// The fully qualified namespace for the Schema Registry
    /// </summary>
    public string FullyQualifiedNamespace { get; set; } = string.Empty;

    /// <summary>
    /// The name of the schema group in the Schema Registry
    /// </summary>
    public string SchemaGroup { get; set; } = "loan-schemas";

    /// <summary>
    /// Indicates whether to use DefaultAzureCredential
    /// </summary>
    public bool UseDefaultCredential { get; set; } = true;

    /// <summary>
    /// Client ID for authentication (only used if UseDefaultCredential = false)
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Client secret for authentication (only used if UseDefaultCredential = false)
    /// </summary>
    public string? ClientSecret { get; set; }

    /// <summary>
    /// Tenant ID for authentication (only used if UseDefaultCredential = false)
    /// </summary>
    public string? TenantId { get; set; }

    /// <summary>
    /// Version of the schema to retrieve (defaults to 1)
    /// </summary>
    public int SchemaVersion { get; set; } = 1;
}