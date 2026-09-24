using CompassionConnect.Web.Models;

namespace CompassionConnect.Web.Repositories.Interfaces;

public interface ITaxCertificateRepository : IRepository<TaxCertificate>
{
    Task<TaxCertificate?> GetByCertificateNumberAsync(string certificateNumber);
    Task<TaxCertificate?> GetByDonationIdAsync(int donationId);
    Task<IReadOnlyList<TaxCertificate>> GetByDonorIdAsync(int donorId);
    Task<bool> CertificateNumberExistsAsync(string certificateNumber);
    Task<int> CountForYearAsync(int year);
}
