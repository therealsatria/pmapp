namespace Infrastructures.Dtos;

public class ProjectListDto
{
    public Guid Id { get; set; }
    public string ProjectName { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public IEnumerable<ProjectMemberShortDto> TeamMembers { get; set; }
}

public class ProjectDetailDto : ProjectListDto
{
    public string Description { get; set; }
    public string Priority { get; set; }
    public UserShortDto CreatedBy { get; set; }
    // public IEnumerable<ProjectPhaseDto> Phases { get; set; }
    // public IEnumerable<ProjectUpdateDto> RecentUpdates { get; set; }
}