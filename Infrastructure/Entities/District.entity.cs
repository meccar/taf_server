using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;

public class DistrictEntity : BaseEntity
{
    public int Id { get; set; }
    public string Uuid { get; set; } = "";
    public string Name { get; set; } = "";
    public int CityId { get; set; }
    public CityEntity? City { get; set; }
}
