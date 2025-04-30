using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Dtos;

/// <summary>
/// DTO for task attachments
/// </summary>
public class ProjectTaskAttachmentDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
    public long FileSize { get; set; }
    public string FileUrl { get; set; }
    public DateTime UploadedAt { get; set; }
    public UserShortDto UploadedBy { get; set; }
} 