using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Dtos;

public class CreateProjectDto
{
    [Required, StringLength(100)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    public string Priority { get; set; } // "Low", "Medium", "High"
}

public class UpdateProjectDto : CreateProjectDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Status { get; set; } // "Planning", "Active", etc.
}