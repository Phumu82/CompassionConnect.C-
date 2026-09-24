using CompassionConnect.Web.Models;

namespace CompassionConnect.Web.Services.Interfaces;

public interface ITaxCertificateService
{
    Task<TaxCertificate> IssueForDonationAsync(int donationId);
    Task<TaxCertificate?> GetByCertificateNumberAsync(string certificateNumber);
    Task<IReadOnlyList<TaxCertificate>> GetByDonorIdAsync(int donorId);

    /// <summary>Renders the certificate as a printable PDF byte array.</summary>
    Task<byte[]> GeneratePdfAsync(TaxCertificate certificate);
}
