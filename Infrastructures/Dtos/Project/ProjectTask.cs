using System;
using System.Collections.Generic;
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

/// <summary>
/// DTO for task comments
/// </summary>
public class ProjectTaskCommentDto
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserShortDto User { get; set; }
}

/// <summary>
/// DTO for task history records
/// </summary>
public class ProjectTaskHistoryDto
{
    public Guid Id { get; set; }
    public string ChangeType { get; set; }
    public string PreviousValue { get; set; }
    public string NewValue { get; set; }
    public DateTime ChangedAt { get; set; }
    public UserShortDto ChangedBy { get; set; }
}

/// <summary>
/// DTO for task attachments
/// </summary>
public class ProjectTaskAttachmentDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
    public long FileSize { get; set; }
    public string FileUrl { get; set; }
    public DateTime UploadedAt { get; set; }
    public UserShortDto UploadedBy { get; set; }
} 