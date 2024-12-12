using Shared.Dtos.Pagination;

namespace Shared.Helpers;

/// <summary>
/// A generic pagination helper that can be used across different types and data sources
/// </summary>
/// <typeparam name="T">The type of items to paginate</typeparam>
public class PaginationHelper<T>
{ 
    // Configuration default constants
    private const int DefaultPageNumber = 1;
    private const int DefaultMaxPageSize = 50;
    private const int DefaultPageSize = 10;

    // Pagination parameters
    public int PageNumber { get; }
    public int PageSize { get; }
        
    // Filtering and sorting parameters
    public string SearchTerm { get; }
    public string SortBy { get; }
    public bool IsAscending { get; }
    public string Category { get; }

    /// <summary>
    /// Create a new pagination helper with optional parameters
    /// </summary>
    public PaginationHelper(
        int? pageNumber = null, 
        int? pageSize = null, 
        string searchTerm = null, 
        string sortBy = null, 
        bool isAscending = true,
        string category = null)
    {
        // Validate and set page number
        PageNumber = Math.Max(pageNumber ?? DefaultPageNumber, 1);
            
        // Validate and set page size
        PageSize = Math.Clamp(
            pageSize ?? DefaultPageSize, 
            1, 
            DefaultMaxPageSize
        );

        // Set optional filtering and sorting parameters
        SearchTerm = searchTerm?.Trim();
        SortBy = sortBy;
        IsAscending = isAscending;
        Category = category;
    }

    // Async version for more flexibility
    public PaginationResponse<T> Paginate(IEnumerable<T> source)
    {
        IEnumerable<T> query = source.AsEnumerable();

        // Apply search if search term is provided
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            query = ApplySearch(query, SearchTerm);
        }

        // Apply category filtering if category is provided
        if (!string.IsNullOrEmpty(Category))
            query = ApplyCategoryFilter(query, Category);

        // Apply sorting if sort column is specified
        if (!string.IsNullOrEmpty(SortBy))
            query = ApplySorting(query);

        // Use provided count function or default to Count()
        int totalItems = query.Count();
        int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);

        // Apply pagination
        var paginatedItems = query
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        return new PaginationResponse<T>
        {
            Items = paginatedItems,
            PageNumber = PageNumber,
            PageSize = PageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            HasPreviousPage = PageNumber > 1,
            HasNextPage = PageNumber < totalPages
        };
    }

    /// <summary>
    /// Asynchronous version of Paginate for async data sources
    /// </summary>
    public async Task<PaginationResponse<T>> PaginateAsync(IQueryable<T> source)
    {
        // Similar implementation to Paginate, but for IQueryable
        return await Task.Run(() => Paginate(source.AsEnumerable()));
    }
    
    /// <summary>
    /// Apply category filtering based on a matching property
    /// </summary>
    private IEnumerable<T> ApplyCategoryFilter(IEnumerable<T> query, string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return query;

        var categoryProperty = typeof(T)
            .GetProperties()
            .FirstOrDefault(p => 
                p.Name.Equals("Category", StringComparison.OrdinalIgnoreCase) ||
                p.Name.EndsWith("Category", StringComparison.OrdinalIgnoreCase));

        if (categoryProperty == null)
            return query;

        return query.Where(item =>
        {
            var propertyValue = categoryProperty.GetValue(item);
            return propertyValue != null && 
                   propertyValue.ToString() == category;
        });
    }

    /// <summary>
    /// Apply search across string properties
    /// </summary>
    private IEnumerable<T> ApplySearch(IEnumerable<T> query, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return query;

        return query.Where(item =>
        {
            // Get all string properties
            var stringProperties = typeof(T)
                .GetProperties()
                .Where(p => p.PropertyType == typeof(string));

            // Check if any string property contains the search term
            return stringProperties.Any(prop =>
            {
                var value = prop.GetValue(item) as string;
                return value != null && 
                       value.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    /// <summary>
    /// Apply sorting based on a specified property
    /// </summary>
    private IEnumerable<T> ApplySorting(IEnumerable<T> query)
    {
        if (string.IsNullOrWhiteSpace(SortBy))
            return query;

        // Find the property to sort by (case-insensitive)
        var sortProperty = typeof(T)
            .GetProperties()
            .FirstOrDefault(p => 
                p.Name.Equals(SortBy, StringComparison.OrdinalIgnoreCase));

        if (sortProperty == null)
            return query;

        // Use reflection to create a sorting function
        return IsAscending
            ? query.OrderBy(x => sortProperty.GetValue(x))
            : query.OrderByDescending(x => sortProperty.GetValue(x));
    }
}
