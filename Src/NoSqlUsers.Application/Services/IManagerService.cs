using MyEmployees.Domain.Models;
using MyEmployees.Domain.Models.Common;

namespace MyEmployees.Application.Services;

public interface IManagerService
{
    Task<bool> CreateAsync(Manager manager, CancellationToken cancellationToken = default);

    Task<Manager?> GetByUsernameAsync(Username username, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(Manager manager, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteAsync(Username username, CancellationToken cancellationToken = default);
}