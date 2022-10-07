namespace MyEmployees.WebApi.Contracts.Requests;

[Serializable]
public abstract class UserRequest
{
    public string Username { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public DateTime DateOfBirth { get; set; } = default!;
    
    public string Email { get; set; } = default!;
}