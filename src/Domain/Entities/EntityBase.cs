using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Represents the base entity for all domain entities.
/// This class includes common properties such as the entity's creation, update, and deletion timestamps,
/// along with a flag indicating whether the entity is marked as deleted.
/// </summary>
public abstract class EntityBase : IEntityBase
{
    /// <summary>
    /// Gets or sets a value indicating whether the entity has been deleted.
    /// When set to <c>true</c>, the entity is considered deleted and should no longer be used.
    /// Default is <c>false</c>.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// This timestamp is typically set automatically when the entity is first created in the system.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was last updated.
    /// This timestamp is updated whenever any changes are made to the entity.
    /// A <c>null</c> value indicates that the entity has not been updated since its creation.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was deleted.
    /// This timestamp is set when an entity is marked as deleted. If the entity is not deleted,
    /// this property will be <c>null</c>.
    /// </summary>
    public DateTime? DeletedAt { get; set; } = null;
}

