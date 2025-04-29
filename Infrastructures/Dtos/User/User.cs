namespace Infrastructures.Dtos;

public class UserProfileDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UserShortDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string AvatarUrl { get; set; }
}

public class UserSearchResultDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string AvatarUrl { get; set; }
}