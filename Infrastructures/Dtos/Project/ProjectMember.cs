namespace Infrastructures.Dtos;

public class ProjectMemberDto
{
    public Guid ProjectMemberId { get; set; }
    //public UserShortDto User { get; set; }
    public string Role { get; set; }
    public DateTime JoinedAt { get; set; }
    public bool IsActive { get; set; }
}

public class ProjectMemberShortDto
{
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime JoinedAt { get; set; }
    public string Role { get; set; }
}