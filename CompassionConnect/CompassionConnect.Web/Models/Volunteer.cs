using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompassionConnect.Web.Models;

public enum VolunteerStatus
{
    Pending = 0,
    Approved = 1,
    Placed = 2,
    Declined = 3
}

public enum Availability
{
    Weekdays = 0,
    Weekends = 1,
    EmergencyOnly = 2,
    Flexible = 3
}

public class Volunteer
{
    [Key]
    public int VolunteerId { get; set; }

    [Required, MaxLength(20)]
    public string ReferenceNumber { get; set; } = string.Empty;

    public string? ApplicationUserId { get; set; }

    [ForeignKey(nameof(ApplicationUserId))]
    public ApplicationUser? ApplicationUser { get; set; }

    [Required, MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? IdNumber { get; set; }

    [Required, MaxLength(100)]
    public string Province { get; set; } = string.Empty;

    /// <summary>Comma-separated list of skills, e.g. "Medical,Driving".</summary>
    [MaxLength(400)]
    public string Skills { get; set; } = string.Empty;

    [Required]
    public Availability Availability { get; set; } = Availability.Weekends;

    [Required]
    public VolunteerStatus Status { get; set; } = VolunteerStatus.Pending;

    public int? ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public ReliefProject? Project { get; set; }

    [MaxLength(1000)]
    public string? Motivation { get; set; }

    public DateTime DateApplied { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public IEnumerable<string> SkillList =>
        Skills.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
