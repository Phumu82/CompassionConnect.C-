using CompassionConnect.Web.Models;

namespace CompassionConnect.Web.Repositories.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee?> GetByApplicationUserIdAsync(string applicationUserId);
}
