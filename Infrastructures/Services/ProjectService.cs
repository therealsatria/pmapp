using Infrastructures.Dtos;
using Infrastructures.Models;
using Infrastructures.Repositories;

namespace Infrastructures.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;

    public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository)
    {
        _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
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

    public async Task<ProjectDetailDto> GetProjectByIdAsync(Guid projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
        {
            throw new KeyNotFoundException($"Project with ID {projectId} not found.");
        }

        // Get project progress
        int progress = 0;
        try 
        {
            progress = await _projectRepository.GetProjectProgressAsync(projectId);
        }
        catch { /* Ignore errors and default to 0 */ }

        // Create user info for CreatedBy - using the loaded navigation property
        var createdBy = project.CreatedBy != null
            ? new UserShortDto
            {
                Id = project.CreatedBy.Id,
                Username = project.CreatedBy.Username,
                FullName = project.CreatedBy.FullName,
                Email = project.CreatedBy.Email,
                ProfilePictureUrl = project.CreatedBy.ProfilePictureUrl,
                Role = project.CreatedBy.Role
            }
            : new UserShortDto(); // Empty if no created by user

        // Create new ProjectDetailDto
        return new ProjectDetailDto
        {
            Id = project.Id,
            ProjectName = project.ProjectName,
            Description = project.Description,
            Status = project.Status,
            StartDate = project.StartDate ?? DateTime.MinValue,
            EndDate = project.EndDate,
            Progress = progress,
            DaysRemaining = project.EndDate.HasValue 
                ? (int)(project.EndDate.Value - DateTime.UtcNow).TotalDays 
                : 0,
            IsOverdue = project.EndDate.HasValue && project.EndDate.Value < DateTime.UtcNow,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            CreatedBy = createdBy,
            TeamMembers = project.ProjectMembers.Select(m => new ProjectMemberShortDto
            {
                UserId = m.UserId,
                Role = m.Role,
                JoinedAt = m.JoinedAt,
                ProjectId = m.ProjectId
            }),
            // Tambahkan property lain sesuai kebutuhan
            TasksByStatus = new Dictionary<string, int>(), // Perlu implementasi lebih lanjut
            TasksByPriority = new Dictionary<string, int>(), // Perlu implementasi lebih lanjut
            RecentTasks = new List<ProjectTaskDto>(), // Perlu implementasi lebih lanjut
            Members = new List<ProjectMemberDto>(), // Perlu implementasi lebih lanjut
            RecentActivities = new List<ProjectActivityDto>() // Perlu implementasi lebih lanjut
        };
    }

    public async Task<ProjectListDto> CreateProjectAsync(CreateProjectDto request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        if (request.CreatedById == Guid.Empty)
        {
            throw new ArgumentException("Project owner (CreatedById) is required", nameof(request.CreatedById));
        }

        var project = new Project
        {
            Id = Guid.NewGuid(),
            ProjectName = request.ProjectName,
            Description = request.Description,
            StartDate = request.StartDate.ToUniversalTime(),
            EndDate = request.EndDate?.ToUniversalTime(),
            Status = "Planning",
            CreatedById = request.CreatedById,
            CreatedAt = DateTime.UtcNow
        };
        var createdProject = await _projectRepository.CreateAsync(project);
        if (createdProject == null)
        {
            throw new InvalidOperationException("Failed to create project.");
        }
        return new ProjectListDto
        {
            Id = createdProject.Id,
            ProjectName = createdProject.ProjectName,
            Description = createdProject.Description,
            Status = createdProject.Status,
            StartDate = createdProject.StartDate ?? DateTime.MinValue,
            EndDate = createdProject.EndDate,
            CreatedAt = createdProject.CreatedAt,
            UpdatedAt = createdProject.UpdatedAt,
            TeamMembers = createdProject.ProjectMembers.Select(m => new ProjectMemberShortDto
            {
                UserId = m.UserId,
                Role = m.Role,
                JoinedAt = m.JoinedAt,
                ProjectId = m.ProjectId
            })
        };
    }

    async Task<ProjectListDto> IProjectService.UpdateProjectAsync(Guid projectId, UpdateProjectDto request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        if (request.CreatedById == Guid.Empty)
        {
            throw new ArgumentException("Project owner (CreatedById) is required", nameof(request.CreatedById));
        }

        var project = await _projectRepository.GetByIdAsync(projectId)
        ?? throw new KeyNotFoundException($"Project with ID {projectId} not found.");

        project.ProjectName = request.ProjectName;
        project.Description = request.Description;
        project.StartDate = request.StartDate;
        project.EndDate = request.EndDate;
        project.Status = request.Status;
        project.CreatedById = request.CreatedById;
        project.UpdatedAt = DateTime.UtcNow;
        var updatedProject = await _projectRepository.UpdateAsync(projectId, project);
        if (updatedProject == null)
        {
            throw new InvalidOperationException("Failed to update project.");
        }
        return new ProjectListDto
        {
            Id = updatedProject.Id,
            ProjectName = updatedProject.ProjectName,
            Description = updatedProject.Description,
            Status = updatedProject.Status,
            StartDate = (DateTime)updatedProject.StartDate,
            EndDate = updatedProject.EndDate,
            CreatedAt = updatedProject.CreatedAt,
            UpdatedAt = updatedProject.UpdatedAt,
            TeamMembers = updatedProject.ProjectMembers.Select(m => new ProjectMemberShortDto
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

    Task<bool> IProjectService.CompleteTaskAsync(Guid taskId)
    {
        throw new NotImplementedException();
    }

    Task<bool> IProjectService.ReopenTaskAsync(Guid taskId)
    {
        throw new NotImplementedException();
    }
}