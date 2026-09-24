using CompassionConnect.Web.Models;
using CompassionConnect.Web.Repositories.Interfaces;
using CompassionConnect.Web.Services.Interfaces;

namespace CompassionConnect.Web.Services;

public class NewsService : INewsService
{
    private readonly IUnitOfWork _unitOfWork;

    public NewsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<NewsArticle>> GetAllAsync() => await _unitOfWork.News.GetAllAsync();

    public async Task<IReadOnlyList<NewsArticle>> GetLatestAsync(int count)
    {
        var all = await _unitOfWork.News.GetAllAsync();
        return all.OrderByDescending(n => n.PublishedDate).Take(count).ToList();
    }

    public async Task<NewsArticle?> GetBySlugAsync(string slug) => await _unitOfWork.News.GetBySlugAsync(slug);
}
