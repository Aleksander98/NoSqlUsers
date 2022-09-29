using FluentValidation;
using FluentValidation.Results;
using MyEmployees.Application.Repositories;
using MyEmployees.Domain.Models;
using MyEmployees.Domain.Models.Common;

namespace MyEmployees.Application.Services;

public sealed class ManagerService : IManagerService
{
    private readonly IManagerRepository _managerRepository;

    public ManagerService(IManagerRepository managerRepository)
    {
        _managerRepository = managerRepository ?? throw new ArgumentNullException(nameof(managerRepository));
    }

    public async Task<bool> CreateAsync(Manager manager, CancellationToken cancellationToken = default)
    {
        if (await _managerRepository.GetByUsernameAsync(manager.Username, cancellationToken) is not null)
        {
            var message = $"Username '{manager.Username}' is already taken.";
            
            throw new ValidationException(message, GenerateValidationFailure(message));
        }
        
        return await _managerRepository.CreateAsync(manager, cancellationToken);
    }

    public Task<Manager?> GetByUsernameAsync(Username username, CancellationToken cancellationToken = default)
    {
        return _managerRepository.GetByUsernameAsync(username, cancellationToken);
    }

    public Task<bool> DeleteAsync(Username username, CancellationToken cancellationToken = default)
    {
        return _managerRepository.DeleteAsync(username, cancellationToken);
    }

    private static IEnumerable<ValidationFailure> GenerateValidationFailure(string message) => new[]
    {
        new ValidationFailure(nameof(Manager), message)
    };
}