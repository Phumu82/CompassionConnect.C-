using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Web.Models;

public enum NewsCategory
{
    DisasterResponse = 0,
    Healthcare = 1,
    Community = 2,
    Partnerships = 3,
    Reports = 4
}

public static class NewsCategoryExtensions
{
    public static string ToDisplayName(this NewsCategory category) => category switch
    {
        NewsCategory.DisasterResponse => "Disaster Response",
        _ => category.ToString()
    };
}

public class NewsArticle
{
    [Key]
    public int NewsArticleId { get; set; }

    [Required, MaxLength(160)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(160)]
    public string Slug { get; set; } = string.Empty;

    [Required]
    public NewsCategory Category { get; set; }

    [Required, MaxLength(100)]
    public string Author { get; set; } = string.Empty;

    public int ReadMinutes { get; set; } = 4;

    [MaxLength(500)]
    public string Excerpt { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    [MaxLength(300)]
    public string ImageUrl { get; set; } = "/images/news/default-news.jpg";

    [MaxLength(300)]
    public string ImageAlt { get; set; } = string.Empty;

    public bool Featured { get; set; }

    public DateTime PublishedDate { get; set; } = DateTime.UtcNow;
}


