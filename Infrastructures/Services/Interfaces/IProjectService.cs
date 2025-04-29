using Infrastructures.Dtos;

namespace Infrastructures.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectListDto>> GetAllProjectsAsync();
    Task<ProjectListDto> GetProjectByIdAsync(Guid projectId);
}