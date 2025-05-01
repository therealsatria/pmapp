using Infrastructures.Dtos;

namespace Infrastructures.Services;

/// <summary>
/// Service for managing projects in the project management application
/// </summary>
public interface IProjectService
{
    // Project CRUD operations
    Task<IEnumerable<ProjectListDto>> GetAllProjectsAsync();
    Task<ProjectDetailDto> GetProjectByIdAsync(Guid projectId);
    Task<ProjectListDto> CreateProjectAsync(CreateProjectDto request);
    Task<ProjectListDto> UpdateProjectAsync(Guid projectId, UpdateProjectDto request);
    Task<bool> DeleteProjectAsync(Guid projectId);
    
    // User-specific project operations
    Task<IEnumerable<ProjectListDto>> GetProjectsByUserIdAsync(Guid userId, string status = null);
    Task<bool> IsUserInProjectAsync(Guid userId, Guid projectId);
    
    // Project analytics
    Task<int> GetProjectProgressAsync(Guid projectId);
    Task<Dictionary<string, int>> GetTaskStatusDistributionAsync(Guid projectId);
    Task<Dictionary<string, int>> GetMemberWorkloadAsync(Guid projectId);
    Task<ProjectStatisticsDto> GetProjectStatisticsAsync(Guid projectId);
    
    // Project member management
    Task<IEnumerable<ProjectMemberDto>> GetProjectMembersAsync(Guid projectId);
    Task<ProjectMemberDto> GetProjectMemberAsync(Guid projectId, Guid userId);
    Task<ProjectMemberDto> AddMemberToProjectAsync(AddProjectMemberDto request);
    Task<ProjectMemberDto> UpdateMemberRoleAsync(UpdateProjectMemberRoleDto request);
    Task<bool> RemoveMemberFromProjectAsync(Guid memberId);
    
    // Project status management
    Task<bool> UpdateProjectStatusAsync(Guid projectId, string newStatus);
    Task<bool> ExtendProjectDeadlineAsync(Guid projectId, DateTime newDeadline);
    
    // Project activity tracking
    Task<IEnumerable<ProjectActivityDto>> GetProjectActivitiesAsync(Guid projectId, int count = 10);
    Task LogProjectActivityAsync(Guid projectId, string activityType, string description, Guid userId);
}