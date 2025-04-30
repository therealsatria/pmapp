using Infrastructures.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace pmapp.Infrastructures.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [Authorize(Roles = "user")]
        public async Task<IActionResult> Index()
        {
            var project = await _projectService.GetAllProjectsAsync();
            return View(project);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }
    }
}