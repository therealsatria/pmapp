using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Dtos;

public class TaskCommentDto
{
    public Guid Id { get; set; }
    public string CommentText { get; set; }
    // public UserShortDto Author { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateTaskCommentDto
{
    [Required]
    public Guid TaskId { get; set; }

    [Required, StringLength(1000)]
    public string CommentText { get; set; }
}