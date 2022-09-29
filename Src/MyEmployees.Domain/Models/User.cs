using MyEmployees.Domain.Models.Common;

namespace MyEmployees.Domain.Models;

public abstract class User
{
    public Username Username { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public DateOnly DateOfBirth { get; set; }
    
    public Email Email { get; set; } = default!;
}