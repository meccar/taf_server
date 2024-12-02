using Shared.Enums;

namespace Domain.Entities;

public class PermissionEntity : EntityBase
{
    public int Id { get; set; }
    public string Ulid { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Description { get; set; } = "";
    public ERequestMethod? Method { get; set; }
    public bool IsActive { get; set; }
}
