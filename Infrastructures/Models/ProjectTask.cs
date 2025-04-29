using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructures.Models
{
    public class ProjectTask : BaseEntity
    {
        public ProjectTask()
        {
            TaskComments = new HashSet<TaskComment>();
            TaskAttachments = new HashSet<TaskAttachment>();
            TimeLogs = new HashSet<TimeLog>();
            ChildTasks = new HashSet<ProjectTask>();
        }

        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Priority { get; set; }

        public DateTime? DueDate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? EstimatedHours { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? ActualHours { get; set; }

        [Required]
        public Guid CreatedById { get; set; }

        public Guid? AssignedToId { get; set; }

        public Guid? ParentTaskId { get; set; }

        // Navigation properties
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("CreatedById")]
        public virtual User CreatedBy { get; set; }

        [ForeignKey("AssignedToId")]
        public virtual User AssignedTo { get; set; }

        [ForeignKey("ParentTaskId")]
        public virtual ProjectTask ParentTask { get; set; }

        public virtual ICollection<ProjectTask> ChildTasks { get; set; }
        public virtual ICollection<TaskComment> TaskComments { get; set; }
        public virtual ICollection<TaskAttachment> TaskAttachments { get; set; }
        public virtual ICollection<TimeLog> TimeLogs { get; set; }
    }
} 