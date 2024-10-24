using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;

public class RoleEntity : BaseEntity
{
    public int Id { get; set; }
    public string Uuid { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public bool IsActive { get; set; }
    public bool IsGlobal { get; set; }
    public PermissionEntity[] Permissions { get; set; } = [];
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
    public UserAccountEntity? CreatedByUser { get; set; }
    public UserAccountEntity? UpdatedByUser { get; set; }
}
