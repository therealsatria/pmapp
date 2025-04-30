namespace Infrastructures.Dtos;

/// <summary>
/// DTO for project statistics and analytics
/// </summary>
public class ProjectStatisticsDto
{
    public Guid ProjectId { get; set; }
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int InProgressTasks { get; set; }
    public int PendingTasks { get; set; }
    public int OverdueTasks { get; set; }
    public int TotalMembers { get; set; }
    public Dictionary<string, int> TasksByPriority { get; set; }
    public Dictionary<string, int> TasksByStatus { get; set; }
    public Dictionary<string, int> TasksByAssignee { get; set; }
}