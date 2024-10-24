using taf_server.Domain.SeedWork.Enums.Token;
using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;

public class UserTokenEntity : BaseEntity
{
    public int Id { get; set; }
    public int UserAccountId { get; set; }
    public UserTokenType Type { get; set; }
    public string IpAddress { get; set; } = "";
    public string UserAgent { get; set; } = "";
    public string Token { get; set; } = "";
    public DateTime ExpiredAt { get; set; }
    public UserAccountEntity? User { get; set; }

}
