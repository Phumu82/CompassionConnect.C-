using CompassionConnect.Web.Models;

namespace CompassionConnect.Web.Repositories.Interfaces;

public interface IVolunteerRepository : IRepository<Volunteer>
{
    Task<Volunteer?> GetByReferenceAsync(string reference);
    Task<IReadOnlyList<Volunteer>> GetRecentAsync(int count);
    Task<bool> ReferenceExistsAsync(string reference);
    Task<int> CountForYearAsync(int year);
}
