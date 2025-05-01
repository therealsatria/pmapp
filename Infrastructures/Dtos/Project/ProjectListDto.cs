namespace Infrastructures.Dtos;

/// <summary>
/// DTO for basic project information in list views
/// </summary>
public class ProjectListDto
{
    public Guid Id { get; set; }
    public string ProjectName { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int Progress { get; set; }
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public UserShortDto CreatedBy { get; set; }
    public IEnumerable<ProjectMemberShortDto> TeamMembers { get; set; }
}




