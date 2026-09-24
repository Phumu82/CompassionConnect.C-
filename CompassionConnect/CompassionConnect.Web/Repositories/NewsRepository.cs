using CompassionConnect.Web.Data;
using CompassionConnect.Web.Models;
using CompassionConnect.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompassionConnect.Web.Repositories;

public class NewsRepository : Repository<NewsArticle>, INewsRepository
{
    public NewsRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<NewsArticle?> GetBySlugAsync(string slug) =>
        await DbSet.FirstOrDefaultAsync(n => n.Slug == slug);

    public async Task<IReadOnlyList<NewsArticle>> GetFeaturedAsync() =>
        await DbSet.Where(n => n.Featured).OrderByDescending(n => n.PublishedDate).ToListAsync();
}
