using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Web.Models;

public class Donor
{
    [Key]
    public int DonorId { get; set; }

    /// <summary>Null for anonymous / guest donors who never registered an account.</summary>
    public string? ApplicationUserId { get; set; }

    [ForeignKey(nameof(ApplicationUserId))]
    public ApplicationUser? ApplicationUser { get; set; }

    [Required, MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? PhoneNumber { get; set; }

    [MaxLength(20)]
    public string? IdNumber { get; set; }

    [MaxLength(20)]
    public string? TaxNumber { get; set; }

    [MaxLength(250)]
    public string? Address { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(100)]
    public string? Province { get; set; }

    [MaxLength(10)]
    public string? PostalCode { get; set; }

    public bool IsAnonymousDonor { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public ICollection<Donation> Donations { get; set; } = new List<Donation>();
}


