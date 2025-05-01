using Infrastructures.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Infrastructures.Dtos;

namespace pmapp.Infrastructures.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public ProjectController(IProjectService projectService, IUserService userService)
        {
            _projectService = projectService;
            _userService = userService;
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

        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Create()
        {
            // Dapatkan daftar semua pengguna untuk dropdown
            var users = await _userService.GetAllUsersAsync();
            ViewBag.Users = users;
            
            var model = new CreateProjectDto
            {
                StartDate = DateTime.UtcNow, // Default to current UTC time
                CreatedById = User.FindFirst("sub")?.Value != null 
                              ? Guid.Parse(User.FindFirst("sub").Value) 
                              : Guid.Empty
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProjectDto projectDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure all dates are in UTC format to avoid PostgreSQL errors
                    projectDto.StartDate = projectDto.StartDate.ToUniversalTime();
                    if (projectDto.EndDate.HasValue)
                    {
                        projectDto.EndDate = projectDto.EndDate.Value.ToUniversalTime();
                    }
                    
                    // Pastikan CreatedById tidak kosong
                    if (projectDto.CreatedById == Guid.Empty)
                    {
                        ModelState.AddModelError("CreatedById", "Please select a user");
                        var users = await _userService.GetAllUsersAsync();
                        ViewBag.Users = users;
                        return View(projectDto);
                    }

                    var result = await _projectService.CreateProjectAsync(projectDto);
                    return RedirectToAction(nameof(Details), new { id = result.Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }
            
            // Jika validation error, kita perlu mengambil ulang daftar user
            var userList = await _userService.GetUserShorts();
            ViewBag.Users = userList;
            return View(projectDto);
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            // Dapatkan daftar semua pengguna untuk dropdown
            var users = await _userService.GetAllUsersAsync();
            ViewBag.Users = users;

            var updateProjectDto = new UpdateProjectDto
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status,
                CreatedById = project.CreatedBy?.Id ?? Guid.Empty
            };

            return View(updateProjectDto);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateProjectDto projectDto)
        {
            if (id != projectDto.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure all dates are in UTC format
                    if (projectDto.StartDate.HasValue)
                    {
                        projectDto.StartDate = projectDto.StartDate.Value.ToUniversalTime();
                    }
                    
                    if (projectDto.EndDate.HasValue)
                    {
                        projectDto.EndDate = projectDto.EndDate.Value.ToUniversalTime();
                    }
                    
                    // Pastikan CreatedById tidak kosong
                    if (projectDto.CreatedById == Guid.Empty)
                    {
                        ModelState.AddModelError("CreatedById", "Please select a user");
                        var users = await _userService.GetAllUsersAsync();
                        ViewBag.Users = users;
                        return View(projectDto);
                    }
                    
                    var result = await _projectService.UpdateProjectAsync(id, projectDto);
                    return RedirectToAction(nameof(Details), new { id = result.Id });
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }
            
            // Jika validation error, kita perlu mengambil ulang daftar user
            var userList = await _userService.GetAllUsersAsync();
            ViewBag.Users = userList;
            return View(projectDto);
        }
    
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            
            return View(project);
        }
        
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                await _projectService.DeleteProjectAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                var project = await _projectService.GetProjectByIdAsync(id);
                return View(project);
            }
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> AddMember(Guid id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            // Get all users for dropdown
            var users = await _userService.GetAllUsersAsync();
            ViewBag.Users = users;
            
            var model = new AddProjectMemberDto
            {
                ProjectId = id
            };
            
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(AddProjectMemberDto memberDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _projectService.AddMemberToProjectAsync(memberDto);
                    return RedirectToAction(nameof(Details), new { id = memberDto.ProjectId });
                }
                catch (KeyNotFoundException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }
            
            // If we get here, there was an error, so repopulate the dropdown
            var users = await _userService.GetAllUsersAsync();
            ViewBag.Users = users;
            
            return View(memberDto);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(Guid id, Guid memberId)
        {
            try
            {
                await _projectService.RemoveMemberFromProjectAsync(memberId);
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error removing member: {ex.Message}";
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMemberRole(Guid id, Guid memberId, string newRole)
        {
            try
            {
                var updateDto = new UpdateProjectMemberRoleDto
                {
                    MemberId = memberId,
                    NewRole = newRole
                };
                
                await _projectService.UpdateMemberRoleAsync(updateDto);
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating member role: {ex.Message}";
                return RedirectToAction(nameof(Details), new { id });
            }
        }
    }
}