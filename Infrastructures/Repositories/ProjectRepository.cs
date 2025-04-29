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

        var project = await _context.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null)
        {
            throw new KeyNotFoundException($"Project with ID {projectId} not found.");
        }
        return project;
    }
            

    

    
}