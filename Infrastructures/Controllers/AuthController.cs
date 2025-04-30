using Infrastructures.Dtos;
using Infrastructures.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Infrastructures.Controllers;

public class AuthController : Controller
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }
    
    // GET action to display the registration form
    [HttpGet]
    public IActionResult Register()
    {
        return View("~/Views/User/Register.cshtml");
    }
    
    // POST action to handle form submission
    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/User/Register.cshtml", registerDto);
        }
        
        if (registerDto.Password != registerDto.ConfirmPassword)
        {
            ModelState.AddModelError("ConfirmPassword", "Password and Confirm Password do not match");
            return View("~/Views/User/Register.cshtml", registerDto);
        }
        
        var user = await _userService.RegisterUserAsync(registerDto);
        if (user == null)
        {
            ModelState.AddModelError("", "User already exists");
            return View("~/Views/User/Register.cshtml", registerDto);
        }   
        
        // Redirect to login page after successful registration
        return RedirectToAction("Login");
    }

    // GET action to display the login form
    [HttpGet]
    public IActionResult Login()
    {
        return View("~/Views/User/Login.cshtml");
    }
    
    // POST action to handle login submission
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/User/Login.cshtml", loginDto);
        }
        
        var user = await _userService.LoginUserAsync(loginDto);
        if (user == null)
        {
            ModelState.AddModelError("", "Invalid username or password");
            return View("~/Views/User/Login.cshtml", loginDto);
        }
        
        // Set authentication cookie
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };
        
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = loginDto.RememberMe,
            ExpiresUtc = loginDto.RememberMe ? 
                DateTimeOffset.UtcNow.AddDays(30) : 
                DateTimeOffset.UtcNow.AddHours(1)
        };
        
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
        
        // Update last login timestamp
        await _userService.UpdateLastLoginAsync(user.Id);
        
        // Redirect to previous page if available, otherwise go to home
        if (TempData["ReturnUrl"] != null)
        {
            return Redirect(TempData["ReturnUrl"]?.ToString() ?? "/");
        }
        
        return RedirectToAction("Index", "Home");
    }
    
    // GET action to log out
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
    
    // GET action to handle access denied
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View("~/Views/User/AccessDenied.cshtml");
    }
    
    // GET action to display all users
    [HttpGet]
    public async Task<IActionResult> UserList()
    {
        var users = await _userService.GetAllUsersAsync();
        return View("~/Views/User/UserList.cshtml", users);
    }   
}