using Infrastructures.Models;

namespace Infrastructures.Services;

public interface ITokenService
{
    string CreateToken(User user);
}