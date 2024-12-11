namespace Domain.Entities;

/// <summary>
/// Represents a state entity within a country.
/// This class defines the properties of a state, including its unique identifier, name, initials, area code, and its associated country.
/// </summary>
public class StateEntity : EntityBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the state entity.
    /// This is a primary key used to uniquely identify the state in the system.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the universally unique identifier (ULID) for the state entity.
    /// This identifier is used to uniquely identify the state across different systems.
    /// </summary>
    public string Ulid { get; set; } = "";

    /// <summary>
    /// Gets or sets the name of the state.
    /// This property represents the full name of the state (e.g., "California").
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Gets or sets the initials for the state.
    /// This property typically represents the short form or abbreviation for the state's name (e.g., "CA" for California).
    /// </summary>
    public string Initials { get; set; } = "";

    /// <summary>
    /// Gets or sets the area code for the state.
    /// This property represents the telephone area code(s) associated with the state.
    /// </summary>
    public string AreaCode { get; set; } = "";

    /// <summary>
    /// Gets or sets the identifier for the country to which this state belongs.
    /// This is a foreign key linking the state to a specific country.
    /// </summary>
    public int CountryId { get; set; }

    /// <summary>
    /// Gets or sets the country entity that the state is part of.
    /// This property represents the associated country for the state (e.g., "United States").
    /// </summary>
    public CountryEntity? Country { get; set; }
}
