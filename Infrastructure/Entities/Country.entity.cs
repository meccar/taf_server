using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;

public class CountryEntity : BaseEntity
{
    public int Id { get; set; }
    public required string Uuid { get; set; }

    public required string Name { get; set; }
    public required string Initials { get; set; }
    //public required BaseEntity baseEntity { get; set; }
}