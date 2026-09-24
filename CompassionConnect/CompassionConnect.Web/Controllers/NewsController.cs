using CompassionConnect.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompassionConnect.Web.Controllers;

public class NewsController : Controller
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? category)
    {
        var articles = await _newsService.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(category) && category != "All")
            articles = articles.Where(a => a.Category.ToString() == category).ToList();

        return View(articles.OrderByDescending(a => a.PublishedDate).ToList());
    }

    [HttpGet]
    public async Task<IActionResult> Details(string slug)
    {
        var article = await _newsService.GetBySlugAsync(slug);
        if (article is null) return NotFound();
        return View(article);
    }
}
