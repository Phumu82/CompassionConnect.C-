namespace GiftOfTheGivers.Web.Repositories.Interfaces;

/// <summary>Aggregates repositories and coordinates a single SaveChanges transaction
/// per unit of work, per the Repository/Unit-of-Work pattern.</summary>
public interface IUnitOfWork : IDisposable
{
    IDonationRepository Donations { get; }
    IVolunteerRepository Volunteers { get; }
    IProjectRepository Projects { get; }
    IDonorRepository Donors { get; }
    IEmployeeRepository Employees { get; }
    ITaxCertificateRepository TaxCertificates { get; }
    INewsRepository News { get; }

    Task<int> CompleteAsync();
}


