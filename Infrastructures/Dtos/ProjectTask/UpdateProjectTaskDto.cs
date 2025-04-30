using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Dtos;

/// <summary>
/// DTO for updating an existing task
/// </summary>
public class UpdateProjectTaskDto
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string Status { get; set; }
    
    public string Priority { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public Guid? AssignedToId { get; set; }
    
    public int? Progress { get; set; }
    
    public int? EstimatedHours { get; set; }
    
    public int? ActualHours { get; set; }
    
    public List<string> Tags { get; set; }
}