using Infrastructure.SeedWork.Entities;

namespace Infrastructure.Entities;

public class StateEntity : BaseEntity
{
    public int Id { get; set; }
    public string Uuid { get; set; } = "";
    public string Name { get; set; } = "";
    public string Initials { get; set; } = "";
    public string AreaCode { get; set; } = "";
    public int CountryId { get; set; }
    public CountryEntity? Country { get; set; }
}
