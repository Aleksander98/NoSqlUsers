namespace MyEmployees.WebApi.Contracts.Responses;

[Serializable]
public sealed class EmployeeResponse :  UserResponse
{
    public string? ManagerUsername { get; set; }
}