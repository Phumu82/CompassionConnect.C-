using CompassionConnect.Web.Models;

namespace CompassionConnect.Web.Repositories.Interfaces;

public interface INewsRepository : IRepository<NewsArticle>
{
    Task<NewsArticle?> GetBySlugAsync(string slug);
    Task<IReadOnlyList<NewsArticle>> GetFeaturedAsync();
}
