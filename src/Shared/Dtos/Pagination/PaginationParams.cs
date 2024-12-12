namespace Shared.Dtos.Pagination;

/// <summary>
/// Pagination parameters for filtering and sorting news
/// </summary>
public class PaginationParams
{
    /// <summary>
    /// Page number for pagination (default: 1)
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Number of items per page (default: 10, max: 50)
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Search term to filter news items
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Property to sort by (e.g., "Title", "PublishedDate")
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Sort direction (true for ascending, false for descending)
    /// </summary>
    public bool IsAscending { get; set; } = false;

    /// <summary>
    /// Category filter for news items
    /// </summary>
    public string? Category { get; set; }
}