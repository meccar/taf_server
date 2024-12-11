namespace Domain.Entities;

/// <summary>
/// Represents a city entity in the system.
/// This class is used to store information about a city, including its unique identifier, name, initials, code, and the state it belongs to.
/// </summary>
public class CityEntity : EntityBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the city entity.
    /// This is the primary key for the city entity and is used to uniquely identify each city.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier (Ulid) for the city entity.
    /// This is a globally unique identifier that can be used as an alternative to the integer `Id`.
    /// </summary>
    public string Ulid { get; set; } = "";
    
    /// <summary>
    /// Gets or sets the name of the city.
    /// This is the full name of the city, e.g., "New York", "Los Angeles", etc.
    /// </summary>
    public string Name { get; set; } = "";
    
    /// <summary>
    /// Gets or sets the initials of the city.
    /// This is a short abbreviation or code representing the city, e.g., "NY" for New York.
    /// </summary>
    public string Initials { get; set; } = "";
    
    /// <summary>
    /// Gets or sets the code of the city.
    /// This code can be used to represent the city in other systems or for organizational purposes.
    /// </summary>
    public string Code { get; set; } = "";
    
    /// <summary>
    /// Gets or sets the ID of the state to which the city belongs.
    /// This establishes a relationship between the city and the state it is located in.
    /// </summary>
    public int StateId { get; set; }
    
    /// <summary>
    /// Gets or sets the state entity associated with the city.
    /// This property represents the state that the city belongs to. It is an optional property, as a city may or may not have an associated state.
    /// </summary>
    public StateEntity? State { get; set; }
}
