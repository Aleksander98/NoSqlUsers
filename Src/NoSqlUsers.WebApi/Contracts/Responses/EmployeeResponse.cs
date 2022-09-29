namespace MyEmployees.WebApi.Contracts.Responses;

public sealed class EmployeeResponse :  UserResponse
{
    public string? ManagerUsername { get; set; }
}