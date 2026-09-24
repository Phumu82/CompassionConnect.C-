using CompassionConnect.Web.Models;

namespace CompassionConnect.Web.Repositories.Interfaces;

public interface IDonorRepository : IRepository<Donor>
{
    Task<Donor?> GetByEmailAsync(string email);
    Task<Donor?> GetByApplicationUserIdAsync(string applicationUserId);
}
