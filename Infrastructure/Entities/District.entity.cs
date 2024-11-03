using Infrastructure.SeedWork.Entities;

namespace Infrastructure.Entities;

public class DistrictEntity : BaseEntity
{
    public int Id { get; set; }
    public string Uuid { get; set; } = "";
    public string Name { get; set; } = "";
    public int CityId { get; set; }
    public CityEntity? City { get; set; }
}
