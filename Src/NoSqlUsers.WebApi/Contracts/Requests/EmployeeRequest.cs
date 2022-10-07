namespace MyEmployees.WebApi.Contracts.Requests;

[Serializable]
public sealed class EmployeeRequest : UserRequest
{
    public string? ManagerUsername { get; set; }
}