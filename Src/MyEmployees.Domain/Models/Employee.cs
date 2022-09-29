using MyEmployees.Domain.Models.Common;

namespace MyEmployees.Domain.Models;

public sealed class Employee : User
{
    public Username? ManagerUsername { get; set; }
}