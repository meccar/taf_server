using Infrastructure.SeedWork.Entities;

namespace Infrastructure.Entities;

public class CountryEntity : BaseEntity
{
    public int Id { get; set; }
    public string Uuid { get; set; } = "";

    public string Name { get; set; } = "";
    public string Initials { get; set; } = "";
}