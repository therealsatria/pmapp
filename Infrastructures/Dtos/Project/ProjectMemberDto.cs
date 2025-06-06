namespace Infrastructures.Dtos;

/// <summary>
/// DTO for project member information
/// </summary>
public class ProjectMemberDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public UserShortDto User { get; set; }
    public string Role { get; set; }
    public DateTime JoinedAt { get; set; }
    public DateTime? LastActive { get; set; }
    public bool IsActive { get; set; }
    public int AssignedTasksCount { get; set; }
    public int CompletedTasksCount { get; set; }
    public double CompletionRate => AssignedTasksCount > 0 ? (double)CompletedTasksCount / AssignedTasksCount * 100 : 0;
}
