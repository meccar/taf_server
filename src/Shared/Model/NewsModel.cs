namespace Shared.Model;

public class NewsModel
{
    public string Id { get; set; } = null!;
    public required string Uuid { get; set; }
    public string ImageUrl { get; set; } = "";
    public string Title { get; set; } = "";
    public string SubTitle { get; set; } = "";
    public string Summary { get; set; } = "";
    public string Content { get; set; } = "";
    public bool IsPublished { get; set; } = false;
    public int CreatedByUserAccountId { get; set; }
    public int UpdatedByUserAccountId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    // public UserAccountModel? CreatedByUserAccount { get; set; }
    // public UserAccountModel? UpdatedByUserAccount { get; set; }
}