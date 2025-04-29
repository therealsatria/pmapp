using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Models
{
    public class User : BaseEntity
    {
        public User()
        {
            ProjectMembers = new HashSet<ProjectMember>();
            CreatedTasks = new HashSet<ProjectTask>();
            AssignedTasks = new HashSet<ProjectTask>();
            TaskComments = new HashSet<TaskComment>();
            TaskAttachments = new HashSet<TaskAttachment>();
            TimeLogs = new HashSet<TimeLog>();
        }

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        
        [Required]
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        
        public string FullName { get; set; }

        public string ProfilePictureUrl { get; set; }

        public DateTime? LastLogin { get; set; }

        public string Role { get; set; } = "user";

        [Required]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<ProjectMember> ProjectMembers { get; set; }
        public virtual ICollection<ProjectTask> CreatedTasks { get; set; }
        public virtual ICollection<ProjectTask> AssignedTasks { get; set; }
        public virtual ICollection<TaskComment> TaskComments { get; set; }
        public virtual ICollection<TaskAttachment> TaskAttachments { get; set; }
        public virtual ICollection<TimeLog> TimeLogs { get; set; }
    }
} 