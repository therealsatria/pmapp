using Infrastructures.Dtos;
using Infrastructures.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructures.Services;

/// <summary>
/// Service for managing projects in the project management application
/// </summary>
public interface IProjectService
{
    // Project CRUD operations
    Task<IEnumerable<ProjectListDto>> GetAllProjectsAsync();
    Task<ProjectDetailDto> GetProjectByIdAsync(Guid projectId);
    Task<ProjectListDto> CreateProjectAsync(CreateProjectDto projectDto);
    Task<ProjectListDto> UpdateProjectAsync(Guid projectId, UpdateProjectDto projectDto);
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
    Task<ProjectMemberDto> AddMemberToProjectAsync(AddProjectMemberDto memberDto);
    Task<ProjectMemberDto> UpdateMemberRoleAsync(UpdateProjectMemberRoleDto updateDto);
    Task<bool> RemoveMemberFromProjectAsync(Guid memberId);
    
    // Project status management
    Task<bool> UpdateProjectStatusAsync(Guid projectId, string newStatus);
    Task<bool> ExtendProjectDeadlineAsync(Guid projectId, DateTime newDeadline);
    
    // Task management within projects
    Task<IEnumerable<ProjectTaskDto>> GetProjectTasksAsync(Guid projectId);
    Task<ProjectTaskDetailDto> GetTaskByIdAsync(Guid taskId);
    Task<ProjectTaskDto> CreateTaskAsync(CreateProjectTaskDto taskDto);
    Task<ProjectTaskDto> UpdateTaskAsync(UpdateProjectTaskDto taskDto);
    Task<bool> DeleteTaskAsync(Guid taskId);
    Task<bool> AssignTaskAsync(Guid taskId, Guid userId);
    Task<bool> UpdateTaskStatusAsync(Guid taskId, string newStatus);
    
    // Project activity tracking
    Task<IEnumerable<ProjectActivityDto>> GetProjectActivitiesAsync(Guid projectId, int count = 10);
    Task LogProjectActivityAsync(Guid projectId, string activityType, string description, Guid userId);
}