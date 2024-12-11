namespace Domain.SeedWork.Interfaces;

/// <summary>
/// Represents entities that track creation and update timestamps.
/// </summary>
/// <remarks>
/// The <see cref="IDateTracking"/> interface is used for entities that need to track when they were created and when they were last updated.
/// This is commonly used for auditing, logging, or optimizing data retrieval.
/// </remarks>
public interface IDateTracking
{
    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    /// <value>
    /// The date and time of entity creation.
    /// </value>
    DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was last updated.
    /// </summary>
    /// <value>
    /// The date and time of the last update, or null if the entity has never been updated.
    /// </value>
    DateTime? UpdatedAt { get; set; }
}