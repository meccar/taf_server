using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;

public class RoleEntity : BaseEntity
{
    public int Id { get; set; }
    public required string Uuid { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsGlobal { get; set; }
    public required PermissionEntity[] Permissions { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
    public required UserAccountEntity CreatedByUser { get; set; }
    public required UserAccountEntity UpdatedByUser { get; set; }
    //public required BaseEntity baseEntity { get; set; }
}
