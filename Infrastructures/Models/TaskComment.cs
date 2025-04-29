using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructures.Models
{
    public class TaskComment : BaseEntity
    {
        [Required]
        public Guid TaskId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string CommentText { get; set; }

        // Navigation properties
        [ForeignKey("TaskId")]
        public virtual ProjectTask Task { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
} 