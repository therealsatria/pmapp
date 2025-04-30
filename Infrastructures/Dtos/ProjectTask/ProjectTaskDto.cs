using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Dtos;

/// <summary>
/// DTO for task information in list views
/// </summary>
public class ProjectTaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public int Progress { get; set; }
    public UserShortDto AssignedTo { get; set; }
    public UserShortDto CreatedBy { get; set; }
    public Guid ProjectId { get; set; }
    public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.UtcNow && Status != "Completed";
}









