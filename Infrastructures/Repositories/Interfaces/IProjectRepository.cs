using Infrastructures.Models;

namespace Infrastructures.Repositories;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllProjectsAsync();
    Task<Project> GetByIdAsync(Guid projectId);
    Task<Project> CreateAsync(Project project);
    Task<Project> UpdateAsync(Guid projectId, Project project);
    Task DeleteAsync(Guid projectId);
    Task<IEnumerable<Project>> GetProjectsByUserIdAsync(Guid userId, string status = null);

    // Additional methods
    Task<bool> IsUserInProject(Guid userId, Guid projectId);
    Task<int> GetProjectProgressAsync(Guid projectId);
}