using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Dtos;

public class AddProjectMemberDto
{
    [Required]
    public Guid ProjectId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string Role { get; set; } // "Manager", "Developer", etc.
}

public class UpdateProjectMemberRoleDto
{
    [Required]
    public Guid ProjectMemberId { get; set; }

    [Required]
    public string NewRole { get; set; }
}