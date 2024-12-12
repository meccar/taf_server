namespace Shared.Dtos.News;

/// <summary>
/// DTO for returning news items in a paginated response
/// </summary>
public class GetAllNewsResponseDto
{
    /// <summary>
    /// Unique identifier for the news item
    /// </summary>
    public required string Uuid { get; set; }
    
    public string ImageUrl { get; set; } = "";
    
    /// <summary>
    /// Title of the news article
    /// </summary>
    public string Title { get; set; } = "";
    public string SubTitle { get; set; } = "";
    
    /// <summary>
    /// Short summary or excerpt of the news article
    /// </summary>
    public string Summary { get; set; } = "";
    public string Content { get; set; } = "";
    public bool IsPublished { get; set; } = false;
}