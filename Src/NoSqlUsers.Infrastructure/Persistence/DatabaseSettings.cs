namespace MyEmployees.Infrastructure.Persistence;

public sealed record DatabaseSettings
{
    public const string KeyName = "DynamoDB";
    
    public const string EmployeesPrefix = "EMPL#";

    public const string ManagersPrefix = "MNGR#";

    public string UsersTableName { get; init; } = "Users";

    public string PageTokenSecretKey { get; init; } = "SecretKey";
}