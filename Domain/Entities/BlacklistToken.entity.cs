namespace Domain.Entities;
public class BlacklistTokenEntity : EntityBase
{
    public int Id { get; set; }
    public int UserAccountId { get; set; }
    public string Token { get; set; } = "";
    public DateTime ExpiredAt { get; set; }
    // public UserAccountEntity? User { get; set; }
}