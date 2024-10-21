using taf_server.Domain.Interfaces;

namespace taf_server.Domain.Entities;
public abstract class EntityBase : IEntityBase
{
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; } = null;
}
