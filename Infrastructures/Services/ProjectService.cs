using Infrastructures.Dtos;
using Infrastructures.Repositories;

namespace Infrastructures.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
    }

    public async Task<IEnumerable<ProjectListDto>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllProjectsAsync();
        return projects.Select(p => new ProjectListDto
        {
            Id = p.Id,
            ProjectName = p.ProjectName,
            Description = p.Description,
            Status = p.Status,
            StartDate = p.StartDate ?? DateTime.MinValue,
            EndDate = p.EndDate,
            TeamMembers = p.ProjectMembers.Select(m => new ProjectMemberShortDto
            {
                UserId = m.UserId,
                Role = m.Role,
                JoinedAt = m.JoinedAt,
                ProjectId = m.ProjectId
            })
        });
    }
    public async Task<ProjectListDto> GetProjectByIdAsync(Guid projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
        {
            throw new KeyNotFoundException($"Project with ID {projectId} not found.");
        }

        return new ProjectListDto
        {
            Id = project.Id,
            ProjectName = project.ProjectName,
            Description = project.Description,
            Status = project.Status,
            StartDate = project.StartDate ?? DateTime.MinValue,
            EndDate = project.EndDate,
            TeamMembers = project.ProjectMembers.Select(m => new ProjectMemberShortDto
            {
                UserId = m.UserId,
                Role = m.Role,
                JoinedAt = m.JoinedAt,
                ProjectId = m.ProjectId
            })
        };
    }

    Task<ProjectMemberDto> IProjectService.AddMemberToProjectAsync(AddProjectMemberDto memberDto)
    {
        throw new NotImplementedException();
    }

    Task<bool> IProjectService.AssignTaskAsync(Guid taskId, Guid userId)
    {
        throw new NotImplementedException();
    }

    Task<ProjectListDto> IProjectService.CreateProjectAsync(CreateProjectDto projectDto)
    {
        throw new NotImplementedException();
    }

    Task<ProjectTaskDto> IProjectService.CreateTaskAsync(CreateProjectTaskDto taskDto)
    {
        throw new NotImplementedException();
    }

    Task<bool> IProjectService.DeleteProjectAsync(Guid projectId)
    {
        throw new NotImplementedException();
    }

    Task<bool> IProjectService.DeleteTaskAsync(Guid taskId)
    {
        throw new NotImplementedException();
    }

    Task<bool> IProjectService.ExtendProjectDeadlineAsync(Guid projectId, DateTime newDeadline)
    {
        throw new NotImplementedException();
    }

    Task<Dictionary<string, int>> IProjectService.GetMemberWorkloadAsync(Guid projectId)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<ProjectActivityDto>> IProjectService.GetProjectActivitiesAsync(Guid projectId, int count)
    {
        throw new NotImplementedException();
    }

    Task<ProjectDetailDto> IProjectService.GetProjectByIdAsync(Guid projectId)
    {
        throw new NotImplementedException();
    }

    Task<ProjectMemberDto> IProjectService.GetProjectMemberAsync(Guid projectId, Guid userId)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<ProjectMemberDto>> IProjectService.GetProjectMembersAsync(Guid projectId)
    {
        throw new NotImplementedException();
    }

    Task<int> IProjectService.GetProjectProgressAsync(Guid projectId)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<ProjectListDto>> IProjectService.GetProjectsByUserIdAsync(Guid userId, string status)
    {
        throw new NotImplementedException();
    }

    Task<ProjectStatisticsDto> IProjectService.GetProjectStatisticsAsync(Guid projectId)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<ProjectTaskDto>> IProjectService.GetProjectTasksAsync(Guid projectId)
    {
        throw new NotImplementedException();
    }

    Task<ProjectTaskDetailDto> IProjectService.GetTaskByIdAsync(Guid taskId)
    {
        throw new NotImplementedException();
    }

    Task<Dictionary<string, int>> IProjectService.GetTaskStatusDistributionAsync(Guid projectId)
    {
        throw new NotImplementedException();
    }

    Task<bool> IProjectService.IsUserInProjectAsync(Guid userId, Guid projectId)
    {
        throw new NotImplementedException();
    }

    Task IProjectService.LogProjectActivityAsync(Guid projectId, string activityType, string description, Guid userId)
    {
        throw new NotImplementedException();
    }

    Task<bool> IProjectService.RemoveMemberFromProjectAsync(Guid memberId)
    {
        throw new NotImplementedException();
    }

    Task<ProjectMemberDto> IProjectService.UpdateMemberRoleAsync(UpdateProjectMemberRoleDto updateDto)
    {
        throw new NotImplementedException();
    }

    Task<ProjectListDto> IProjectService.UpdateProjectAsync(Guid projectId, UpdateProjectDto projectDto)
    {
        throw new NotImplementedException();
    }

    Task<bool> IProjectService.UpdateProjectStatusAsync(Guid projectId, string newStatus)
    {
        throw new NotImplementedException();
    }

    Task<ProjectTaskDto> IProjectService.UpdateTaskAsync(UpdateProjectTaskDto taskDto)
    {
        throw new NotImplementedException();
    }

    Task<bool> IProjectService.UpdateTaskStatusAsync(Guid taskId, string newStatus)
    {
        throw new NotImplementedException();
    }
}