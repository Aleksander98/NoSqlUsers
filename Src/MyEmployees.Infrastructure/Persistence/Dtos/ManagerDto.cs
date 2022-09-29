using System.Text.Json.Serialization;

namespace MyEmployees.Infrastructure.Persistence.Dtos;

public sealed class ManagerDto : UserDto
{
    [JsonPropertyName("pk")]
    public string Pk => DatabaseSettings.ManagersPrefix + Username;

    [JsonPropertyName("sk")]
    public string Sk => DatabaseSettings.ManagersPrefix + Username;
}