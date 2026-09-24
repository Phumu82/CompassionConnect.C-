using GiftOfTheGivers.Web.Repositories.Interfaces;
using GiftOfTheGivers.Web.Services.Interfaces;

namespace GiftOfTheGivers.Web.Services;

public class ReferenceNumberService : IReferenceNumberService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReferenceNumberService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> GenerateDonationReferenceAsync()
    {
        var year = DateTime.UtcNow.Year;
        string reference;
        do
        {
            var sequence = await _unitOfWork.Donations.CountForYearAsync(year) + 1;
            reference = $"DON-{year}-{sequence:D6}";
        } while (await _unitOfWork.Donations.ReferenceExistsAsync(reference));

        return reference;
    }

    public async Task<string> GenerateVolunteerReferenceAsync()
    {
        var year = DateTime.UtcNow.Year;
        string reference;
        do
        {
            var sequence = await _unitOfWork.Volunteers.CountForYearAsync(year) + 1;
            reference = $"VOL-{year}-{sequence:D6}";
        } while (await _unitOfWork.Volunteers.ReferenceExistsAsync(reference));

        return reference;
    }

    public async Task<string> GenerateTaxCertificateNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        string number;
        do
        {
            var sequence = await _unitOfWork.TaxCertificates.CountForYearAsync(year) + 1;
            number = $"GG18A-{year}-{sequence:D6}";
        } while (await _unitOfWork.TaxCertificates.CertificateNumberExistsAsync(number));

        return number;
    }
}


