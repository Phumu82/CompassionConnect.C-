using CompassionConnect.Web.Data;
using CompassionConnect.Web.Models;
using CompassionConnect.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompassionConnect.Web.Repositories;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Employee?> GetByApplicationUserIdAsync(string applicationUserId) =>
        await DbSet.FirstOrDefaultAsync(e => e.ApplicationUserId == applicationUserId);
}
