using Infrastructures.Dtos;
using Infrastructures.Models;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using Infrastructures.Data;
using AutoMapper;

namespace Infrastructures.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public ProjectService(
        IProjectRepository projectRepository, 
        IUserRepository userRepository,
        AppDbContext dbContext,
        IMapper mapper)
    {
        _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
                Id = m.Id,
                UserId = m.UserId,
                Username = m.User?.Username,
                FullName = m.User?.FullName,
                ProfilePictureUrl = m.User?.ProfilePictureUrl,
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
                Id = m.Id,
                UserId = m.UserId,
                Username = m.User?.Username,
                FullName = m.User?.FullName,
                ProfilePictureUrl = m.User?.ProfilePictureUrl,
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
                Id = m.Id,
                UserId = m.UserId,
                Username = m.User?.Username,
                FullName = m.User?.FullName,
                ProfilePictureUrl = m.User?.ProfilePictureUrl,
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
                Id = m.Id,
                UserId = m.UserId,
                Username = m.User?.Username,
                FullName = m.User?.FullName,
                ProfilePictureUrl = m.User?.ProfilePictureUrl,
                Role = m.Role,
                JoinedAt = m.JoinedAt,
                ProjectId = m.ProjectId
            })
        };
    }

    public async Task<bool> DeleteProjectAsync(Guid projectId)
    {
        if (projectId == Guid.Empty)
        {
            throw new ArgumentException("Project ID cannot be empty", nameof(projectId));
        }

        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
        {
            throw new KeyNotFoundException($"Project with ID {projectId} not found.");
        }

        await _projectRepository.DeleteAsync(projectId);
        return true;
    }

    //======================Project Member==========================================================

    public async Task<ProjectMemberDto> AddMemberToProjectAsync(AddProjectMemberDto memberDto)
    {
        if (memberDto == null)
        {
            throw new ArgumentNullException(nameof(memberDto));
        }

        // Validate project exists using LINQ
        var projectExists = await _dbContext.Projects
            .AnyAsync(p => p.Id == memberDto.ProjectId && !p.IsDeleted);
            
        if (!projectExists)
        {
            throw new KeyNotFoundException($"Project with ID {memberDto.ProjectId} not found.");
        }

        // Validate user exists using LINQ
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == memberDto.UserId && !u.IsDeleted && u.IsActive);
            
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {memberDto.UserId} not found or is inactive.");
        }

        // Check if user is already a member of the project using LINQ
        var isAlreadyMember = await _dbContext.ProjectMembers
            .AnyAsync(pm => 
                pm.ProjectId == memberDto.ProjectId && 
                pm.UserId == memberDto.UserId && 
                !pm.IsDeleted);
                                      
        if (isAlreadyMember)
        {
            throw new InvalidOperationException($"User {user.Username} is already a member of this project.");
        }

        // Create new ProjectMember entity
        var newMember = new ProjectMember
        {
            Id = Guid.NewGuid(),
            ProjectId = memberDto.ProjectId,
            UserId = memberDto.UserId,
            Role = memberDto.Role,
            JoinedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        // Add to database and save changes
        await _dbContext.ProjectMembers.AddAsync(newMember);
        await _dbContext.SaveChangesAsync();

        // Fetch the newly created member with related user data using Include
        var createdMember = await _dbContext.ProjectMembers
            .Include(pm => pm.User)
            .FirstOrDefaultAsync(pm => pm.Id == newMember.Id);
            
        if (createdMember == null)
        {
            throw new InvalidOperationException("Failed to retrieve the newly created project member.");
        }
        
        // Count assigned and completed tasks for the user in this project
        var assignedTasksCount = await _dbContext.ProjectTasks
            .CountAsync(pt => 
                pt.ProjectId == memberDto.ProjectId && 
                pt.AssignedToId == memberDto.UserId && 
                !pt.IsDeleted);
                
        var completedTasksCount = await _dbContext.ProjectTasks
            .CountAsync(pt => 
                pt.ProjectId == memberDto.ProjectId && 
                pt.AssignedToId == memberDto.UserId && 
                pt.Status == "Completed" && 
                !pt.IsDeleted);

        // Use AutoMapper to map the entity to DTO
        var resultDto = _mapper.Map<ProjectMemberDto>(createdMember);
        
        // Set additional properties that aren't directly mapped
        resultDto.AssignedTasksCount = assignedTasksCount;
        resultDto.CompletedTasksCount = completedTasksCount;
        resultDto.IsActive = true;
        
        return resultDto;
    }

    public async Task<bool> RemoveMemberFromProjectAsync(Guid memberId)
    {
        if (memberId == Guid.Empty)
        {
            throw new ArgumentException("Member ID cannot be empty", nameof(memberId));
        }

        // Find the project member by ID
        var projectMember = await _dbContext.ProjectMembers
            .FirstOrDefaultAsync(pm => pm.Id == memberId && !pm.IsDeleted);

        if (projectMember == null)
        {
            throw new KeyNotFoundException($"Project member with ID {memberId} not found.");
        }

        // Get project and user info for activity log
        var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectMember.ProjectId);
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == projectMember.UserId);

        // Perform soft delete instead of hard delete to maintain referential integrity
        projectMember.IsDeleted = true;
        projectMember.UpdatedAt = DateTime.UtcNow;

        // Update any task assignments (optional - mark tasks as unassigned)
        var assignedTasks = await _dbContext.ProjectTasks
            .Where(pt => pt.ProjectId == projectMember.ProjectId && 
                       pt.AssignedToId == projectMember.UserId && 
                       !pt.IsDeleted)
            .ToListAsync();

        if (assignedTasks.Any())
        {
            foreach (var task in assignedTasks)
            {
                task.AssignedToId = null;
                task.UpdatedAt = DateTime.UtcNow;
            }
        }

        // Save changes to the database
        await _dbContext.SaveChangesAsync();

        // Note: Activity logging would be implemented here when LogProjectActivityAsync is available
        // Example: await LogProjectActivityAsync(projectMember.ProjectId, "MemberRemoved", 
        //    $"User {user?.Username ?? "Unknown"} was removed from project {project?.ProjectName ?? "Unknown"}", projectMember.UserId);

        return true;
    }

    public async Task<ProjectMemberDto> UpdateMemberRoleAsync(UpdateProjectMemberRoleDto updateDto)
    {
        if (updateDto == null)
        {
            throw new ArgumentNullException(nameof(updateDto));
        }

        if (updateDto.MemberId == Guid.Empty)
        {
            throw new ArgumentException("Member ID cannot be empty", nameof(updateDto.MemberId));
        }

        if (string.IsNullOrWhiteSpace(updateDto.NewRole))
        {
            throw new ArgumentException("New role cannot be empty", nameof(updateDto.NewRole));
        }

        // Find the project member by ID
        var projectMember = await _dbContext.ProjectMembers
            .Include(pm => pm.User)
            .Include(pm => pm.Project)
            .FirstOrDefaultAsync(pm => pm.Id == updateDto.MemberId && !pm.IsDeleted);

        if (projectMember == null)
        {
            throw new KeyNotFoundException($"Project member with ID {updateDto.MemberId} not found.");
        }

        // Update the role
        projectMember.Role = updateDto.NewRole;
        projectMember.UpdatedAt = DateTime.UtcNow;

        // Save changes to the database
        await _dbContext.SaveChangesAsync();

        // Count assigned and completed tasks for statistics
        var assignedTasksCount = await _dbContext.ProjectTasks.CountAsync(
            pt => pt.ProjectId == projectMember.ProjectId && 
                 pt.AssignedToId == projectMember.UserId && 
                 !pt.IsDeleted);
                
        var completedTasksCount = await _dbContext.ProjectTasks.CountAsync(
            pt => pt.ProjectId == projectMember.ProjectId && 
                 pt.AssignedToId == projectMember.UserId && 
                 pt.Status == "Completed" && 
                 !pt.IsDeleted);

        // Map to DTO using AutoMapper
        var memberDto = _mapper.Map<ProjectMemberDto>(projectMember);
        
        // Set additional properties that aren't mapped automatically
        memberDto.AssignedTasksCount = assignedTasksCount;
        memberDto.CompletedTasksCount = completedTasksCount;
        memberDto.IsActive = !projectMember.IsDeleted;
        
        // Note: Activity logging would be implemented here when LogProjectActivityAsync is available
        // Example: await LogProjectActivityAsync(
        //     projectMember.ProjectId, 
        //     "MemberRoleUpdated",
        //     $"User {projectMember.User?.Username ?? "Unknown"} role changed to {updateDto.NewRole}",
        //     projectMember.UserId);

        return memberDto;
    }

    //============================not implemented=========================================================

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

    

    Task<bool> IProjectService.UpdateProjectStatusAsync(Guid projectId, string newStatus)
    {
        throw new NotImplementedException();
    }
}