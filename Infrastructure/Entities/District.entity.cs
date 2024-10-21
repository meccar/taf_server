using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;

public class DistrictEntity : BaseEntity
{
    public int Id { get; set; }
    public required string Uuid { get; set; }
    public required string Name { get; set; }
    public int CityId { get; set; }
    public required CityEntity City { get; set; }
    //public required BaseEntity baseEntity { get; set; }
}
