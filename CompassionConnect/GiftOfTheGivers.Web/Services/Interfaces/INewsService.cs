using GiftOfTheGivers.Web.Models;

namespace GiftOfTheGivers.Web.Services.Interfaces;

public interface INewsService
{
    Task<IReadOnlyList<NewsArticle>> GetAllAsync();
    Task<IReadOnlyList<NewsArticle>> GetLatestAsync(int count);
    Task<NewsArticle?> GetBySlugAsync(string slug);
}


