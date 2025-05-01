using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Dtos;

/// <summary>
/// DTO for updating an existing project
/// </summary>
public class UpdateProjectDto
{
    [Required]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string ProjectName { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; } // "Planning", "Active", "OnHold", "Completed", "Cancelled"
    
    [Required]
    public Guid CreatedById { get; set; }
}