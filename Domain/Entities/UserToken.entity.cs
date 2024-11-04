using Domain.SeedWork.Enums.Token;

namespace Domain.Entities;

public class UserTokenEntity : EntityBase
{
    public int Id { get; set; }
    public int UserAccountId { get; set; }
    public UserTokenType Type { get; set; }
    public string IpAddress { get; set; } = "";
    public string UserAgent { get; set; } = "";
    public string Token { get; set; } = "";
    public DateTime ExpiredAt { get; set; }
    // public UserAccountEntity? User { get; set; }

}
