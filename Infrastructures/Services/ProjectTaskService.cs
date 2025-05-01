using Infrastructures.Dtos;
using Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Infrastructures.Data;
using AutoMapper;

namespace Infrastructures.Services;
public class ProjectTaskService : IProjectTaskService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public ProjectTaskService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ProjectTaskDto> CreateTaskAsync(CreateProjectTaskDto request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        // Validate project exists
        var projectExists = await _dbContext.Projects
            .AnyAsync(p => p.Id == request.ProjectId && !p.IsDeleted);
            
        if (!projectExists)
        {
            throw new KeyNotFoundException($"Project with ID {request.ProjectId} not found.");
        }

        // Validate assigned user if provided
        if (request.AssignedToId.HasValue)
        {
            var userExists = await _dbContext.Users
                .AnyAsync(u => u.Id == request.AssignedToId && !u.IsDeleted && u.IsActive);
                
            if (!userExists)
            {
                throw new KeyNotFoundException($"User with ID {request.AssignedToId} not found or is inactive.");
            }
        }

        // Create a new task entity
        var taskEntity = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description ?? string.Empty,
            Status = "Todo", // Default status for new tasks
            Priority = request.Priority,
            ProjectId = request.ProjectId,
            AssignedToId = request.AssignedToId,
            EstimatedHours = request.EstimatedHours,
            DueDate = request.DueDate?.ToUniversalTime(),
            CreatedById = request.AssignedToId ?? Guid.Empty, // Temporary. Should be from current user
            CreatedAt = DateTime.UtcNow
        };

        // Add task tags if provided
        if (request.Tags != null && request.Tags.Any())
        {
            taskEntity.Tags = string.Join(",", request.Tags);
        }

        // Add to database
        await _dbContext.ProjectTasks.AddAsync(taskEntity);
        await _dbContext.SaveChangesAsync();

        // Fetch the created task with related entities
        var createdTask = await _dbContext.ProjectTasks
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedBy)
            .FirstOrDefaultAsync(t => t.Id == taskEntity.Id);

        // Map to DTO and return
        return _mapper.Map<ProjectTaskDto>(createdTask);
    }

    public async Task<IEnumerable<ProjectTaskDto>> GetProjectTasksAsync(Guid projectId)
    {
        if (projectId == Guid.Empty)
        {
            throw new ArgumentException("Project ID cannot be empty", nameof(projectId));
        }

        // Validate project exists
        var projectExists = await _dbContext.Projects
            .AnyAsync(p => p.Id == projectId && !p.IsDeleted);
            
        if (!projectExists)
        {
            throw new KeyNotFoundException($"Project with ID {projectId} not found.");
        }

        // Get all tasks for the project
        var tasks = await _dbContext.ProjectTasks
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedBy)
            .Where(t => t.ProjectId == projectId && !t.IsDeleted)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        // Map to DTOs and return
        return _mapper.Map<IEnumerable<ProjectTaskDto>>(tasks);
    }

    public async Task<ProjectTaskDetailDto> GetTaskByIdAsync(Guid taskId)
    {
        if (taskId == Guid.Empty)
        {
            throw new ArgumentException("Task ID cannot be empty", nameof(taskId));
        }

        // Get the task with all related entities
        var task = await _dbContext.ProjectTasks
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedBy)
            .Include(t => t.TaskComments)
                .ThenInclude(tc => tc.User)
            .Include(t => t.TaskAttachments)
                .ThenInclude(ta => ta.UploadedBy)
            .FirstOrDefaultAsync(t => t.Id == taskId && !t.IsDeleted);

        if (task == null)
        {
            throw new KeyNotFoundException($"Task with ID {taskId} not found.");
        }

        // Map to DTO and return
        return _mapper.Map<ProjectTaskDetailDto>(task);
    }

    public async Task<ProjectTaskDto> UpdateTaskAsync(UpdateProjectTaskDto request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        // Find the task
        var task = await _dbContext.ProjectTasks
            .FirstOrDefaultAsync(t => t.Id == request.Id && !t.IsDeleted);

        if (task == null)
        {
            throw new KeyNotFoundException($"Task with ID {request.Id} not found.");
        }

        // Validate assigned user if provided
        if (request.AssignedToId.HasValue)
        {
            var userExists = await _dbContext.Users
                .AnyAsync(u => u.Id == request.AssignedToId && !u.IsDeleted && u.IsActive);
                
            if (!userExists)
            {
                throw new KeyNotFoundException($"User with ID {request.AssignedToId} not found or is inactive.");
            }
        }

        // Update task properties
        task.Title = request.Title;
        task.Description = request.Description ?? task.Description;
        task.Status = request.Status ?? task.Status;
        task.Priority = request.Priority ?? task.Priority;
        task.DueDate = request.DueDate?.ToUniversalTime() ?? task.DueDate;
        task.AssignedToId = request.AssignedToId ?? task.AssignedToId;
        task.EstimatedHours = request.EstimatedHours ?? task.EstimatedHours;
        task.ActualHours = request.ActualHours ?? task.ActualHours;
        task.UpdatedAt = DateTime.UtcNow;

        // Update tags if provided
        if (request.Tags != null)
        {
            task.Tags = request.Tags.Any() ? string.Join(",", request.Tags) : null;
        }

        // Save changes
        _dbContext.ProjectTasks.Update(task);
        await _dbContext.SaveChangesAsync();

        // Fetch the updated task with related entities
        var updatedTask = await _dbContext.ProjectTasks
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedBy)
            .FirstOrDefaultAsync(t => t.Id == task.Id);

        // Map to DTO and return
        return _mapper.Map<ProjectTaskDto>(updatedTask);
    }

    public async Task<bool> CompleteTaskAsync(Guid taskId)
    {
        if (taskId == Guid.Empty)
        {
            throw new ArgumentException("Task ID cannot be empty", nameof(taskId));
        }

        // Find the task
        var task = await _dbContext.ProjectTasks
            .FirstOrDefaultAsync(t => t.Id == taskId && !t.IsDeleted);

        if (task == null)
        {
            throw new KeyNotFoundException($"Task with ID {taskId} not found.");
        }

        // Update task status and completion date
        task.Status = "Completed";
        task.UpdatedAt = DateTime.UtcNow;

        // Save changes
        _dbContext.ProjectTasks.Update(task);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ReopenTaskAsync(Guid taskId)
    {
        if (taskId == Guid.Empty)
        {
            throw new ArgumentException("Task ID cannot be empty", nameof(taskId));
        }

        // Find the task
        var task = await _dbContext.ProjectTasks
            .FirstOrDefaultAsync(t => t.Id == taskId && !t.IsDeleted);

        if (task == null)
        {
            throw new KeyNotFoundException($"Task with ID {taskId} not found.");
        }

        // Update task status
        task.Status = "InProgress";
        task.UpdatedAt = DateTime.UtcNow;

        // Save changes
        _dbContext.ProjectTasks.Update(task);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteTaskAsync(Guid taskId)
    {
        if (taskId == Guid.Empty)
        {
            throw new ArgumentException("Task ID cannot be empty", nameof(taskId));
        }

        // Find the task
        var task = await _dbContext.ProjectTasks
            .FirstOrDefaultAsync(t => t.Id == taskId && !t.IsDeleted);

        if (task == null)
        {
            throw new KeyNotFoundException($"Task with ID {taskId} not found.");
        }

        // Soft delete the task
        task.IsDeleted = true;
        task.UpdatedAt = DateTime.UtcNow;

        // Save changes
        _dbContext.ProjectTasks.Update(task);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AssignTaskAsync(Guid taskId, Guid userId)
    {
        if (taskId == Guid.Empty)
        {
            throw new ArgumentException("Task ID cannot be empty", nameof(taskId));
        }

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty", nameof(userId));
        }

        // Find the task
        var task = await _dbContext.ProjectTasks
            .FirstOrDefaultAsync(t => t.Id == taskId && !t.IsDeleted);

        if (task == null)
        {
            throw new KeyNotFoundException($"Task with ID {taskId} not found.");
        }

        // Validate user exists
        var userExists = await _dbContext.Users
            .AnyAsync(u => u.Id == userId && !u.IsDeleted && u.IsActive);
            
        if (!userExists)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found or is inactive.");
        }

        // Update task assignment
        task.AssignedToId = userId;
        task.UpdatedAt = DateTime.UtcNow;

        // Save changes
        _dbContext.ProjectTasks.Update(task);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateTaskStatusAsync(Guid taskId, string requestStatus)
    {
        if (taskId == Guid.Empty)
        {
            throw new ArgumentException("Task ID cannot be empty", nameof(taskId));
        }

        if (string.IsNullOrWhiteSpace(requestStatus))
        {
            throw new ArgumentException("Status cannot be empty", nameof(requestStatus));
        }

        // Validate status is valid
        var validStatuses = new[] { "Todo", "InProgress", "Blocked", "Completed" };
        if (!validStatuses.Contains(requestStatus))
        {
            throw new ArgumentException($"Invalid status: {requestStatus}. Valid values are: {string.Join(", ", validStatuses)}");
        }

        // Find the task
        var task = await _dbContext.ProjectTasks
            .FirstOrDefaultAsync(t => t.Id == taskId && !t.IsDeleted);

        if (task == null)
        {
            throw new KeyNotFoundException($"Task with ID {taskId} not found.");
        }

        // Update task status
        task.Status = requestStatus;
        task.UpdatedAt = DateTime.UtcNow;

        // Save changes
        _dbContext.ProjectTasks.Update(task);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}