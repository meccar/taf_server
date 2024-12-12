namespace Shared.Dtos.News;

public class GetDetailNewsResponseDto
{
    public required string Uuid { get; set; }
    public string ImageUrl { get; set; } = "";
    public string Title { get; set; } = "";
    public string SubTitle { get; set; } = "";
    public string Summary { get; set; } = "";
    public string Content { get; set; } = "";
    public bool IsPublished { get; set; } = false;
}