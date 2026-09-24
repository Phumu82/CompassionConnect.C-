using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Web.Models;

/// <summary>South African Section 18A donation tax certificate.</summary>
public class TaxCertificate
{
    [Key]
    public int TaxCertificateId { get; set; }

    [Required, MaxLength(30)]
    public string CertificateNumber { get; set; } = string.Empty;

    [Required]
    public int DonationId { get; set; }

    [ForeignKey(nameof(DonationId))]
    public Donation? Donation { get; set; }

    [Required]
    public int DonorId { get; set; }

    [ForeignKey(nameof(DonorId))]
    public Donor? Donor { get; set; }

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public DateTime DateIssued { get; set; } = DateTime.UtcNow;

    [MaxLength(20)]
    public string TaxYear => DateIssued.Year.ToString();
}


