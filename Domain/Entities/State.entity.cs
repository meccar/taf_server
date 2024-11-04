namespace Domain.Entities;

public class StateEntity : EntityBase
{
    public int Id { get; set; }
    public string Ulid { get; set; } = "";
    public string Name { get; set; } = "";
    public string Initials { get; set; } = "";
    public string AreaCode { get; set; } = "";
    public int CountryId { get; set; }
    public CountryEntity? Country { get; set; }
}
