using CompassionConnect.Web.Models;
using CompassionConnect.Web.ViewModels;

namespace CompassionConnect.Web.Services.Interfaces;

public interface IDonationService
{
    /// <summary>Processes a donation for a registered or guest/anonymous donor,
    /// creates the donor record if needed, and issues a Section 18A tax certificate.</summary>
    Task<Donation> ProcessDonationAsync(DonateViewModel model, string? applicationUserId);
    Task<Donation?> GetByReferenceAsync(string reference);
    Task<IReadOnlyList<Donation>> GetDonorHistoryAsync(int donorId);
    Task<IReadOnlyList<Donation>> GetRecentAsync(int count = 10);
    Task<decimal> GetTotalRaisedAsync();
}
