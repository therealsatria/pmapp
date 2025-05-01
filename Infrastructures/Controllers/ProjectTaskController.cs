using Infrastructures.Dtos;
using Infrastructures.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace pmapp.Infrastructures.Controllers
{
    public class ProjectTaskController : Controller
    {
        private readonly IProjectTaskService _projectTaskService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public ProjectTaskController(
            IProjectTaskService projectTaskService,
            IProjectService projectService,
            IUserService userService)
        {
            _projectTaskService = projectTaskService ?? throw new ArgumentNullException(nameof(projectTaskService));
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Index(Guid projectId)
        {
            try
            {
                var tasks = await _projectTaskService.GetProjectTasksAsync(projectId);
                var project = await _projectService.GetProjectByIdAsync(projectId);
                
                ViewBag.ProjectId = projectId;
                ViewBag.ProjectName = project.ProjectName;
                
                return View(tasks);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading tasks: {ex.Message}";
                return RedirectToAction("Details", "Project", new { id = projectId });
            }
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Create(Guid projectId)
        {
            try
            {
                // Get project for breadcrumb and validation
                var project = await _projectService.GetProjectByIdAsync(projectId);
                if (project == null)
                {
                    return NotFound();
                }

                // Get users for dropdown
                var users = await _userService.GetAllUsersAsync();
                ViewBag.Users = users;
                ViewBag.ProjectId = projectId;
                ViewBag.ProjectName = project.ProjectName;
                
                // Initialize model with projectId
                var model = new CreateProjectTaskDto
                {
                    ProjectId = projectId,
                    Priority = "Medium" // Default priority
                };
                
                return View(model);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("Details", "Project", new { id = projectId });
            }
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProjectTaskDto taskDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Convert DueDate to UTC if provided
                    if (taskDto.DueDate.HasValue)
                    {
                        taskDto.DueDate = taskDto.DueDate.Value.ToUniversalTime();
                    }
                    
                    var createdTask = await _projectTaskService.CreateTaskAsync(taskDto);
                    return RedirectToAction(nameof(Index), new { projectId = taskDto.ProjectId });
                }
                catch (KeyNotFoundException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }
            
            // If we reach here, something went wrong. Repopulate the dropdown
            try
            {
                var project = await _projectService.GetProjectByIdAsync(taskDto.ProjectId);
                var users = await _userService.GetAllUsersAsync();
                ViewBag.Users = users;
                ViewBag.ProjectId = taskDto.ProjectId;
                ViewBag.ProjectName = project.ProjectName;
                return View(taskDto);
            }
            catch
            {
                // Fallback if we can't load the related data again
                return View(taskDto);
            }
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                // Note: You need to implement this method in IProjectTaskService
                var task = await _projectTaskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound();
                }
                
                var project = await _projectService.GetProjectByIdAsync(task.ProjectId);
                ViewBag.ProjectId = task.ProjectId;
                ViewBag.ProjectName = project.ProjectName;
                
                return View(task);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading task details: {ex.Message}";
                // When error occurs, redirect back to task list
                // We don't know the projectId here, so we'll have to fetch it from the service
                return RedirectToAction("Index", "Project");
            }
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                // Note: You need to implement this method in IProjectTaskService
                var task = await _projectTaskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound();
                }
                
                var project = await _projectService.GetProjectByIdAsync(task.ProjectId);
                var users = await _userService.GetAllUsersAsync();
                
                ViewBag.Users = users;
                ViewBag.ProjectId = task.ProjectId;
                ViewBag.ProjectName = project.ProjectName;
                
                // Map to update DTO 
                var updateModel = new UpdateProjectTaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    Priority = task.Priority,
                    DueDate = task.DueDate,
                    AssignedToId = task.AssignedTo?.Id,
                    Progress = task.Progress,
                    EstimatedHours = task.EstimatedHours,
                    Tags = task.Tags
                };
                
                return View(updateModel);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading task for editing: {ex.Message}";
                return RedirectToAction("Index", "Project");
            }
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateProjectTaskDto taskDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Convert DueDate to UTC if provided
                    if (taskDto.DueDate.HasValue)
                    {
                        taskDto.DueDate = taskDto.DueDate.Value.ToUniversalTime();
                    }
                    
                    // Note: You need to implement this method in IProjectTaskService
                    var updatedTask = await _projectTaskService.UpdateTaskAsync(taskDto);
                    
                    return RedirectToAction(nameof(Details), new { id = taskDto.Id });
                }
                catch (KeyNotFoundException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }
            
            // If we reach here, something went wrong
            try
            {
                // We need the task to get the project ID
                var task = await _projectTaskService.GetTaskByIdAsync(taskDto.Id);
                var project = await _projectService.GetProjectByIdAsync(task.ProjectId);
                var users = await _userService.GetAllUsersAsync();
                
                ViewBag.Users = users;
                ViewBag.ProjectId = task.ProjectId;
                ViewBag.ProjectName = project.ProjectName;
                
                return View(taskDto);
            }
            catch
            {
                return View(taskDto);
            }
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var task = await _projectTaskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound();
                }
                
                var project = await _projectService.GetProjectByIdAsync(task.ProjectId);
                ViewBag.ProjectId = task.ProjectId;
                ViewBag.ProjectName = project.ProjectName;
                
                return View(task);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading task for deletion: {ex.Message}";
                return RedirectToAction("Index", "Project");
            }
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                // Get the task first to know which project to redirect to after deletion
                var task = await _projectTaskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound();
                }
                
                // Store project ID for redirection after deletion
                var projectId = task.ProjectId;
                
                // Note: You need to implement this method in IProjectTaskService
                var result = await _projectTaskService.DeleteTaskAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Task deleted successfully.";
                    return RedirectToAction(nameof(Index), new { projectId });
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete task.";
                    return RedirectToAction(nameof(Index), new { projectId });
                }
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting task: {ex.Message}";
                return RedirectToAction("Index", "Project");
            }
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteTask(Guid id)
        {
            try
            {
                var result = await _projectTaskService.CompleteTaskAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Task marked as completed.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to complete task.";
                }
                
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error completing task: {ex.Message}";
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReopenTask(Guid id)
        {
            try
            {
                var result = await _projectTaskService.ReopenTaskAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Task reopened.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to reopen task.";
                }
                
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error reopening task: {ex.Message}";
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(Guid taskId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["ErrorMessage"] = "Comment cannot be empty.";
                return RedirectToAction(nameof(Details), new { id = taskId });
            }

            try
            {
                // Note: You would need to implement this in an ITaskCommentService
                // var comment = await _taskCommentService.AddCommentAsync(taskId, User.GetUserId(), content);
                
                // For now, just show a message that this is not implemented
                TempData["ErrorMessage"] = "Comment functionality is not yet implemented.";
                
                return RedirectToAction(nameof(Details), new { id = taskId });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error adding comment: {ex.Message}";
                return RedirectToAction(nameof(Details), new { id = taskId });
            }
        }
    }
} 