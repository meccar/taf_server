namespace Shared.Dtos.News;

/// <summary>
/// DTO for Updating a news item.
/// </summary>
public class UpdateNewsRequestDto
{
    /// <summary>
    /// News's Image Url
    /// </summary>
    /// <example>https://example.com/news-image.jpg</example>
    public string ImageUrl { get; set; } = "";
    
    /// <summary>
    /// News's Title
    /// </summary>
    /// <example>Breaking News: Market Hits All-Time High</example>
    public string Title { get; set; } = "";
    
    /// <summary>
    /// News's SubTitle
    /// </summary>
    /// <example>Key insights into the record-breaking stock market surge</example>
    public string SubTitle { get; set; } = "";
    
    /// <summary>
    /// News's Summary
    /// </summary>
    /// <example>The stock market has reached an unprecedented level, driven by tech growth.</example>
    public string Summary { get; set; } = "";
    
    /// <summary>
    /// News's Content
    /// </summary>
    /// <example>The stock market hit a new milestone today, with major indices...</example>
    public string Content { get; set; } = "";
    
    /// <summary>
    /// Is News Published
    /// </summary>
    /// <example>true</example>
    public bool IsPublished { get; set; } = false;
}