using System.Text.Json.Serialization;

namespace MyEmployees.Infrastructure.Persistence.Dtos;

public sealed class EmployeeDto : UserDto
{
    [JsonPropertyName("pk")]
    public string Pk => DatabaseSettings.EmployeesPrefix + Username;

    [JsonPropertyName("sk")]
    public string Sk => DatabaseSettings.EmployeesPrefix + Username;
    
    [JsonPropertyName("managerUsername")]
    public string? ManagerUsername { get; set; }
}