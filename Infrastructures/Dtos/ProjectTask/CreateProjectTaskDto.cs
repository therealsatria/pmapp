using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Dtos;

/// <summary>
/// DTO for creating a new task
/// </summary>
public class CreateProjectTaskDto
{
    [Required, StringLength(200)]
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    [Required]
    public string Priority { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    [Required]
    public Guid ProjectId { get; set; }
    
    public Guid? AssignedToId { get; set; }
    
    public int EstimatedHours { get; set; }
    
    public List<string> Tags { get; set; } = new List<string>();
}