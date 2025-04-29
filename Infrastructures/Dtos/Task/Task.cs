namespace Infrastructures.Dtos;

public class TaskListDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Status { get; set; } // "To Do", "In Progress", etc.
    public string Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public UserShortDto AssignedTo { get; set; }
    public decimal? EstimatedHours { get; set; }
}

public class TaskDetailDto : TaskListDto
{
    public string Description { get; set; }
    public UserShortDto CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<TaskCommentDto> Comments { get; set; }
    // public IEnumerable<TaskAttachmentDto> Attachments { get; set; }
    // public IEnumerable<TimeLogDto> TimeLogs { get; set; }
}