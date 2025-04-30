using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Dtos;

/// <summary>
/// DTO for detailed task information
/// </summary>
public class ProjectTaskDetailDto : ProjectTaskDto
{
    public IEnumerable<ProjectTaskCommentDto> Comments { get; set; }
    public IEnumerable<ProjectTaskHistoryDto> History { get; set; }
    public IEnumerable<ProjectTaskAttachmentDto> Attachments { get; set; }
    public List<string> Tags { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int EstimatedHours { get; set; }
    public int? ActualHours { get; set; }
}