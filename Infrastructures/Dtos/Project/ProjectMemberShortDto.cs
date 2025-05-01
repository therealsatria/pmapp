namespace Infrastructures.Dtos;

/// <summary>
/// DTO for compact project member information
/// </summary>
public class ProjectMemberShortDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public string ProfilePictureUrl { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime JoinedAt { get; set; }
    public string Role { get; set; }
}