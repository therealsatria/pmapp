using System;
using System.ComponentModel.DataAnnotations;

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

/// <summary>
/// DTO for compact project member information
/// </summary>
public class ProjectMemberShortDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public string ProfilePictureUrl { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime JoinedAt { get; set; }
    public string Role { get; set; }
}