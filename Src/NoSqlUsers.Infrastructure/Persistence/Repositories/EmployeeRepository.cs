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

public sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly IAmazonDynamoDB _dynamoDB;
    private readonly IOptions<DatabaseSettings> _databaseSettings;

    public EmployeeRepository(IAmazonDynamoDB dynamoDB, IOptions<DatabaseSettings> dynamoDBSettings)
    {
        _dynamoDB = dynamoDB ?? throw new ArgumentNullException(nameof(dynamoDB));
        _databaseSettings = dynamoDBSettings ?? throw new ArgumentNullException(nameof(dynamoDBSettings));
    }

    public Task<bool> CreateAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        return UpdateAsync(employee, cancellationToken);
    }

    public async Task<Employee?> GetByUsernameAsync(Username username, CancellationToken cancellationToken = default)
    {
        var getItemRequest = new GetItemRequest
        {
            TableName = _databaseSettings.Value.UsersTableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue(DatabaseSettings.EmployeesPrefix + username.Value) },
                { "sk", new AttributeValue(DatabaseSettings.EmployeesPrefix + username.Value) }
            }
        };
        var response = await _dynamoDB.GetItemAsync(getItemRequest, cancellationToken);

        if (response.Item.Count == 0)
        {
            return null;
        }

        var itemAsDocument = Document.FromAttributeMap(response.Item);
        var employeeDto = JsonSerializer.Deserialize<EmployeeDto>(itemAsDocument.ToJson());

        return employeeDto?.ToEmployee();
    }

    public async Task<bool> UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        var putItemRequest = new PutItemRequest
        {
            TableName = _databaseSettings.Value.UsersTableName,
            Item = CreatePutItemAttributes(employee)
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
                { "pk", new AttributeValue(DatabaseSettings.EmployeesPrefix + username.Value) },
                { "sk", new AttributeValue(DatabaseSettings.EmployeesPrefix + username.Value) }
            },
            ReturnValues = ReturnValue.ALL_OLD
        };

        var response = await _dynamoDB.DeleteItemAsync(deleteItemRequest, cancellationToken);
        
        return response.HttpStatusCode == HttpStatusCode.OK && response.Attributes.Count > 0;
    }

    private static Dictionary<string, AttributeValue> CreatePutItemAttributes(Employee employee)
    {
        var employeeDto = employee.ToEmployeeDto();
        var employeeDtoAsJson = JsonSerializer.Serialize(employeeDto);
        var itemAsAttributes = Document
            .FromJson(employeeDtoAsJson)
            .ToAttributeMap();

        return itemAsAttributes;
    }
}