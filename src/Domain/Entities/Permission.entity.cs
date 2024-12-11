using Shared.Enums;

namespace Domain.Entities;

/// <summary>
/// Represents a permission entity in the system.
/// This class defines the attributes and characteristics of a permission, including its unique identifier, slug, description, 
/// request method type, and its active status.
/// </summary>
public class PermissionEntity : EntityBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the permission entity.
    /// This is a primary key used to uniquely identify the permission in the system.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the universally unique identifier (ULID) for the permission entity.
    /// This identifier is used to uniquely identify the permission across different systems.
    /// </summary>
    public string Ulid { get; set; } = "";

    /// <summary>
    /// Gets or sets the slug for the permission.
    /// The slug is a short, URL-friendly identifier used to refer to the permission in the system.
    /// </summary>
    public string Slug { get; set; } = "";

    /// <summary>
    /// Gets or sets the description of the permission.
    /// This provides additional details or context about what the permission allows or controls.
    /// </summary>
    public string Description { get; set; } = "";

    /// <summary>
    /// Gets or sets the request method associated with the permission.
    /// This value is an enumeration representing the HTTP method (e.g., GET, POST, PUT, DELETE) that the permission applies to.
    /// This property is optional and can be <c>null</c>.
    /// </summary>
    public ERequestMethod? Method { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the permission is active.
    /// When set to <c>true</c>, the permission is considered active and can be assigned to users or roles.
    /// When set to <c>false</c>, the permission is considered inactive and will not be used.
    /// </summary>
    public bool IsActive { get; set; }
}
