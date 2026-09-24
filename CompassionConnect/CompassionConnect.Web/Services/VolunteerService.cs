using CompassionConnect.Web.Models;
using CompassionConnect.Web.Repositories.Interfaces;
using CompassionConnect.Web.Services.Interfaces;
using CompassionConnect.Web.ViewModels;

namespace CompassionConnect.Web.Services;

public class VolunteerService : IVolunteerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReferenceNumberService _referenceNumberService;

    public VolunteerService(IUnitOfWork unitOfWork, IReferenceNumberService referenceNumberService)
    {
        _unitOfWork = unitOfWork;
        _referenceNumberService = referenceNumberService;
    }

    public async Task<Volunteer> RegisterAsync(VolunteerRegisterViewModel model, string? applicationUserId)
    {
        var volunteer = new Volunteer
        {
            ReferenceNumber = await _referenceNumberService.GenerateVolunteerReferenceAsync(),
            ApplicationUserId = applicationUserId,
            FullName = model.FullName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            IdNumber = model.IdNumber,
            Province = model.Province,
            Skills = string.Join(",", model.Skills ?? Array.Empty<string>()),
            Availability = model.Availability,
            ProjectId = model.ProjectId,
            Motivation = model.Motivation,
            Status = VolunteerStatus.Pending,
            DateApplied = DateTime.UtcNow
        };

        await _unitOfWork.Volunteers.AddAsync(volunteer);
        await _unitOfWork.CompleteAsync();
        return volunteer;
    }

    public async Task<Volunteer?> GetByReferenceAsync(string reference) =>
        await _unitOfWork.Volunteers.GetByReferenceAsync(reference);

    public async Task<IReadOnlyList<Volunteer>> GetAllAsync() =>
        await _unitOfWork.Volunteers.GetAllAsync();

    public async Task<IReadOnlyList<Volunteer>> GetRecentAsync(int count = 10) =>
        await _unitOfWork.Volunteers.GetRecentAsync(count);

    public async Task UpdateStatusAsync(int volunteerId, VolunteerStatus status)
    {
        var volunteer = await _unitOfWork.Volunteers.GetByIdAsync(volunteerId)
            ?? throw new InvalidOperationException($"Volunteer {volunteerId} not found.");
        volunteer.Status = status;
        _unitOfWork.Volunteers.Update(volunteer);
        await _unitOfWork.CompleteAsync();
    }
}
