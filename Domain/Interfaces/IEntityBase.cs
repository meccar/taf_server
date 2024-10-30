using Domain.SeedWork.Interfaces;

namespace Domain.Interfaces;

/// <summary>
/// Represents the base interface for entities in the domain, providing common 
/// functionality for date tracking and soft deletion.
/// </summary>
/// <remarks>
/// This interface inherits from <see cref="IDateTracking"/> and <see cref="ISoftDeletable"/>,
/// ensuring that implementing entities have properties for tracking creation and update
/// timestamps as well as indicating whether they are marked as deleted.
/// </remarks>
public interface IEntityBase : IDateTracking, ISoftDeletable;
