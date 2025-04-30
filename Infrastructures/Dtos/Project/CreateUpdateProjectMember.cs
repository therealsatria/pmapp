using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Dtos;

/// <summary>
/// DTO for adding a member to a project
/// </summary>
public class AddProjectMemberDto
{
    [Required]
    public Guid ProjectId { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public string Role { get; set; } // "Manager", "Developer", etc.
}

/// <summary>
/// DTO for updating a project member's role
/// </summary>
public class UpdateProjectMemberRoleDto
{
    [Required]
    public Guid MemberId { get; set; }
    
    [Required]
    public string NewRole { get; set; }
}