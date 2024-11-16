namespace Domain.Entities;
public class UserLoginDataExternalEntity : EntityBase
{
    public int Id { get; set; }
    public string Ulid { get; set; } = "";
    public string SsocialId { get; set; } = "";
    // public Provider Provider { get; set; }
    public string Token { get; set; } = "";
    public DateTime EpiredAt { get; set; }
    // public UserAccountEntity? UserAccount { get; set; }
}