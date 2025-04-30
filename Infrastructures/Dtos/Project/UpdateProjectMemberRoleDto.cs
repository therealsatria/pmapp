using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Dtos;

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