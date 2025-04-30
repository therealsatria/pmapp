using System;
using System.Collections.Generic;

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
    public string Priority { get; set; }
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