namespace MyEmployees.WebApi.Contracts.Requests;

public sealed class EmployeeRequest : UserRequest
{
    public string? ManagerUsername { get; set; }
}