using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Dtos;

public class LoginDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}