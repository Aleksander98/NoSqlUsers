using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Options;
using MyEmployees.Application.Models;
using MyEmployees.Application.Repositories;
using MyEmployees.Domain.Models;
using MyEmployees.Domain.Models.Common;
using MyEmployees.Infrastructure.Persistence.Dtos;
using MyEmployees.Infrastructure.Persistence.Mapping;
using MyEmployees.Infrastructure.Persistence.Pagination;

namespace MyEmployees.Infrastructure.Persistence.Repositories;

public sealed class ManagerRepository : IManagerRepository
{
    private readonly IAmazonDynamoDB _dynamoDB;
    private readonly IOptions<DatabaseSettings> _databaseSettings;

    public ManagerRepository(IAmazonDynamoDB dynamoDB, IOptions<DatabaseSettings> dynamoDBSettings)
    {
        _dynamoDB = dynamoDB ?? throw new ArgumentNullException(nameof(dynamoDB));
        _databaseSettings = dynamoDBSettings ?? throw new ArgumentNullException(nameof(dynamoDBSettings));
    }

    public Task<bool> CreateAsync(Manager manager, CancellationToken cancellationToken = default)
    {
        return UpdateAsync(manager, cancellationToken);
    }
    
    public async Task<Manager?> GetByUsernameAsync(Username username, CancellationToken cancellationToken = default)
    {
        var getItemRequest = new GetItemRequest
        {
            TableName = _databaseSettings.Value.UsersTableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue(DatabaseSettings.ManagersPrefix + username.Value) },
                { "sk", new AttributeValue(DatabaseSettings.ManagersPrefix + username.Value) }
            }
        };
        var response = await _dynamoDB.GetItemAsync(getItemRequest, cancellationToken);

        return response.Item.Count == 0 ? null : CreateManagerFromAttributeMap(response.Item);
    }

    public async Task<PaginatedList<Manager>> GetAllWithPaginationAsync(int pageSize, string? pageToken,
        CancellationToken cancellationToken = default)
    {
        var scanRequest = new ScanRequest
        {
            TableName = _databaseSettings.Value.UsersTableName,
            FilterExpression = "begins_with(pk, :prefix)",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":prefix", new AttributeValue(DatabaseSettings.ManagersPrefix) }
            },
            ExclusiveStartKey = PageTokenEncryptor.Decrypt(pageToken, _databaseSettings.Value.PageTokenSecretKey),
            Limit = pageSize
        };

        var response = await _dynamoDB.ScanAsync(scanRequest, cancellationToken);

        var nextPageToken = PageTokenEncryptor
            .Encrypt(response.LastEvaluatedKey, _databaseSettings.Value.PageTokenSecretKey);
        var items = response.Items
            .Select(CreateManagerFromAttributeMap)
            .ToList();

        return new PaginatedList<Manager>(items, pageSize, pageToken, nextPageToken);
    }

    public async Task<bool> UpdateAsync(Manager manager, CancellationToken cancellationToken = default)
    {
        var putItemRequest = new PutItemRequest
        {
            TableName = _databaseSettings.Value.UsersTableName,
            Item = CreateAttributeMapFromManager(manager)
        };
        var response = await _dynamoDB.PutItemAsync(putItemRequest, cancellationToken);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> DeleteAsync(Username username, CancellationToken cancellationToken = default)
    {
        var deleteItemRequest = new DeleteItemRequest
        {
            TableName = _databaseSettings.Value.UsersTableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue(DatabaseSettings.ManagersPrefix + username.Value) },
                { "sk", new AttributeValue(DatabaseSettings.ManagersPrefix + username.Value) }
            },
            ReturnValues = ReturnValue.ALL_OLD
        };

        var response = await _dynamoDB.DeleteItemAsync(deleteItemRequest, cancellationToken);

        return response.HttpStatusCode == HttpStatusCode.OK && response.Attributes.Count > 0;
    }
    
    private static Manager CreateManagerFromAttributeMap(Dictionary<string, AttributeValue> attributeMap)
    {
        var managerDtoAsJson = Document
            .FromAttributeMap(attributeMap)
            .ToJson();
        
        var managerDto = JsonSerializer.Deserialize<ManagerDto>(managerDtoAsJson);

        return managerDto!.ToManager();
    }

    private static Dictionary<string, AttributeValue> CreateAttributeMapFromManager(Manager manager)
    {
        var managerDto = manager.ToManagerDto();
        var managerDtoAsJson = JsonSerializer.Serialize(managerDto);
        var itemAsAttributes = Document
            .FromJson(managerDtoAsJson)
            .ToAttributeMap();

        return itemAsAttributes;
    }
}