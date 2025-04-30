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

