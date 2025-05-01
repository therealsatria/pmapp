using Infrastructures.Dtos;

namespace Infrastructures.Services;
public interface IUserService
{
    Task<UserDto> RegisterUserAsync(RegisterDto registerDto);
    Task<UserDto> LoginUserAsync(LoginDto loginDto);
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<bool> UpdateLastLoginAsync(Guid userId);
    Task<IEnumerable<UserShortDto>> GetUserShorts();
}