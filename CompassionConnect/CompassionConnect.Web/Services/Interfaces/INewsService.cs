using CompassionConnect.Web.Models;

namespace CompassionConnect.Web.Services.Interfaces;

public interface INewsService
{
    Task<IReadOnlyList<NewsArticle>> GetAllAsync();
    Task<IReadOnlyList<NewsArticle>> GetLatestAsync(int count);
    Task<NewsArticle?> GetBySlugAsync(string slug);
}
