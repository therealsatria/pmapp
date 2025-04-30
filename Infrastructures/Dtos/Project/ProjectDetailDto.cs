namespace Infrastructures.Dtos;

/// <summary>
/// DTO for detailed project information
/// </summary>
public class ProjectDetailDto : ProjectListDto
{
    public int DaysRemaining { get; set; }
    public bool IsOverdue { get; set; }
    public Dictionary<string, int> TasksByStatus { get; set; }
    public Dictionary<string, int> TasksByPriority { get; set; }
    public IEnumerable<ProjectTaskDto> RecentTasks { get; set; }
    public IEnumerable<ProjectMemberDto> Members { get; set; }
    public IEnumerable<ProjectActivityDto> RecentActivities { get; set; }
}