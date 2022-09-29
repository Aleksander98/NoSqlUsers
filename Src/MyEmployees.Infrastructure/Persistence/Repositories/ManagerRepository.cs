using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Options;
using MyEmployees.Application.Repositories;
using MyEmployees.Domain.Models;
using MyEmployees.Domain.Models.Common;
using MyEmployees.Infrastructure.Persistence.Dtos;
using MyEmployees.Infrastructure.Persistence.Mapping;

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

    public async Task<bool> CreateAsync(Manager manager, CancellationToken cancellationToken = default)
    {
        var managerDto = manager.ToManagerDto();
        var managerDtoAsJson = JsonSerializer.Serialize(managerDto);
        var itemAsAttributes = Document
            .FromJson(managerDtoAsJson)
            .ToAttributeMap();

        var putItemRequest = new PutItemRequest
        {
            TableName = _databaseSettings.Value.UsersTableName,
            Item = itemAsAttributes
        };
        var response = await _dynamoDB.PutItemAsync(putItemRequest, cancellationToken);

        return response.HttpStatusCode == HttpStatusCode.OK;
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
        
        if (response.Item.Count == 0)
        {
            return null;
        }

        var itemAsDocument = Document.FromAttributeMap(response.Item);
        var managerDto = JsonSerializer.Deserialize<ManagerDto>(itemAsDocument.ToJson());

        return managerDto?.ToManager();
    }
}