namespace Infrastructures.Dtos;

/// <summary>
/// DTO for project activity/history records
/// </summary>
public class ProjectActivityDto
{
    public Guid Id { get; set; }
    public string ActivityType { get; set; }
    public string Description { get; set; }
    public DateTime Timestamp { get; set; }
    public UserShortDto User { get; set; }
}