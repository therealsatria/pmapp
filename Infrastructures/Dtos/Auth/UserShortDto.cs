using System;

namespace Infrastructures.Dtos;

/// <summary>
/// DTO for compact user information used in other DTOs
/// </summary>
public class UserShortDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string ProfilePictureUrl { get; set; }
    public string Role { get; set; }
} 