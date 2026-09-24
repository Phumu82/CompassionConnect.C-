using System.ComponentModel.DataAnnotations;
using GiftOfTheGivers.Web.Models;

namespace GiftOfTheGivers.Web.ViewModels;

public class VolunteerRegisterViewModel
{
    [Required, MaxLength(150), Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, Phone, Display(Name = "Phone number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "ID number")]
    public string? IdNumber { get; set; }

    [Required]
    public string Province { get; set; } = string.Empty;

    [Display(Name = "Skills")]
    public string[]? Skills { get; set; }

    [Required]
    public Availability Availability { get; set; } = Availability.Weekends;

    [Display(Name = "Preferred project (optional)")]
    public int? ProjectId { get; set; }

    [MaxLength(1000), Display(Name = "Why do you want to volunteer with us?")]
    public string? Motivation { get; set; }

    public IReadOnlyList<ReliefProject>? AvailableProjects { get; set; }
}


