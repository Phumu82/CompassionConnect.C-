using GiftOfTheGivers.Web.Models;

namespace GiftOfTheGivers.Web.Repositories.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee?> GetByApplicationUserIdAsync(string applicationUserId);
}


