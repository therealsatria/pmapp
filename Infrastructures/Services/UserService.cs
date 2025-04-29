using Infrastructures.Dtos;
using Infrastructures.Repositories;

namespace Infrastructures.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    public UserService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    public async Task<UserDto?> LoginUserAsync(LoginDto loginDto)
    {
        var user = await _userRepository.LoginUserAsync(loginDto.Username, loginDto.Password);
        if (user == null) return null;
        
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            Token = _tokenService.CreateToken(user)
        };
    }
    public async Task<UserDto> RegisterUserAsync(RegisterDto registerDto)
    {
        var user = await _userRepository.RegisterUserAsync(
            registerDto.Username, 
            registerDto.Email, 
            registerDto.Password
        );

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            Token = _tokenService.CreateToken(user)
        };
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null) return null;
        
        return new UserDto  
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            Token = _tokenService.CreateToken(user)
        };
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            Token = _tokenService.CreateToken(user)
        });
    }    
}
