using GiftOfTheGivers.Web.Models;

namespace GiftOfTheGivers.Web.Repositories.Interfaces;

public interface INewsRepository : IRepository<NewsArticle>
{
    Task<NewsArticle?> GetBySlugAsync(string slug);
    Task<IReadOnlyList<NewsArticle>> GetFeaturedAsync();
}


