using taf_server.Domain.Interfaces;

namespace taf_server.Domain.Entities;

/// <summary>
/// Serves as a base class for entities in the domain, providing common properties 
/// for tracking state and timestamps.
/// </summary>
/// <remarks>
/// This abstract class implements the <see cref="IEntityBase"/> interface and includes 
/// properties for marking an entity as deleted and for tracking its creation, 
/// update, and deletion timestamps.
/// </remarks>
public abstract class EntityBase : IEntityBase
{
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; } = null;
}
