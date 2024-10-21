using taf_server.Domain.SeedWork.Enums.Token;
using taf_server.Infrastructure.SeedWork.Entities;

namespace taf_server.Infrastructure.Entities;

public class UserTokenEntity : BaseEntity
{
    public int Id { get; set; }
    public int UserAccountId { get; set; }
    public UserTokenType Type { get; set; }
    public required string IpAddress { get; set; }
    public required string UserAgent { get; set; }
    public required string Token { get; set; }
    public DateTime ExpiredAt { get; set; }
    public required UserAccountEntity User { get; set; }
    //public required BaseEntity baseEntity { get; set; }

}
