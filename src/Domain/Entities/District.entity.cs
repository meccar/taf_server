namespace Domain.Entities;

/// <summary>
/// Represents a district entity within a city.
/// This class stores details about a district, including its unique identifiers, name, and associated city.
/// </summary>
public class DistrictEntity : EntityBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the district entity.
    /// This is the primary key for the district entity and is used to uniquely identify each district.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the globally unique identifier (Ulid) for the district.
    /// This provides an alternative unique identifier for the district.
    /// </summary>
    public string Ulid { get; set; } = "";

    /// <summary>
    /// Gets or sets the name of the district.
    /// This property holds the full official name of the district.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Gets or sets the unique identifier of the city to which the district belongs.
    /// This property links the district to a specific city.
    /// </summary>
    public int CityId { get; set; }

    /// <summary>
    /// Gets or sets the associated <see cref="CityEntity"/> for the district.
    /// This property represents the city that the district belongs to.
    /// </summary>
    public CityEntity? City { get; set; }
}
