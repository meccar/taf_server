using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;

public class CityEntity : BaseEntity
{
    public int Id { get; set; }
    public required string Uuid { get; set; }
    public required string Name { get; set; }
    public required string Initials { get; set; }
    public required string Code { get; set; }
    public int StateId { get; set; }
    public required StateEntity State { get; set; }
    //public required BaseEntity baseEntity { get; set; }
}
