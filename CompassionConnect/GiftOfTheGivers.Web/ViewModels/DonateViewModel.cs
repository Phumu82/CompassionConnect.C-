using System.ComponentModel.DataAnnotations;
using GiftOfTheGivers.Web.Models;

namespace GiftOfTheGivers.Web.ViewModels;

public class DonateViewModel
{
    [Required, MaxLength(150), Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, Display(Name = "Email address")]
    public string Email { get; set; } = string.Empty;

    [Phone, Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "ID number")]
    public string? IdNumber { get; set; }

    [Required, Range(50, 1000000, ErrorMessage = "Enter an amount between R50 and R1,000,000.")]
    public decimal Amount { get; set; } = 500;

    public string Currency { get; set; } = "ZAR";

    [Display(Name = "Relief project")]
    public int? ProjectId { get; set; }

    [Display(Name = "Make this a monthly recurring donation")]
    public bool IsRecurring { get; set; }

    [Display(Name = "Donate anonymously")]
    public bool IsAnonymous { get; set; }

    [Required]
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Card;

    public IReadOnlyList<ReliefProject>? AvailableProjects { get; set; }
}


