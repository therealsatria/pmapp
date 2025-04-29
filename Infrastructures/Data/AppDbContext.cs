using Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructures.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet properties
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<TaskAttachment> TaskAttachments { get; set; }
        public DbSet<TimeLog> TimeLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Set PostgreSQL-specific column types
            modelBuilder.Entity<ProjectTask>()
                .Property(t => t.EstimatedHours)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<ProjectTask>()
                .Property(t => t.ActualHours)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<TimeLog>()
                .Property(tl => tl.HoursSpent)
                .HasColumnType("decimal(10,2)");
            
            // User entity configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(u => u.ProjectMembers)
                      .WithOne(pm => pm.User)
                      .HasForeignKey(pm => pm.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.CreatedTasks)
                      .WithOne(t => t.CreatedBy)
                      .HasForeignKey(t => t.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.AssignedTasks)
                      .WithOne(t => t.AssignedTo)
                      .HasForeignKey(t => t.AssignedToId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.TaskComments)
                      .WithOne(tc => tc.User)
                      .HasForeignKey(tc => tc.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.TaskAttachments)
                      .WithOne(ta => ta.UploadedBy)
                      .HasForeignKey(ta => ta.UploadedById)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.TimeLogs)
                      .WithOne(tl => tl.User)
                      .HasForeignKey(tl => tl.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Project entity configuration
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasMany(p => p.ProjectMembers)
                      .WithOne(pm => pm.Project)
                      .HasForeignKey(pm => pm.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Tasks)
                      .WithOne(t => t.Project)
                      .HasForeignKey(t => t.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.CreatedBy)
                      .WithMany()
                      .HasForeignKey(p => p.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ProjectTask entity configuration
            modelBuilder.Entity<ProjectTask>(entity =>
            {
                entity.HasOne(t => t.ParentTask)
                      .WithMany(t => t.ChildTasks)
                      .HasForeignKey(t => t.ParentTaskId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(t => t.TaskComments)
                      .WithOne(tc => tc.Task)
                      .HasForeignKey(tc => tc.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.TaskAttachments)
                      .WithOne(ta => ta.Task)
                      .HasForeignKey(ta => ta.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.TimeLogs)
                      .WithOne(tl => tl.Task)
                      .HasForeignKey(tl => tl.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Soft delete filter for all entities derived from BaseEntity
            ApplySoftDeleteFilter(modelBuilder);
        }

        private void ApplySoftDeleteFilter(ModelBuilder modelBuilder)
        {
            // Apply global query filter for soft delete to all entities that inherit from BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var condition = Expression.Equal(property, Expression.Constant(false));
                    var lambda = Expression.Lambda(condition, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
} 