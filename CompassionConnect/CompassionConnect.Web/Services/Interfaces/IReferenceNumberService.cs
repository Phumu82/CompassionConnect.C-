namespace CompassionConnect.Web.Services.Interfaces;

/// <summary>Generates unique, human-readable reference numbers such as
/// DON-2026-000001, VOL-2026-000001 and GG18A-2026-000001.</summary>
public interface IReferenceNumberService
{
    Task<string> GenerateDonationReferenceAsync();
    Task<string> GenerateVolunteerReferenceAsync();
    Task<string> GenerateTaxCertificateNumberAsync();
}
