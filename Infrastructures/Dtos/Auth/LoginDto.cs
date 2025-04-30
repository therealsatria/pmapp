using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "Username is required")]
    [Display(Name = "Username or Email")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    
    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; } = false;
}