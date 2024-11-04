namespace Domain.Entities;

public class CityEntity : EntityBase
{
    public int Id { get; set; }
    public string Ulid { get; set; } = "";
    public string Name { get; set; } = "";
    public string Initials { get; set; } = "";
    public string Code { get; set; } = "";
    public int StateId { get; set; }
    public StateEntity? State { get; set; }
}
