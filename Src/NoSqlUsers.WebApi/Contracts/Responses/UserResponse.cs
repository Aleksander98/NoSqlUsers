namespace MyEmployees.WebApi.Contracts.Responses;

[Serializable]
public abstract class UserResponse
{
    public string Username { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public DateTime DateOfBirth { get; set; }
    
    public string Email { get; set; } = default!;
}