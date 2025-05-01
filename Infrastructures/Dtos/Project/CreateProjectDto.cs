using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Dtos;

/// <summary>
/// DTO for creating a new project
/// </summary>
public class CreateProjectDto
{
    [Required, StringLength(100)]
    public string ProjectName { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
    
    public List<Guid> InitialMemberIds { get; set; } = new List<Guid>();
    
    public Guid CreatedById { get; set; }
}
