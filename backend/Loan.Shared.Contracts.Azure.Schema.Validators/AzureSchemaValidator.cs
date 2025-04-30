using Azure.Data.SchemaRegistry;
using Azure.Identity;
using Loan.Shared.Contract.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Text.Json;

namespace Loan.Shared.Contracts.Azure.Schema.Validators;


/// <summary>
/// Implementation of ISchemaValidator that uses Azure Schema Registry for schema validation
/// </summary>
public class AzureSchemaValidator : ISchemaValidator
{
    private readonly Dictionary<Type, string> _schemaCache = [];
    private readonly SchemaRegistryClient _schemaRegistryClient;
    private readonly ILogger<AzureSchemaValidator> _logger;
    private readonly AzureSchemaValidatorOptions _options;

    /// <summary>
    /// Initializes a new instance of the AzureSchemaValidator class
    /// </summary>
    /// <param name="options">Configuration options for the validator</param>
    /// <param name="logger">Logger for diagnostic information</param>
    public AzureSchemaValidator(IOptions<AzureSchemaValidatorOptions> options, ILogger<AzureSchemaValidator> logger)
    {
        _options = options.Value;
        _logger = logger;

        if (string.IsNullOrEmpty(_options.FullyQualifiedNamespace))
        {
            throw new ArgumentException("FullyQualifiedNamespace is required in AzureSchemaValidatorOptions");
        }

        // Configure the client based on options
        if (_options.UseDefaultCredential)
        {
            _schemaRegistryClient = new SchemaRegistryClient(
                _options.FullyQualifiedNamespace,
                new DefaultAzureCredential());
        }
        else
        {
            if (string.IsNullOrEmpty(_options.ClientId) ||
                string.IsNullOrEmpty(_options.ClientSecret) ||
                string.IsNullOrEmpty(_options.TenantId))
            {
                throw new ArgumentException("ClientId, ClientSecret, and TenantId are required when UseDefaultCredential is false");
            }

            _schemaRegistryClient = new SchemaRegistryClient(
                _options.FullyQualifiedNamespace,
                new ClientSecretCredential(_options.TenantId, _options.ClientId, _options.ClientSecret));
        }
    }

    /// <summary>
    /// Validates a message against its schema from Azure Schema Registry
    /// </summary>
    /// <typeparam name="T">The type of message to validate</typeparam>
    /// <param name="message">The message instance to validate</param>
    /// <returns>True if the message is valid, false otherwise</returns>
    public async Task<bool> ValidateAsync<T>(T message)
    {
        try
        {
            var jsonMessage = JsonSerializer.Serialize(message);
            var schema = await GetSchemaForTypeAsync<T>();
            var jSchema = JSchema.Parse(schema);
            var json = JToken.Parse(jsonMessage);
            var isValid = json.IsValid(jSchema, out IList<string> errors);

            if (!isValid)
            {
                foreach (var error in errors)
                {
                    _logger.LogError("Validation error: {Error}", error);
                }
            }

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating message of type {Type}", typeof(T).Name);
            return false;
        }
    }

    /// <summary>
    /// Retrieves the schema for a given type from Azure Schema Registry
    /// </summary>
    /// <typeparam name="T">The type to get the schema for</typeparam>
    /// <returns>The schema definition as a string</returns>
    private async Task<string> GetSchemaForTypeAsync<T>()
    {
        Type type = typeof(T);

        if (_schemaCache.TryGetValue(type, out string? cachedSchema))
        {
            return cachedSchema;
        }

        try
        {
            string schemaName = type.Name;

            var response = await _schemaRegistryClient.GetSchemaAsync(
                _options.SchemaGroup,
                schemaName,
                _options.SchemaVersion,
                CancellationToken.None);

            var schema = response.Value.Definition;
            _schemaCache[type] = schema;

            return schema;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving schema for type {Type}", type.Name);
            throw;
        }
    }
}