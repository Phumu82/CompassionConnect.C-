using CompassionConnect.Web.Models;
using CompassionConnect.Web.Repositories.Interfaces;
using CompassionConnect.Web.Services.Interfaces;
using CompassionConnect.Web.ViewModels;

namespace CompassionConnect.Web.Services;

/// <summary>Encapsulates the business rules for accepting a donation: resolving
/// or creating the donor, generating a reference number, persisting the
/// donation and issuing an instant Section 18A tax certificate.</summary>
public class DonationService : IDonationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReferenceNumberService _referenceNumberService;
    private readonly ITaxCertificateService _taxCertificateService;

    public DonationService(
        IUnitOfWork unitOfWork,
        IReferenceNumberService referenceNumberService,
        ITaxCertificateService taxCertificateService)
    {
        _unitOfWork = unitOfWork;
        _referenceNumberService = referenceNumberService;
        _taxCertificateService = taxCertificateService;
    }

    public async Task<Donation> ProcessDonationAsync(DonateViewModel model, string? applicationUserId)
    {
        var donor = await ResolveDonorAsync(model, applicationUserId);

        var donation = new Donation
        {
            ReferenceNumber = await _referenceNumberService.GenerateDonationReferenceAsync(),
            DonorId = donor.DonorId,
            ProjectId = model.ProjectId,
            Amount = model.Amount,
            Currency = string.IsNullOrWhiteSpace(model.Currency) ? "ZAR" : model.Currency,
            Type = model.IsRecurring ? DonationType.Recurring : DonationType.OneTime,
            PaymentMethod = model.PaymentMethod,
            Status = DonationStatus.Completed,
            IsAnonymous = model.IsAnonymous,
            DonationDate = DateTime.UtcNow
        };

        await _unitOfWork.Donations.AddAsync(donation);
        await _unitOfWork.CompleteAsync();

        if (model.ProjectId.HasValue)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(model.ProjectId.Value);
            if (project is not null)
            {
                project.RaisedAmount += donation.Amount;
                _unitOfWork.Projects.Update(project);
                await _unitOfWork.CompleteAsync();
            }
        }

        await _taxCertificateService.IssueForDonationAsync(donation.DonationId);

        return donation;
    }

    private async Task<Donor> ResolveDonorAsync(DonateViewModel model, string? applicationUserId)
    {
        if (!string.IsNullOrEmpty(applicationUserId))
        {
            var existing = await _unitOfWork.Donors.GetByApplicationUserIdAsync(applicationUserId);
            if (existing is not null) return existing;
        }

        var byEmail = await _unitOfWork.Donors.GetByEmailAsync(model.Email);
        if (byEmail is not null) return byEmail;

        var donor = new Donor
        {
            ApplicationUserId = applicationUserId,
            FullName = model.FullName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            IdNumber = model.IdNumber,
            IsAnonymousDonor = string.IsNullOrEmpty(applicationUserId),
            DateCreated = DateTime.UtcNow
        };

        await _unitOfWork.Donors.AddAsync(donor);
        await _unitOfWork.CompleteAsync();
        return donor;
    }

    public async Task<Donation?> GetByReferenceAsync(string reference) =>
        await _unitOfWork.Donations.GetByReferenceAsync(reference);

    public async Task<IReadOnlyList<Donation>> GetDonorHistoryAsync(int donorId) =>
        await _unitOfWork.Donations.GetByDonorIdAsync(donorId);

    public async Task<IReadOnlyList<Donation>> GetRecentAsync(int count = 10) =>
        await _unitOfWork.Donations.GetRecentAsync(count);

    public async Task<decimal> GetTotalRaisedAsync() => await _unitOfWork.Donations.GetTotalRaisedAsync();
}
