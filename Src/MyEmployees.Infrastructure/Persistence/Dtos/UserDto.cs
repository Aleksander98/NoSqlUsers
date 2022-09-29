using System.Text.Json.Serialization;

namespace MyEmployees.Infrastructure.Persistence.Dtos;

public abstract class UserDto
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = default!;

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = default!;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = default!;

    [JsonPropertyName("dateOfBirth")]
    public DateTime DateOfBirth { get; set; } = default!;
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = default!;
}