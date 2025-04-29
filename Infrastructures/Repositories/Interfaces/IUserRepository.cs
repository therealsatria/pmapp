using Infrastructures.Models;

namespace Infrastructures.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User> RegisterUserAsync(string username, string email, string password);
    Task<User> LoginUserAsync(string username, string password);    
}