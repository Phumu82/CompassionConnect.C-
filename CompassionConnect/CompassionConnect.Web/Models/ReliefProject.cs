using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompassionConnect.Web.Models;

public enum ProjectStatus
{
    Planned = 0,
    Active = 1,
    OnHold = 2,
    Completed = 3
}

public enum ProjectCategory
{
    FloodRelief = 0,
    WildfireResponse = 1,
    MedicalAid = 2,
    FoodDistribution = 3,
    WaterAndSanitation = 4,
    ShelterAndRebuild = 5,
    GeneralEmergencyFund = 6
}

public class ReliefProject
{
    [Key]
    public int ProjectId { get; set; }

    [Required, MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(160)]
    public string Slug { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Province { get; set; } = string.Empty;

    [Required]
    public ProjectCategory Category { get; set; }

    [Required]
    public ProjectStatus Status { get; set; } = ProjectStatus.Active;

    [MaxLength(400)]
    public string Summary { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal GoalAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal RaisedAmount { get; set; }

    public int PeopleAssisted { get; set; }

    [MaxLength(300)]
    public string ImageUrl { get; set; } = "/images/campaigns/default-campaign.jpg";

    public bool IsEmergency { get; set; }

    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    public DateTime? EndDate { get; set; }

    public int? EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee? ProjectLead { get; set; }

    public ICollection<Donation> Donations { get; set; } = new List<Donation>();

    public ICollection<Volunteer> Volunteers { get; set; } = new List<Volunteer>();

    [NotMapped]
    public double PercentFunded => GoalAmount <= 0 ? 0 : Math.Min(100, (double)(RaisedAmount / GoalAmount * 100));
}
