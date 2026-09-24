using System.ComponentModel.DataAnnotations;
using GiftOfTheGivers.Web.Models;

namespace GiftOfTheGivers.Web.ViewModels;

public class ProjectEditViewModel
{
    public int ProjectId { get; set; }

    [Required, MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Province { get; set; } = string.Empty;

    [Required]
    public ProjectCategory Category { get; set; }

    [Required]
    public ProjectStatus Status { get; set; } = ProjectStatus.Active;

    [Required, MaxLength(400)]
    public string Summary { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    [Display(Name = "Funding goal (R)")]
    public decimal GoalAmount { get; set; }

    [Range(0, double.MaxValue)]
    [Display(Name = "Amount raised (R)")]
    public decimal RaisedAmount { get; set; }

    [Range(0, int.MaxValue)]
    [Display(Name = "People assisted")]
    public int PeopleAssisted { get; set; }

    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Emergency campaign")]
    public bool IsEmergency { get; set; }

    [Required, Display(Name = "Start date")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    [Display(Name = "End date")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Project lead")]
    public int? EmployeeId { get; set; }

    public IReadOnlyList<Employee>? AvailableEmployees { get; set; }
}


