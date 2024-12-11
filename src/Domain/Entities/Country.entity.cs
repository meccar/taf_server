namespace Domain.Entities;

/// <summary>
/// Represents a country entity in the system.
/// This class stores details about a country, including its unique identifiers, name, and initials.
/// </summary>
public class CountryEntity : EntityBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the country entity.
    /// This is the primary key for the country entity and is used to uniquely identify each country.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the globally unique identifier (Ulid) for the country.
    /// This provides an alternative unique identifier for the country.
    /// </summary>
    public string Ulid { get; set; } = "";

    /// <summary>
    /// Gets or sets the name of the country.
    /// This property holds the full official name of the country.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Gets or sets the initials or short code for the country.
    /// This property holds a short form or code representing the country (e.g., "US" for the United States).
    /// </summary>
    public string Initials { get; set; } = "";
}
