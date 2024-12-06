namespace Domain.SeedWork.Interfaces;

/// <summary>
/// Represents entities that support soft deletion.
/// </summary>
/// <remarks>
/// The <see cref="ISoftDeletable"/> interface is used for entities that can be logically deleted rather than physically removed from the database.
/// Soft deletion marks an entity as deleted by setting the <see cref="IsDeleted"/> flag to <c>true</c> and optionally records the time of deletion in <see cref="DeletedAt"/>.
/// This allows for data recovery or historical audit purposes.
/// </remarks>
public interface ISoftDeletable
{
    /// <summary>
    /// Gets or sets a value indicating whether the entity is marked as deleted.
    /// </summary>
    /// <value>
    /// <c>true</c> if the entity is marked as deleted; otherwise, <c>false</c>.
    /// </value>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was deleted.
    /// </summary>
    /// <value>
    /// The date and time when the entity was marked as deleted, or <c>null</c> if the entity is not deleted.
    /// </value>
    DateTime? DeletedAt { get; set; }
}
