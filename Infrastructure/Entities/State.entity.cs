using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;

public class StateEntity : BaseEntity
{
    public int Id { get; set; }
    public required string Uuid { get; set; }
    public required string Name { get; set; }
    public required string Initials { get; set; }
    public required string AreaCode { get; set; }
    public int CountryId { get; set; }
    public required CountryEntity Country { get; set; }
    //public required BaseEntity baseEntity { get; set; }
}
