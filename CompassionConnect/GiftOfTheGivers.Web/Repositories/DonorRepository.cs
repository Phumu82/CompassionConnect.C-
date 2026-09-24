using GiftOfTheGivers.Web.Data;
using GiftOfTheGivers.Web.Models;
using GiftOfTheGivers.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Web.Repositories;

public class DonorRepository : Repository<Donor>, IDonorRepository
{
    public DonorRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Donor?> GetByEmailAsync(string email) =>
        await DbSet.FirstOrDefaultAsync(d => d.Email == email);

    public async Task<Donor?> GetByApplicationUserIdAsync(string applicationUserId) =>
        await DbSet.FirstOrDefaultAsync(d => d.ApplicationUserId == applicationUserId);
}


