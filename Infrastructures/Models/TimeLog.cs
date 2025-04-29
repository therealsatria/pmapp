using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructures.Models
{
    public class TimeLog : BaseEntity
    {
        [Required]
        public Guid TaskId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime LogDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal HoursSpent { get; set; }

        public string Description { get; set; }

        // Navigation properties
        [ForeignKey("TaskId")]
        public virtual ProjectTask Task { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
} 