using System.Security.Cryptography;
using System.Text;
using Infrastructures.Data;
using Infrastructures.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
    }

    public async Task<User> RegisterUserAsync(string username, string email, string password, string fullName)
    {
        using var hmac = new HMACSHA512();
        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        var user = new User
        {
            Username = username,
            Email = email,
            FullName = fullName,
            PasswordHash = passwordHash,
            PasswordSalt = hmac.Key,
            ProfilePictureUrl = "", // Default empty string
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> LoginUserAsync(string username, string password)
    {
        var user = await GetUserByUsernameAsync(username);
        
        if (user == null) return null;
        
        using var hmac = new HMACSHA512(user.PasswordSalt);
        
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return null;
        }
        
        return user;
    }
}
