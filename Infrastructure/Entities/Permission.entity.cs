using Domain.SeedWork.Enums.Http;
using Infrastructure.SeedWork.Entities;

namespace Infrastructure.Entities;

public class PermissionEntity : BaseEntity
{
    public int Id { get; set; }
    public string Uuid { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Description { get; set; } = "";
    public RequestMethod? Method { get; set; }
    public bool IsActive { get; set; }
}
