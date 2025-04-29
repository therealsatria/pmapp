using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructures.Models
{
    public class TaskAttachment : BaseEntity
    {
        [Required]
        public Guid TaskId { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public int FileSize { get; set; }

        [Required]
        public string FileType { get; set; }

        [Required]
        public Guid UploadedById { get; set; }

        [Required]
        public DateTime UploadedAt { get; set; }

        // Navigation properties
        [ForeignKey("TaskId")]
        public virtual ProjectTask Task { get; set; }

        [ForeignKey("UploadedById")]
        public virtual User UploadedBy { get; set; }
    }
} 