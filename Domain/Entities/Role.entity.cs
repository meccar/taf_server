namespace Domain.Entities;

public class RoleEntity : EntityBase
{
    public int Id { get; set; }
    public string Ulid { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public bool IsActive { get; set; }
    public bool IsGlobal { get; set; }
    public PermissionEntity[] Permissions { get; set; } = [];
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
    // public UserAccountEntity? CreatedByUser { get; set; }
    // public UserAccountEntity? UpdatedByUser { get; set; }
}
