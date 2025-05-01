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
            Role = user.Role
        };
    }
    public async Task<UserDto> RegisterUserAsync(RegisterDto registerDto)
    {
        var user = await _userRepository.RegisterUserAsync(
            registerDto.Username, 
            registerDto.Email, 
            registerDto.Password,
            registerDto.FullName
        );

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            CreatedAt = DateTime.Now
        };
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null) return null;
        
        var now = DateTime.Now;
        return new UserDto  
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            LastLogin = user.LastLogin ?? now,
            LastLoginFormatted = GetRelativeTimeString(user.LastLogin ?? now, now)
        };
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        var now = DateTime.UtcNow;
        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            LastLogin = user.LastLogin ?? now,
            LastLoginFormatted = GetRelativeTimeString(user.LastLogin ?? now, now)
        });
    }

    public async Task<IEnumerable<UserShortDto>> GetUserShorts()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.Select(user => new UserShortDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role,
            FullName = user.FullName,
            Email = user.Email
        });
    }
    
    private string GetRelativeTimeString(DateTime pastTime, DateTime currentTime)
    {
        var timeSpan = currentTime - pastTime;
        
        if (timeSpan.TotalSeconds < 60)
        {
            return $"{(int)timeSpan.TotalSeconds} detik yang lalu";
        }
        if (timeSpan.TotalMinutes < 60)
        {
            return $"{(int)timeSpan.TotalMinutes} menit yang lalu";
        }
        if (timeSpan.TotalHours < 24)
        {
            return $"{(int)timeSpan.TotalHours} jam yang lalu";
        }
        if (timeSpan.TotalDays < 30)
        {
            return $"{(int)timeSpan.TotalDays} hari yang lalu";
        }
        if (timeSpan.TotalDays < 365)
        {
            int months = (int)(timeSpan.TotalDays / 30);
            return $"{months} bulan yang lalu";
        }
        
        int years = (int)(timeSpan.TotalDays / 365);
        return $"{years} tahun yang lalu";
    }

    public async Task<bool> UpdateLastLoginAsync(Guid userId)
    {
        return await _userRepository.UpdateLastLoginAsync(userId);
    }
}
