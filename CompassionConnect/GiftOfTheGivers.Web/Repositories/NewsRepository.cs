using GiftOfTheGivers.Web.Data;
using GiftOfTheGivers.Web.Models;
using GiftOfTheGivers.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Web.Repositories;

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


