using Infrastructures.Models;

namespace Infrastructures.Repositories;

public interface IProjectRepository
{
    Task<Project> GetByIdAsync(Guid projectId);
    Task<IEnumerable<Project>> GetAllProjectsAsync();
    // Task<IEnumerable<Project>> GetProjectsByUserIdAsync(Guid userId, string status = null);
    // Task CreateAsync(Project project);
    // Task UpdateAsync(Project project);
    // Task DeleteAsync(Guid projectId);

    // Member
    // Task<ProjectMember> GetMembersAsync(Guid projectId, Guid userId);
    // Task<IEnumerable<ProjectMember>> GetMembersByProjectAsync(Guid projectId);
    // Task AddMemberAsync(ProjectMember member);
    // Task UpdateMemberRoleAsync(Guid memberId, string newRole);
    // Task RemoveMemberAsync(Guid memberId);

    // Additional methods
    // Task<bool> IsUserInProject(Guid userId, Guid projectId);
    // Task<int> GetProjectProgressAsync(Guid projectId);
}