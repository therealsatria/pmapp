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
}