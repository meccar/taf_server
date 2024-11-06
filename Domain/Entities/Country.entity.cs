namespace Domain.Entities;

public class CountryEntity : EntityBase
{
    public int Id { get; set; }
    public string Ulid { get; set; } = "";

    public string Name { get; set; } = "";
    public string Initials { get; set; } = "";
}