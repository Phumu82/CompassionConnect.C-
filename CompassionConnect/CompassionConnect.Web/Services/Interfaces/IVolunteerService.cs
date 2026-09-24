using CompassionConnect.Web.Models;
using CompassionConnect.Web.ViewModels;

namespace CompassionConnect.Web.Services.Interfaces;

public interface IVolunteerService
{
    Task<Volunteer> RegisterAsync(VolunteerRegisterViewModel model, string? applicationUserId);
    Task<Volunteer?> GetByReferenceAsync(string reference);
    Task<IReadOnlyList<Volunteer>> GetAllAsync();
    Task<IReadOnlyList<Volunteer>> GetRecentAsync(int count = 10);
    Task UpdateStatusAsync(int volunteerId, VolunteerStatus status);
}
