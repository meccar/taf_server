namespace Domain.Entities;

public class DistrictEntity : EntityBase
{
    public int Id { get; set; }
    public string Ulid { get; set; } = "";
    public string Name { get; set; } = "";
    public int CityId { get; set; }
    public CityEntity? City { get; set; }
}
