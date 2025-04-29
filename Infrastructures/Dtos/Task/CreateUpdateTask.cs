using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Dtos;

public class CreateTaskDto
{
    [Required, StringLength(100)]
    public string Title { get; set; }

    public string Description { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    public Guid? AssignedToId { get; set; }
    public string Priority { get; set; } = "Medium";
    public DateTime? DueDate { get; set; }
    public decimal? EstimatedHours { get; set; }
    public Guid? ParentTaskId { get; set; }
}

public class UpdateTaskDto
{
    [Required]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string Title { get; set; }

    public string Description { get; set; }
    public string Status { get; set; }
    public Guid? AssignedToId { get; set; }
    public DateTime? DueDate { get; set; }
}