using GiftOfTheGivers.Web.Data;
using GiftOfTheGivers.Web.Models;
using GiftOfTheGivers.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Web.Repositories;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Employee?> GetByApplicationUserIdAsync(string applicationUserId) =>
        await DbSet.FirstOrDefaultAsync(e => e.ApplicationUserId == applicationUserId);
}


