using Infrastructure.SeedWork.Entities;

namespace Infrastructure.Entities;

public class CityEntity : BaseEntity
{
    public int Id { get; set; }
    public string Uuid { get; set; } = "";
    public string Name { get; set; } = "";
    public string Initials { get; set; } = "";
    public string Code { get; set; } = "";
    public int StateId { get; set; }
    public StateEntity? State { get; set; }
}
