using taf_server.Domain.SeedWork.Enums.Http;
using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;

public class PermissionEntity : BaseEntity
{
    public int Id { get; set; }
    public required string Uuid { get; set; }
    public required RequestMethod Method { get; set; }
    public required string Slug { get; set; }
    public required string Description { get; set; }
    public bool IsActive { get; set; }
    //public required BaseEntity baseEntity { get; set; }
}
