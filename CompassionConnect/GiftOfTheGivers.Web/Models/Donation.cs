using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Web.Models;

public enum DonationType
{
    OneTime = 0,
    Recurring = 1
}

public enum DonationStatus
{
    Pending = 0,
    Completed = 1,
    Refunded = 2,
    Failed = 3
}

public enum PaymentMethod
{
    Card = 0,
    Eft = 1,
    InstantEft = 2,
    SnapScan = 3
}

public class Donation
{
    [Key]
    public int DonationId { get; set; }

    /// <summary>Human readable unique reference, e.g. DON-2026-000001.</summary>
    [Required, MaxLength(20)]
    public string ReferenceNumber { get; set; } = string.Empty;

    [Required]
    public int DonorId { get; set; }

    [ForeignKey(nameof(DonorId))]
    public Donor? Donor { get; set; }

    public int? ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public ReliefProject? Project { get; set; }

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required, MaxLength(5)]
    public string Currency { get; set; } = "ZAR";

    [Required]
    public DonationType Type { get; set; } = DonationType.OneTime;

    [Required]
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Card;

    [Required]
    public DonationStatus Status { get; set; } = DonationStatus.Completed;

    public bool IsAnonymous { get; set; }

    public DateTime DonationDate { get; set; } = DateTime.UtcNow;

    public TaxCertificate? TaxCertificate { get; set; }
}


