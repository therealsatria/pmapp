using Infrastructures.Data;
using Infrastructures.Dtos;
using Infrastructures.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        var projects = await _context.Projects.AsNoTracking().ToListAsync();
        return projects ?? new List<Project>();
    }

    public async Task<Project> GetByIdAsync(Guid projectId)
    {
        if (projectId == Guid.Empty)
        {
            throw new ArgumentException("Project ID cannot be empty.", nameof(projectId));
        }

        var project = await _context.Projects
            .AsNoTracking()
            .Include(p => p.CreatedBy)
            .Include(p => p.ProjectMembers)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null)
        {
            throw new KeyNotFoundException($"Project with ID {projectId} not found.");
        }
        return project;
    }

    public async Task<Project> CreateAsync(Project project)
    {
        if (project == null)
        {
            throw new ArgumentNullException(nameof(project));
        }
        project.Id = Guid.NewGuid();
        project.CreatedAt = DateTime.UtcNow;

        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project> UpdateAsync(Guid projectId, Project project)
    {
        if (projectId == Guid.Empty)
        {
            throw new ArgumentException("Project ID cannot be empty.", nameof(projectId));
        }
        if (project == null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        var existingProject = await _context.Projects.FindAsync(projectId);
        if (existingProject == null)
        {
            throw new KeyNotFoundException($"Project with ID {projectId} not found.");
        }

        // Update properties from the input project
        existingProject.ProjectName = project.ProjectName;
        existingProject.Description = project.Description;
        existingProject.Status = project.Status;
        existingProject.StartDate = project.StartDate;
        existingProject.EndDate = project.EndDate;
        existingProject.CreatedById = project.CreatedById;
        existingProject.UpdatedAt = DateTime.UtcNow;
        
        _context.Projects.Update(existingProject);
        await _context.SaveChangesAsync();
        
        return existingProject;
    }

    public async Task DeleteAsync(Guid projectId)
    {
        if (projectId == Guid.Empty)
        {
            throw new ArgumentException("Project ID cannot be empty.", nameof(projectId));
        }
        var project = await GetByIdAsync(projectId);
        if (project == null)
        {
            throw new KeyNotFoundException($"Project with ID {projectId} not found.");
        }
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(Guid userId, string status = null)
    {
        var projects = await _context.Projects.AsNoTracking().Where(p => p.CreatedById == userId).ToListAsync();
        if (projects == null)
        {
            throw new KeyNotFoundException($"Project with CreatedById {userId} not found.");
        }
        if (status != null)
        {
            projects = projects.Where(p => p.Status == status).ToList();
        }
        return projects;
    }

    public async Task<IEnumerable<ProjectMember>> GetMembersByProjectAsync(Guid projectId)
    {
        var members = await _context.ProjectMembers.AsNoTracking().Where(m => m.ProjectId == projectId).ToListAsync();
        return members ?? new List<ProjectMember>();
    }   
    public async Task<ProjectMember> GetMembersAsync(Guid projectId, Guid userId)
    {
        var member = await _context.ProjectMembers.AsNoTracking().FirstOrDefaultAsync(m => m.ProjectId == projectId && m.UserId == userId);
        return member ?? new ProjectMember();
    }
    public async Task AddMemberAsync(ProjectMember member)
    {
        await _context.ProjectMembers.AddAsync(member);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateMemberRoleAsync(Guid memberId, string newRole)
    {
        var member = await _context.ProjectMembers.FindAsync(memberId);
        if (member == null)
        {
            throw new KeyNotFoundException($"Project member with ID {memberId} not found.");
        }
        member.Role = newRole;
        member.UpdatedAt = DateTime.UtcNow;
        _context.ProjectMembers.Update(member);
        await _context.SaveChangesAsync();
    }
    public async Task RemoveMemberAsync(Guid memberId)
    {
        var member = await _context.ProjectMembers.FindAsync(memberId);
        if (member == null)
        {
            throw new KeyNotFoundException($"Project member with ID {memberId} not found.");
        }
        _context.ProjectMembers.Remove(member);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> IsUserInProject(Guid userId, Guid projectId)
    {
        return await _context.ProjectMembers.AnyAsync(m => m.UserId == userId && m.ProjectId == projectId);
    }
    public async Task<int> GetProjectProgressAsync(Guid projectId)
    {
        var project = await GetByIdAsync(projectId);
        if (project == null)
        {
            throw new KeyNotFoundException($"Project with ID {projectId} not found.");
        }   
        var totalTasks = await _context.ProjectTasks.CountAsync(t => t.ProjectId == projectId);
        var completedTasks = await _context.ProjectTasks.CountAsync(t => t.ProjectId == projectId && t.Status == "Completed");
        return totalTasks > 0 ? (int)(completedTasks * 100 / totalTasks) : 0;
    }   
}