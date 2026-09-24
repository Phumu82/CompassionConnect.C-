using GiftOfTheGivers.Web.Models;

namespace GiftOfTheGivers.Web.Repositories.Interfaces;

public interface IDonationRepository : IRepository<Donation>
{
    Task<Donation?> GetByReferenceAsync(string reference);
    Task<IReadOnlyList<Donation>> GetByDonorIdAsync(int donorId);
    Task<IReadOnlyList<Donation>> GetRecentAsync(int count);
    Task<bool> ReferenceExistsAsync(string reference);
    Task<int> CountForYearAsync(int year);
    Task<decimal> GetTotalRaisedAsync();
}


