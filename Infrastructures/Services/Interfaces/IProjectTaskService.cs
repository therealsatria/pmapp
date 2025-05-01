using Infrastructures.Dtos;

namespace Infrastructures.Services;

public interface IProjectTaskService
{
    Task<IEnumerable<ProjectTaskDto>> GetProjectTasksAsync(Guid projectId);
    Task<ProjectTaskDetailDto> GetTaskByIdAsync(Guid taskId);
    Task<ProjectTaskDto> CreateTaskAsync(CreateProjectTaskDto request);
    Task<ProjectTaskDto> UpdateTaskAsync(UpdateProjectTaskDto request);
    Task<bool> CompleteTaskAsync(Guid taskId);
    Task<bool> ReopenTaskAsync(Guid taskId);
    Task<bool> DeleteTaskAsync(Guid taskId);
    Task<bool> AssignTaskAsync(Guid taskId, Guid userId);
    Task<bool> UpdateTaskStatusAsync(Guid taskId, string requestStatus);
}