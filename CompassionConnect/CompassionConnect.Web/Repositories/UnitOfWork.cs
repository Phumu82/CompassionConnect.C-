using CompassionConnect.Web.Data;
using CompassionConnect.Web.Repositories.Interfaces;

namespace CompassionConnect.Web.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Donations = new DonationRepository(_context);
        Volunteers = new VolunteerRepository(_context);
        Projects = new ProjectRepository(_context);
        Donors = new DonorRepository(_context);
        Employees = new EmployeeRepository(_context);
        TaxCertificates = new TaxCertificateRepository(_context);
        News = new NewsRepository(_context);
    }

    public IDonationRepository Donations { get; }
    public IVolunteerRepository Volunteers { get; }
    public IProjectRepository Projects { get; }
    public IDonorRepository Donors { get; }
    public IEmployeeRepository Employees { get; }
    public ITaxCertificateRepository TaxCertificates { get; }
    public INewsRepository News { get; }

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
