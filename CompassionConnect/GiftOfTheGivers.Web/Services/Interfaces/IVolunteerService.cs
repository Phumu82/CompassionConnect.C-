using GiftOfTheGivers.Web.Models;
using GiftOfTheGivers.Web.ViewModels;

namespace GiftOfTheGivers.Web.Services.Interfaces;

public interface IVolunteerService
{
    Task<Volunteer> RegisterAsync(VolunteerRegisterViewModel model, string? applicationUserId);
    Task<Volunteer?> GetByReferenceAsync(string reference);
    Task<IReadOnlyList<Volunteer>> GetAllAsync();
    Task<IReadOnlyList<Volunteer>> GetRecentAsync(int count = 10);
    Task UpdateStatusAsync(int volunteerId, VolunteerStatus status);
}


