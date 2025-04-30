using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Dtos;

/// <summary>
/// DTO for task history records
/// </summary>
public class ProjectTaskHistoryDto
{
    public Guid Id { get; set; }
    public string ChangeType { get; set; }
    public string PreviousValue { get; set; }
    public string NewValue { get; set; }
    public DateTime ChangedAt { get; set; }
    public UserShortDto ChangedBy { get; set; }
}